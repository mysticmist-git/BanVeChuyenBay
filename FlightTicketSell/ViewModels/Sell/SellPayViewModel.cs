using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using FlightTicketSell.Models;
using System.Data.Entity.Core;
using FlightTicketSell.Models.SearchRelated;
using FlightTicketSell.Views.SearchViewMore;
using FlightTicketSell.Views;
using FlightTicketSell.ViewModels.Sell;
using FlightTicketSell.ViewModels.Search;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using System.IO;


namespace FlightTicketSell.ViewModels
{
    public class SellPayViewModel : BaseViewModel 
    {
        #region Commands

        /// <summary>
        /// The command to continue
        /// </summary>
        public ICommand ReturnCommand { get; set; }

        public ICommand PayCommand { get; set; }

        public ICommand LoadCommand { get; set; }


        #endregion
        public DetailFlilghtInfo FlightInfo { get; set; }
        public ObservableCollection<OverlayAirport_Search> MidAirports { get; set; }
        public string HoTen { get; set; }
        public string CMND { get; set; }
        public string SDT { get; set; }
        public string Email { get; set; }
        public int MaHangVe { get; set; }
        public string HangVe { get; set; }
        public string MaDuongBay { get; set; }

        public int GiaTien { get; set; }

        public string GiaTien_Convert_VND { get; set; }
        #region Constructor


        /// <summary>
        /// Default constructor
        /// </summary>
        public SellPayViewModel()
        {
            // Create commands
            ReturnCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.TicketInfoFilling);

            PayCommand = new RelayCommand<object>((p) => true, (p) => SaveTicketPay());

            LoadCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                using (var context = new FlightTicketSellEntities())
                {
                    try
                    {
                        GiaTien_Convert_VND = GiaTien.ToString() + "VND";

                        MaDuongBay = context.CHUYENBAYs.Where(result => result.MaChuyenBay == FlightInfo.MaChuyenBay).FirstOrDefault().MaDuongBay.ToString();

                        MidAirports = new ObservableCollection<OverlayAirport_Search>(
                                                                context.SANBAYTGs.Where(result =>
                                                                result.MaDuongBay.ToString() == MaDuongBay)
                                                                .Select(result => new OverlayAirport_Search
                                                                {
                                                                    ThuTu = result.ThuTu.ToString(),
                                                                    TenSanBay = context.SANBAYs.Where(x => x.MaSanBay == result.MaSanBay).FirstOrDefault().TenSanBay,
                                                                    GhiChu = result.GhiChu.ToString(),
                                                                    ThoiGianDung = result.ThoiGianDung.ToString()
                                                                }).ToList());

                        

                    }
                    catch (EntityException)
                    {
                        // TODO: messagebox vo
                        return;
                    }
                }
            });
        }

        #endregion

        #region Methods

        private void SaveTicketPay()
        {
            using (var context = new FlightTicketSellEntities())
            {
                
                try
                {
                    /*--------INSERT KHACH HANG -----------*/
                    // get the latest id to set new id (= latest_id + 1) to new customer
                    var latest_KH_id = context.KHACHHANGs.OrderByDescending(p => p.MaKhachHang).FirstOrDefault().MaKhachHang;
                    var MaKH = latest_KH_id + 1;
                    // insert new customer into database
                    var kh = new KHACHHANG()
                    {
                        MaKhachHang = MaKH,
                        HoTen = HoTen,
                        CMND = CMND,
                        SDT = SDT,
                        Email = Email
                    };

                    

                    // check if there is duplication
                    if (context.KHACHHANGs.Any(result => result.CMND == CMND))
                        MaKH = context.KHACHHANGs.Where(result => result.CMND == CMND).FirstOrDefault().MaKhachHang;
                    
                    else
                    {
                        context.KHACHHANGs.Add(kh);
                        context.SaveChanges();
                    }
                    
                    

                    /*--------INSERT VE -----------*/
                    var latest_VE_id = context.VEs.OrderByDescending(p => p.MaVe).FirstOrDefault().MaVe;

                    // insert new ticket to database
                    var ve = new VE()
                    {
                        MaVe = latest_VE_id + 1,
                        MaChuyenBay = FlightInfo.MaChuyenBay,
                        MaKhachHang = MaKH,
                        GiaTien = GiaTien,
                        NgayThanhToan = DateTime.Now,
                        MaHangVe = MaHangVe
                    };

                    context.VEs.Add(ve);
                    context.SaveChanges();

                    // messagebox to notify sucessful payment
                    DialogResult res = (DialogResult)System.Windows.MessageBox.Show("Mua ve thanh cong, quay lai trang chu", "He thong", MessageBoxButton.OK);

                    if (res == DialogResult.OK)
                    {
                        // Send email to user
                        string to = Email; //To address    

                        // enter your email
                        string from = "user@gmail.com"; //From address    

                        MailMessage message = new MailMessage(from, to);

                        string mailbody = "Your payment is sucessful!!!!";

                        message.Subject = "Flight ticket payment";

                        message.Body = mailbody;

                        message.BodyEncoding = Encoding.UTF8;

                        message.IsBodyHtml = true;

                        SmtpClient client = new SmtpClient("smtp.gmail.com", 587); //Gmail smtp    

                        System.Net.NetworkCredential basicCredential1 = new

                        System.Net.NetworkCredential("user@gmail.com", "password");

                        client.EnableSsl = true;

                        client.UseDefaultCredentials = false;

                        client.Credentials = basicCredential1;

                        try

                        {

                            client.Send(message);

                        }



                        catch (Exception ex)

                        {

                            throw ex;

                        }


                        // Go to main screen
                        IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.Search;

                    }

                }
                catch (EntityException)
                {
                    // TODO: messagebox vo
                    return;
                }
            }
        }

        #endregion
    }
}
