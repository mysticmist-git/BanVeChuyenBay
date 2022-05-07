using System;
using System.Windows.Input;
using FlightTicketSell.Views.Helper;
using System.Globalization;
using FlightTicketSell.Views.SearchViewMore;
using FlightTicketSell.ViewModels;

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

        #endregion

        #region Commands

        public ICommand Open_Window_DescriptionCustomer_Command { get; set; }

        #endregion

        public PlaceReservation()
        {
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
        }

    }
}
