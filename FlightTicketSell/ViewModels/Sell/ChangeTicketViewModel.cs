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
using FlightTicketSell.Models.SearchRelated;
using System.Data.Entity;
using System.Globalization;

namespace FlightTicketSell.ViewModels
{
    public class ChangeTicketViewModel : BaseViewModel
    {
        #region Private Members

        /// <summary>
        /// The intertal of time the customer can cancel the booking
        /// </summary>
        private int _cancelDays;

        #endregion

        #region Commands

        /// <summary>
        /// The command to continue
        /// </summary>
        public ICommand PrintTicketCommand { get; set; }

        /// <summary>
        /// The command to cancel current booking
        /// </summary>
        public ICommand CancelTicketCommand { get; set; }

        /// <summary>
        /// Return to previous view: Sold ticket, bookings
        /// </summary>
        public ICommand ReturnCommand { get; set; }

        /// <summary>
        /// Command executed on view load
        /// </summary>
        public ICommand LoadCommand { get; set; }

        #endregion

        /// <summary>
        /// Contains flight information
        /// </summary>
        public DetailFlilghtInfo FlightInfo { get => IoC.IoC.Get<FlightDetailViewModel>().FlightInfo; }

        /// <summary>
        /// The overlay airports of this flight
        /// </summary>
        public ObservableCollection<OverlayAirport_Search> OverlayAirports { get => IoC.IoC.Get<FlightDetailViewModel>().OverlayAirport; }

        /// <summary>
        /// Stores customers that this book contains
        /// </summary>
        public ObservableCollection<Customer> Customers { get; set; }

        /// <summary>
        /// Stores booking information
        /// </summary>
        public PlaceReservation BookingInfo { get; set; }

        /// <summary>
        /// The deadline for canceling the booking
        /// </summary>
        public DateTime HanChotHuyVe { get => FlightInfo.NgayGio.AddDays(_cancelDays); }

        /// <summary>
        /// The display deadline for canceling the booking
        /// </summary>
        public string DisplayCancelDeadline { get => HanChotHuyVe.ToString("HH:mm dd/mm/yyyy", new CultureInfo("vi-VN")); }

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ChangeTicketViewModel()
        {
            // Create commands

            ReturnCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.TicketSoldOrBooked);

            //PrintTicketCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.);

            //CancelTicketCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.);

            LoadCommand = new RelayCommand<object>((p) => true, async (p) =>
            {
                using (var context = new FlightTicketSellEntities())
                {
                    try
                    {
                        // Get deadline to cancel the booking
                        _cancelDays = context.THAMSOes.Where(ts => ts.TenThamSo == "ThoiGianHuyDatVe").FirstOrDefault().GiaTri;

                        // Get KHACHAHNG
                        var result = await context.DATCHOes
                                .Where(dc => dc.MaChuyenBay == FlightInfo.MaChuyenBay && dc.MaDatCho == BookingInfo.MaDatCho)
                                .Select(dc => dc.CHITIETDATCHOes
                                    .Select(ctdc => ctdc.KHACHHANG)
                                    )
                                .FirstOrDefaultAsync();

                        // Convert KHACHHANG to Customer and add it to Customers list
                        Customers = new ObservableCollection<Customer>();
                        for (int i = 1; i <= result.Count(); i++)
                            Customers.Add(new Customer(result.ElementAt(i - 1)) { Index = i });
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
