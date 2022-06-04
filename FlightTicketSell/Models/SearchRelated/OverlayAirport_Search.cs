using FlightTicketSell.ViewModels;

namespace FlightTicketSell.Models.SearchRelated
{
    public class OverlayAirport_Search : BaseViewModel
    {
        public int MaSanBay { get; set; }        
        public int MaDuongBay { get; set; }
        public string ThuTu { get; set; }
        public string TenSanBay { get; set; }
        public string ThoiGianDung { get; set; }
        public string GhiChu { get; set; }
    }
}
