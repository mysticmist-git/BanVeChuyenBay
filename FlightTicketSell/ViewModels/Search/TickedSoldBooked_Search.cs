using FlightTicketSell.Views;
using FlightTicketSell.Models;
using FlightTicketSell.ViewModels.Search;
using FlightTicketSell.Views.SearchViewMore;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;
using System.Data.Entity.Core;
using System.Windows;

namespace FlightTicketSell.ViewModels.Search
{
    public class TickedSoldBooked_Search : BaseViewModel
    {
        public string MaChuyenBay { get; set; }
        public string MaKhachHang { get; set; }
        public string MaKhachDat { get; set; }

        public ICommand ReturnCommand { get; set; }
        public ICommand LoadCommand { get; set; }

        public ObservableCollection<TicketSold> TickedSolds { get; set; }
        public ObservableCollection<Booked> Bookeds { get; set; }


        public TickedSoldBooked_Search()
        {
            ReturnCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.FlightTicket);

            LoadCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                using (var context = new FlightTicketSellEntities())
                {
                    try
                    {
                        TickedSolds = new ObservableCollection<TicketSold>(
                                                                context.VEs.Where(result =>
                                                                result.MaChuyenBay.ToString() == MaChuyenBay)
                                                                .Select(result => new TicketSold
                                                                {
                                                                    MaVe = result.MaVe.ToString(),
                                                                    TenKhachHang = context.KHACHHANGs.Where(x => x.MaKhachHang == result.MaKhachHang).FirstOrDefault().HoTen,
                                                                    MaKhachHang = result.MaKhachHang.ToString(),
                                                                    SDT = context.KHACHHANGs.Where(x => x.MaKhachHang == result.MaKhachHang).FirstOrDefault().SDT,
                                                                    Email = context.KHACHHANGs.Where(x => x.MaKhachHang == result.MaKhachHang).FirstOrDefault().Email,
                                                                    CMND = context.KHACHHANGs.Where(x => x.MaKhachHang == result.MaKhachHang).FirstOrDefault().SDT,
                                                                    TenHangVe = context.HANGVEs.Where(x => x.MaHangVe == result.MaHangVe).FirstOrDefault().TenHangVe,
                                                                    NgayThanhToan = result.NgayThanhToan,
                                                                    GiaTien = result.GiaTien.ToString()
                                                                }).ToList()
                                                           );

                        Bookeds = new ObservableCollection<Booked>(
                                                                context.DATCHOes.Where(result =>
                                                                result.MaChuyenBay.ToString() == MaChuyenBay).Select(result => new Booked
                                                                {
                                                                    MaDatCho = result.MaDatCho.ToString(),
                                                                    TenKhachDat = context.KHACHHANGs.Where(x => x.MaKhachHang == result.MaNguoiDat).FirstOrDefault().HoTen,
                                                                    SDT = context.KHACHHANGs.Where(x => x.MaKhachHang == result.MaNguoiDat).FirstOrDefault().SDT,
                                                                    Email = context.KHACHHANGs.Where(x => x.MaKhachHang == result.MaNguoiDat).FirstOrDefault().Email,
                                                                    CMND = context.KHACHHANGs.Where(x => x.MaKhachHang == result.MaNguoiDat).FirstOrDefault().SDT,
                                                                    MaKhachDat = result.MaNguoiDat.ToString(),
                                                                    SoCho = result.SoVeDat.ToString(),
                                                                    TeHangVe = context.HANGVEs.Where(x => x.MaHangVe == result.MaHangVe).FirstOrDefault().TenHangVe,
                                                                    NgayDatVe = result.NgayGioDat,
                                                                    GiaTien_Ve = result.GiaTien.ToString()
                                                                }).ToList()
                                                           );
                    }
                    catch
                    {

                    }
                }
            });
        }
    }
}
