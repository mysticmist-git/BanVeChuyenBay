using System;
using System.Windows.Input;
using FlightTicketSell.Views.Helper;
using System.Globalization;
using FlightTicketSell.Views.SearchViewMore;
using FlightTicketSell.ViewModels;
using FlightTicketSell.Models.Enums;
using System.Windows;

namespace FlightTicketSell.ViewModels.Search
{
    public class PlaceReservation : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// The reservatoin ID
        /// </summary>
        public int MaDatCho { get; set; }

        /// <summary>
        /// Display reservation ID
        /// </summary>
        public string DisplayReservationID { get => IoC.IoC.Get<FlightDetailViewModel>().FlightInfo.DisplayFlightCode + "-D" + MaDatCho; }

        /// <summary>
        /// The ID of the customer who book reservation
        /// </summary>
        public int MaKhachDat { get; set; }

        /// <summary>
        /// The name of the customer who book the reservation
        /// </summary>
        public string TenKhachDat { get; set; }

        /// <summary>
        /// The number of places reserved
        /// </summary>
        public int SoCho { get; set; }

        /// <summary>
        /// The name of the ticket tier reserved
        /// </summary>
        public string TenHangVe { get; set; }

        /// <summary>
        /// The date customer book the reservation
        /// </summary>
        public DateTime NgayDatVe { get; set; }

        /// <summary>
        /// The display book date
        /// </summary>
        public string DisplayBookDate { get => NgayDatVe.ToString("HH:mm dd/mm/yyyy", new CultureInfo("vi-VN")); }

        /// <summary>
        /// The price of each reservation
        /// </summary>
        public decimal GiaTien_Ve { get; set; }

        /// <summary>
        /// The display total price of the reservation
        /// </summary>
        public string DisplayReservationPrice { get => ReportHelper.VietnamCurrencyConvert(GiaTien_Ve) + " VNĐ"; }

        /// <summary>
        /// The state of this reservation
        /// ChuaDoi: It hasn't been changed for tickets
        /// DaDoi: It has been changed for tickets
        /// DaHuy: It has been canceled
        /// </summary>
        public string TrangThai { get; set; }

        public BookingState BookingState 
        { 
            get
            {
                switch (TrangThai)
                {
                    case "ChuaDoi":
                        return BookingState.NotChanged;
                    case "DaDoi":
                        return BookingState.Changed;
                    case "DaHuy":
                        return BookingState.NotChanged;
                    default:
                        return BookingState.NotChanged;
                }
            }

            set
            {
                switch (value)
                {
                    case BookingState.NotChanged:
                        TrangThai = "ChuaDoi";
                        break;
                    case BookingState.Changed:
                        TrangThai = "DaDoi";
                        break;
                    case BookingState.Cancel:
                        TrangThai = "DaHuy";
                        break;
                }
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// The command to open the detail description customer widow
        /// </summary>
        public ICommand Open_Window_DescriptionCustomer_Command { get; set; }
        
        /// <summary>
        /// The command to show more information about the booking
        /// </summary>
        public ICommand ShowMoreCommand { get; set; }

        #endregion

        public PlaceReservation()
        {
            // Create commands 
            Open_Window_DescriptionCustomer_Command = new RelayCommand<object>((p) => true, (p) =>
            {
                var view = new DetailCustomers()
                {
                    DataContext = new CustomersDetailViewModel()
                    {
                        CustomerDetailType = Models.Enums.CustomerDetailType.Reservation,
                        Info = (this as object)
                    }
                };

                view.ShowDialog();
            });
            ShowMoreCommand = new RelayCommand<object>(p => true, p =>
            {
                IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.ChangeTicket;
            });
        }
    }
}
