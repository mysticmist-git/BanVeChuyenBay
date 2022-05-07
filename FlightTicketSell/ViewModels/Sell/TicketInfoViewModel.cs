using FlightTicketSell.Views;
using FlightTicketSell.Models;
using FlightTicketSell.ViewModels.Sell;
using FlightTicketSell.ViewModels.Search;
using FlightTicketSell.Views.SearchViewMore;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;
using System.Data.Entity.Core;
using System.Windows;
using System;


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

        public int MaChuyenBay { get; set; }

        public int GiaTien { get; set; }

        public TicketClass HangVe { get; set; }


        public decimal HeSo {get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public TicketInfoViewModel()
        {
            // Create commands
            ContinueCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.SellPay);

            ReturnCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.FlightDetail);

            LoadCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                using (var context = new FlightTicketSellEntities())
                {
                    try
                    {
                        TicketClasses = new ObservableCollection<TicketClass>(context.CHUYENBAYs.Where(result =>
                                                                result.MaChuyenBay == 1).FirstOrDefault()
                                                                .CHITIETHANGVEs.Select(cthv => new TicketClass
                                                                {
                                                                    ID = cthv.MaHangVe,
                                                                    Name = cthv.HANGVE.TenHangVe,
                                                                    Price = 1000,
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
                        HeSo = context.HANGVEs.Where(result => result.TenHangVe == HangVe.Name).FirstOrDefault().HeSo;


                        if (HangVe != null)
                            GiaTien = System.Convert.ToInt32(context.CHUYENBAYs.Where(result =>
                                                                result.MaChuyenBay == 1).FirstOrDefault()
                                                                .GiaVe * HeSo);
                                                                
                      



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
