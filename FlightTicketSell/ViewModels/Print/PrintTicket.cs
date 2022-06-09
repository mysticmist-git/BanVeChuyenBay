using FlightTicketSell.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightTicketSell.ViewModels.Search
{
    public class PrintTicket : BaseViewModel
    {
        public string print_TenHangVe { get; set; }
        public string print_TenKhachHang { get; set; }
        public string print_MaChuyenBay { get; set; }
        public string print_NgayBay { get; set; }
        public string print_GioBay { get; set; }
        public string print_SBDi { get; set; }
        public string print_SBDi_Code { get; set; }
        public string print_SBDen { get; set; }
        public string print_SBDen_Code { get; set; }
        public string print_TGB { get; set; }

        public PrintTicket() { }
        public PrintTicket(HANGVE hANGVE, KHACHHANG kHACHHANG, CHUYENBAY cHUYENBAY)
        {
            print_TenHangVe = hANGVE.TenHangVe.ToUpper();

            print_TenKhachHang = kHACHHANG.HoTen.ToUpper();

            print_NgayBay = cHUYENBAY.NgayGio.Day + "/" + cHUYENBAY.NgayGio.Month + "/" + cHUYENBAY.NgayGio.Year;

            print_GioBay = cHUYENBAY.NgayGio.Hour + ":" + cHUYENBAY.NgayGio.Minute;

            print_SBDi = cHUYENBAY.DUONGBAY.SANBAY1.TenSanBay.ToString().ToUpper();

            print_SBDi_Code = cHUYENBAY.DUONGBAY.SANBAY1.VietTat.ToString();

            print_SBDen = cHUYENBAY.DUONGBAY.SANBAY.TenSanBay.ToString().ToUpper();

            print_SBDen_Code = cHUYENBAY.DUONGBAY.SANBAY.VietTat.ToString();

            print_TGB = cHUYENBAY.DUONGBAY.ThoiGianBay.ToString();

            print_MaChuyenBay = print_SBDi_Code + print_SBDen_Code + "-" + cHUYENBAY.MaChuyenBay.ToString();

        }

    }
}
