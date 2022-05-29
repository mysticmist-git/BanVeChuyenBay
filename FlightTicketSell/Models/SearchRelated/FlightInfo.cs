using FlightTicketSell.Helpers;
using FlightTicketSell.Models.SearchRelated;
using FlightTicketSell.ViewModels;
using FlightTicketSell.Views.Helper;
using System;
using System.Globalization;
using System.Windows.Input;

namespace FlightTicketSell.Models
{
    /// <summary>
    /// The model stores flight information
    /// </summary>
    public class FlightInfo : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// The flight code
        /// </summary>
        public int MaChuyenBay { get; set; }

        /// <summary>
        /// The depart airport code
        /// </summary>
        public int MaSanBayDi { get; set; }

        /// <summary>
        /// The destination airport code
        /// </summary>
        public int MaSanBayDen { get; set; }

        /// <summary>
        /// The depart airport name
        /// </summary>
        public string SanBayDi { get; set; }

        /// <summary>
        /// The destination airport name
        /// </summary>
        public string SanBayDen { get; set; }

        /// <summary>
        /// The depart airport short-name
        /// </summary>
        public string SanBayDiVietTat { get; set; }

        /// <summary>
        /// The destination airport short-name
        /// </summary>
        public string SanBayDenVietTat { get; set; }

        /// <summary>
        /// The depart date
        /// </summary>
        public System.DateTime NgayGio { get; set; }

        /// <summary>
        /// The number of overlay airport this flight have
        /// </summary>
        public Nullable<int> SoDiemDung { get; set; }

        /// <summary>
        /// The number of ticket tier this flight have
        /// </summary>
        public Nullable<int> SoHangVe { get; set; }

        /// <summary>
        /// The standard price of this flight ticket
        /// </summary>
        public decimal GiaVe { get; set; }

        /// <summary>
        /// The number seat left
        /// </summary>
        public Nullable<int> GheTrong { get; set; }

        public Nullable<int> SoGheDat { get; set; }

        /// <summary>
        /// Indicates of this flight has already departed or not
        /// </summary>
        public bool DaKhoiHanh { get; set; }

        /// <summary>
        /// The flight code
        /// </summary>
        public string DisplayFlightCode { get => SanBayDiVietTat + SanBayDenVietTat + "-" + MaChuyenBay.ToString(); }

        /// <summary>
        /// The depart date
        /// </summary>
        public string DisplayDepartDate { get => NgayGio.ToString("HH:mm dd/MM/yyyy", new CultureInfo("vi-VN")); }

        /// <summary>
        /// The standard price the flight ticket
        /// </summary>
        public string DisplayPrice { get => FormatHelper.VietnamCurrencyFormat(GiaVe) + " VNĐ"; }

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
            MaSanBayDi = (int)result.MaSanBayDi;
            MaSanBayDen = (int)result.MaSanBayDen;
            SanBayDi = result.SanBayDi;
            SanBayDen = result.SanBayDen;
            SanBayDiVietTat = result.SanBayDiVietTat;
            SanBayDenVietTat = result.SanBayDenVietTat;
            NgayGio = result.NgayGio;
            SoDiemDung = result.SoDiemDung;
            SoHangVe = result.SoHangVe;
            SoHangVe = result.SoHangVe;
            GiaVe= result.GiaVe;
            if(result.SoGheDat == null)
            {
                SoGheDat = 0;
            }
            else
            {
                SoGheDat = result.SoGheDat;
            }
            GheTrong = result.GheTrong;
            DaKhoiHanh = result.DaKhoiHanh;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public FlightInfo()
        {
            Open_Window_DescriptionTicketFlight_Command = new RelayCommand<object>((p) => true, (p) =>
            {
                IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.FlightDetail;
                IoC.IoC.Get<FlightDetailViewModel>().FlightInfo = new DetailFlilghtInfo(this);
            });

            
        }

        #endregion
    }
}
