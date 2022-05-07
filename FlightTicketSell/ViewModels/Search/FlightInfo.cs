using FlightTicketSell.Models.SearchRelated;
using FlightTicketSell.ViewModels;
using FlightTicketSell.Views.Helper;
using System.Globalization;
using System.Windows.Input;

namespace FlightTicketSell.Models
{
    /// <summary>
    /// The model stores flight information
    /// </summary>
    public class FlightInfo : GetFlightData_Result
    {
        #region Public Properties
        
        /// <summary>
        /// The flight code
        /// </summary>
        public string DisplayFlightCode { get => SanBayDiVietTat + SanBayDenVietTat + "-" + MaChuyenBay.ToString(); }

        /// <summary>
        /// The depart date
        /// </summary>
        public string DisplayDepartDate { get => NgayGio.ToString("HH:mm dd/mm/yyyy", new CultureInfo("vi-VN")); }

        /// <summary>
        /// The standard price the flight ticket
        /// </summary>
        public string DisplayPrice { get => ReportHelper.VietnamCurrencyConvert(GiaVe) + " VNĐ"; }

        #endregion

        #region Commands

        /// <summary>
        /// What to do when click "Detail" button on each flight infos
        /// </summary>
        public ICommand Open_Window_DescriptionTicketFlight_Command { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Copy constructor
        /// </summary>
        public FlightInfo(GetFlightData_Result result) : this()
        {
            // Copy information
            MaChuyenBay = result.MaChuyenBay;
            MaSanBayDi = result.MaSanBayDi;
            MaSanBayDen = result.MaSanBayDen;
            SanBayDi = result.SanBayDi;
            SanBayDen = result.SanBayDen;
            SanBayDiVietTat = result.SanBayDiVietTat;
            SanBayDenVietTat = result.SanBayDenVietTat;
            NgayGio = result.NgayGio;
            SoDiemDung = result.SoDiemDung;
            SoHangVe = result.SoHangVe;
            SoHangVe = result.SoHangVe;
            GiaVe= result.GiaVe;
            GheTrong = result.GheTrong;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public FlightInfo()
        {
            Open_Window_DescriptionTicketFlight_Command = new RelayCommand<object>((p) => true, (p) =>
            {
                IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.FlightTicket;
                IoC.IoC.Get<FlightDetailViewModel>().FlightInfo = new DetailFlilghtInfo(this);
            });
        }

        #endregion
    }
}
