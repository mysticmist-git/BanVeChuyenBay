using FlightTicketSell.Helpers;
using FlightTicketSell.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
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
            using (var context = new FlightTicketSellEntities())
            {
                try
                {
                    var chuyenbay = context.CHUYENBAYs.Where(h => h.MaChuyenBay == cHUYENBAY.MaChuyenBay).FirstOrDefault();

                    print_TenHangVe = hANGVE.TenHangVe.ToUpper();

                    print_TenKhachHang = kHACHHANG.HoTen.ToUpper();

                    print_NgayBay = chuyenbay.NgayGio.Day + "/" + chuyenbay.NgayGio.Month + "/" + chuyenbay.NgayGio.Year;

                    print_GioBay = chuyenbay.NgayGio.Hour + ":" + chuyenbay.NgayGio.Minute;

                    print_SBDi = chuyenbay.DUONGBAY.SANBAY1.TenSanBay.ToString().ToUpper();

                    print_SBDi_Code = "/" + chuyenbay.DUONGBAY.SANBAY1.VietTat.ToString();

                    print_SBDen = chuyenbay.DUONGBAY.SANBAY.TenSanBay.ToString().ToUpper();

                    print_SBDen_Code = "/" + chuyenbay.DUONGBAY.SANBAY.VietTat.ToString();

                    print_TGB = chuyenbay.DUONGBAY.ThoiGianBay.ToString();

                    print_MaChuyenBay = print_SBDi_Code + print_SBDen_Code + "-" + chuyenbay.MaChuyenBay.ToString();
                }
                catch (EntityException e)
                {
                    NotifyHelper.ShowEntityException(e); ;
                }
            }
        }

    }
}
