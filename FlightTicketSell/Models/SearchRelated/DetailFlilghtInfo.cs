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
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public DetailFlilghtInfo() { }

        #endregion
    }
}
