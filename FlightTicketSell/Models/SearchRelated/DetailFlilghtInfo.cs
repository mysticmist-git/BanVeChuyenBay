using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightTicketSell.Models.SearchRelated
{
    public class DetailFlilghtInfo : FlightInfo
    {
        #region Public Properties

        /// <summary>
        /// Flight time
        /// </summary>
        public int ThoiGianBay { get; set; }

        /// <summary>
        /// Flight route code
        /// </summary>
        public int MaDuongBay { get; set; }

        /// <summary>
        /// Indicates if flight has any seat left
        /// </summary>
        public bool AnySeatLeft { get => this.GheTrong != 0; }

        /// <summary>
        /// Indicates if it is able to sell and book
        /// </summary>
        public bool IsAbleToSellAndBook { get => AnySeatLeft && TrangThai == 1; }

        #endregion

        #region Construcotr

        /// <summary>
        /// Copy constructor
        /// </summary>
        public DetailFlilghtInfo(FlightInfo flight)
        {
            // Copy information
            MaChuyenBay = flight.MaChuyenBay;
            MaSanBayDi = flight.MaSanBayDi;
            MaSanBayDen = flight.MaSanBayDen;
            SanBayDi = flight.SanBayDi;
            SanBayDen = flight.SanBayDen;
            SanBayDiVietTat = flight.SanBayDiVietTat;
            SanBayDenVietTat = flight.SanBayDenVietTat;
            NgayGio = flight.NgayGio;
            SoDiemDung = flight.SoDiemDung;
            SoHangVe = flight.SoHangVe;
            SoHangVe = flight.SoHangVe;
            GiaVe = flight.GiaVe;
            GheTrong = flight.GheTrong;
            DaKhoiHanh = flight.DaKhoiHanh;
            TrangThai = flight.TrangThai;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public DetailFlilghtInfo() { }

        #endregion
    }
}
