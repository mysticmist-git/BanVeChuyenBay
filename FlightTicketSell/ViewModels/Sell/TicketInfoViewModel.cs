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
using System.Collections.ObjectModel;
using FlightTicketSell.ViewModels.Sell;




namespace FlightTicketSell.ViewModels
{
    public class TicketInfoViewModel : BaseViewModel
    {
        #region Commands

        /// <summary>
        /// The command to continue
        /// </summary>
        public ICommand ContinueCommand { get; set; }

        public ICommand ReturnCommand { get; set; }

        public ICommand LoadCommand { get; set; }

        public ICommand HienGiaTien { get; set; }

        public ObservableCollection<TicketClass> TicketClasses { get; set; }

        public DetailFlilghtInfo FlightInfo { get; set; }

        public int MaChuyenBay { get; set; }

        public int GiaTien { get; set; }

        public TicketClass HangVe { get; set; }
        public string HoTen { get; set; }
        public string CMND { get; set; }
        public string SDT { get; set; }
        public string Email { get; set; }

        public decimal HeSo {get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public TicketInfoViewModel()
        {
            // Create commands
           

            ContinueCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.SellPay;
                try
                {
                        IoC.IoC.Get<SellPayViewModel>().FlightInfo = new DetailFlilghtInfo(FlightInfo);
                        IoC.IoC.Get<SellPayViewModel>().HoTen = HoTen;
                        IoC.IoC.Get<SellPayViewModel>().SDT = SDT;
                        IoC.IoC.Get<SellPayViewModel>().CMND = CMND;
                        IoC.IoC.Get<SellPayViewModel>().Email = Email;
                        IoC.IoC.Get<SellPayViewModel>().GiaTien = GiaTien.ToString() + " VND";
                        IoC.IoC.Get<SellPayViewModel>().HangVe = HangVe.Name;
                }
                catch(NullReferenceException) {
                    MessageBox.Show("Vui long nhap day du thong tin", "Thong tin bi thieu!!");
                    return;
                }


            });

            ReturnCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.FlightDetail);

            LoadCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                using (var context = new FlightTicketSellEntities())
                {
                    try
                    {
                        TicketClasses = new ObservableCollection<TicketClass>(context.CHUYENBAYs.Where(result =>
                                                                result.MaChuyenBay == FlightInfo.MaChuyenBay).FirstOrDefault()
                                                                .CHITIETHANGVEs.Select(cthv => new TicketClass
                                                                {
                                                                    ID = cthv.MaHangVe,
                                                                    Name = cthv.HANGVE.TenHangVe,
                                                                    
                                                                }).ToList());

                        
                    }
                    catch (EntityException)
                    {
                        // TODO: messagebox vo
                        return;
                    }
                }
            });

            HienGiaTien = new RelayCommand<object>((p) => true, (p) =>
            {
            using (var context = new FlightTicketSellEntities())
            {
                try
                {

                        if (HangVe != null)
                        {
                            HeSo = context.HANGVEs.Where(result => result.TenHangVe == HangVe.Name).FirstOrDefault().HeSo;

                            GiaTien = System.Convert.ToInt32(context.CHUYENBAYs.Where(result =>
                                                                result.MaChuyenBay == FlightInfo.MaChuyenBay).FirstOrDefault()
                                                                .GiaVe * HeSo);

                        }



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

    }
}
