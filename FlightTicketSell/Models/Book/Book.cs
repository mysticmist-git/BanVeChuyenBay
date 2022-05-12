using FlightTicketSell.Helpers;
using FlightTicketSell.Models.Enums;
using FlightTicketSell.ViewModels;
using System.Globalization;

namespace FlightTicketSell.Models
{
    public class Book : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// Stores information about the person who booked
        /// </summary>
        public Customer ThongTinNguoiDat { get; set; } = new Customer();

        /// <summary>
        /// The bookin code
        /// </summary>
        public int MaDatCho { get; set; }

        /// <summary>
        /// Display reservation ID
        /// </summary>
        public string DisplayReservationID { get => IoC.IoC.Get<FlightDetailViewModel>().FlightInfo.DisplayFlightCode + "-D" + MaDatCho; }

        /// <summary>
        /// The ticket tier code this book take
        /// </summary>
        public int MaHangVe { get; set; }

        /// <summary>
        /// The name of the ticket tier reserved
        /// </summary>
        public string TenHangVe { get; set; }

        /// <summary>
        /// The flight code this book take
        /// </summary>
        public int MaChuyenBay { get; set; }

        /// <summary>
        /// The time this book take
        /// </summary>
        public System.DateTime NgayGioDat { get; set; }

        /// <summary>
        /// The display book date
        /// </summary>
        public string DisplayBookDate { get => NgayGioDat.ToString("HH:mm dd/MM/yyyy", new CultureInfo("vi-VN")); }

        /// <summary>
        /// The number of ticket this book take
        /// </summary>
        public int SoVeDat { get; set; }

        /// <summary>
        /// The total price of this book
        /// </summary>
        public decimal GiaTien { get => GiaTien_Ve * SoVeDat; }

        /// <summary>
        /// The display of total price of this book
        /// </summary>
        public string DisplayTicketPrice { get => FormatHelper.VietnamCurrencyFormat(GiaTien) + " VNĐ"; }

        public decimal GiaTien_Ve { get; set; }

        /// <summary>
        /// The display of total price of this book
        /// </summary>
        public string DisplayTotalPrice { get => FormatHelper.VietnamCurrencyFormat(GiaTien) + " VNĐ"; }

        /// <summary>
        /// The take of this book
        /// </summary>
        public string TrangThai { get; set; }

        /// <summary>
        /// The state of this reservation.
        /// This is a interface for <see cref="TrangThai"/>.
        /// </summary>
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
                        return BookingState.Cancel;
                    default:
                        return BookingState.None;
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
    }
}
