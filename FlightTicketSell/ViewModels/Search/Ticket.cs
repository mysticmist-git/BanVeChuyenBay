using System;
using System.Windows.Input;
using FlightTicketSell.Views.Helper;
using System.Globalization;
using FlightTicketSell.ViewModels;
using FlightTicketSell.Helpers;
using FlightTicketSell.Views;

namespace FlightTicketSell.ViewModels.Search
{
    public class Ticket : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// Ticket ID
        /// </summary>
        public int MaVe { get; set; }

        /// <summary>
        /// Display ticket ID
        /// </summary>
        public string DisplayTicketID { get => IoC.IoC.Get<FlightDetailViewModel>().FlightInfo.DisplayFlightCode + "-M" + MaVe; }

        /// <summary>
        /// The ticket tier
        /// </summary>
        public string TenHangVe { get; set; }

        /// <summary>
        /// The pay date
        /// </summary>
        public DateTime NgayThanhToan { get; set; }

        /// <summary>
        /// The display pay date
        /// </summary>
        public string DisplayPayDate { get => NgayThanhToan.ToString("HH:mm dd/MM/yyyy", new CultureInfo("vi-VN")); }

        /// <summary>
        /// Ticket price
        /// </summary>
        public decimal GiaTien { get; set; }

        /// <summary>
        /// The display price of the ticket
        /// </summary>
        public string DisplayTicketPrice { get => FormatHelper.VietnamCurrencyFormat(GiaTien) + " VNĐ"; }

        /// <summary>
        /// Customer name 
        /// </summary>
        public string TenKhachHang { get; set; }

        /// <summary>
        /// Customer code
        /// </summary>
        public int MaKhachHang { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// Open customer detail window
        /// </summary>
        public ICommand Open_Window_DescriptionCustomer_Command { get; set; }

        #endregion

        public Ticket()
        {
            Open_Window_DescriptionCustomer_Command = new RelayCommand<object>((p) => true, (p) =>
            {
                var view = new DetailCustomers()
                {
                    DataContext = new CustomersDetailViewModel()
                    {
                        CustomerDetailType = Models.Enums.CustomerDetailType.Ticket,
                        Info = (this as object)
                    }
                };

                view.ShowDialog();
            });

        }

    }
}
