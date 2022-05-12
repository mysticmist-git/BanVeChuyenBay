using FlightTicketSell.Helpers;
using FlightTicketSell.ViewModels;

namespace FlightTicketSell.Models
{
    public class Book : BaseViewModel
    {
        #region Public Properties

        public Customer ThongTinNguoiDat { get; set; } = new Customer();
        public int MaDatCho { get; set; }
        public int MaHangVe { get; set; }
        public int MaChuyenBay { get; set; }
        public System.DateTime NgayGioDat { get; set; }
        public int SoVeDat { get; set; }
        public decimal GiaTien { get; set; }

        public string DisplayTotalPrice { get => FormatHelper.VietnamCurrencyFormat(GiaTien) + " VNĐ"; }
        
        public string TrangThai { get; set; } 

        #endregion
    }
}
