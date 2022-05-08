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

        /// <summary>
        /// The command to save ticket, customer
        /// </summary>
        public ICommand PayCommand { get; set; }

        /// <summary>
        /// The command execute on view load
        /// </summary>
        public ICommand LoadCommand { get; set; }

        #endregion

        #region Public Properties

        /// <summary>
        /// Stores flight information
        /// </summary>
        public DetailFlilghtInfo FlightInfo { get; set; }

        /// <summary>
        /// Stores overlay airports of this flight
        /// </summary>
        public ObservableCollection<OverlayAirport_Search> OverlayAirports { get; set; }

        /// <summary>
        /// Customer name
        /// </summary>
        public string HoTen { get; set; }

        /// <summary>
        /// The customer ID
        /// </summary>
        public string CMND { get; set; }

        /// <summary>
        /// The phone number of the customer
        /// </summary>
        public string SDT { get; set; }

        /// <summary>
        /// The email of the customer
        /// </summary>
        public string Email { get; set; }
        public int MaHangVe { get; set; }
        public string HangVe { get; set; }

        /// <summary>
        /// The flight route this flight take
        /// </summary>
        public string MaDuongBay { get; set; }

        public int GiaTien { get; set; }
        #endregion
        public string GiaTien_Convert_VND { get; set; }
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public SellPayViewModel()
        {
            // Create commands
            ReturnCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.TicketInfoFilling);

            PayCommand = new RelayCommand<object>((p) => true, (p) => SaveTicketInformation());

            LoadCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                using (var context = new FlightTicketSellEntities())
                {
                    try
                    {
                        GiaTien_Convert_VND = GiaTien.ToString() + "VND";

                        MaDuongBay = context.CHUYENBAYs.Where(result => result.MaChuyenBay == FlightInfo.MaChuyenBay).FirstOrDefault().MaDuongBay.ToString();

                        OverlayAirports = new ObservableCollection<OverlayAirport_Search>(
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

        /// <summary>
        /// Saves customer information, ticket information down database
        /// </summary>
        private void SaveTicketInformation()
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
                        string from = "flightsystem53@gmail.com"; //From address    

                        MailMessage message = new MailMessage(from, to);

                        string mailbody = "<head>" +
            "Here comes some logo" +
          "</head>" +
          "<body>" +
            "<h1>Payment comfirmation: sucessfull</h1>" + Environment.NewLine +
            "<a>Dear  </a>" + HoTen + Environment.NewLine +
            "<a>Here are some information about the ticket </a>" + Environment.NewLine +
            "<a>Your flight code is </a>" + FlightInfo.DisplayFlightCode + Environment.NewLine +
            "<a>The flight will start at </a>" + FlightInfo.NgayGio + Environment.NewLine +
            "<a>Please take note the information above!!!  </a>" + Environment.NewLine
            + "<a>Hope you have a great flight, </a>" +
          "</body>";

                        message.Subject = "Flight ticket payment";

                        message.Body = mailbody;

                        message.BodyEncoding = Encoding.UTF8;

                        message.IsBodyHtml = true;

                        SmtpClient client = new SmtpClient("smtp.gmail.com", 587); //Gmail smtp    

                        System.Net.NetworkCredential basicCredential1 = new

                        System.Net.NetworkCredential("flightsystem53@gmail.com", "flightsystem123");

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
