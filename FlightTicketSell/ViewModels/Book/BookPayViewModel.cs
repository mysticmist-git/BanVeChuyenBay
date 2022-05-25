using System;
using System.Data.Entity.Core;
using System.Windows.Input;
using FlightTicketSell.Models;
using System.Linq;
using System.Data.Entity;
using System.Collections.ObjectModel;
using FlightTicketSell.Models.SearchRelated;
using System.Windows;

namespace FlightTicketSell.ViewModels
{
    public class BookPayViewModel : BaseViewModel
    {
        private int _cancelDays;
        #region Commands

        /// <summary>
        /// The command to return to previous view
        /// </summary>
        public ICommand ReturnCommand { get; set; }

        public ICommand LoadCommand { get; set; }

        public ICommand BookPay { get; set; } 


        #endregion

        #region Constructor
        /// <summary>
        /// FlightDetail
        /// </summary>
        public FlightInfo FlightInfo { get => IoC.IoC.Get<FlightDetailViewModel>().FlightInfo; }

        /// <summary>
        /// The customer list we get from book detail view
        /// </summary>
        public ObservableCollection<CustomerBookVariant> Customers { get => IoC.IoC.Get<BookDetailViewModel>().Customers; }

        /// <summary>
        /// The current ticket tier
        /// </summary>
        public TicketTier CurrentTicketTier { get => IoC.IoC.Get<BookDetailViewModel>().CurrentTicketTier; }

        /// <summary>
        /// The overlay airport
        /// </summary>
        public ObservableCollection<OverlayAirport_Search> OverlayAirport { get => IoC.IoC.Get<FlightDetailViewModel>().OverlayAirport; }

        /// <summary>
        /// The deadline for canceling the booking
        /// </summary>
        public DateTime HanChotHuyVe { get => FlightInfo.NgayGio.AddDays(_cancelDays); }

        /// <summary>
        /// The display deadline for canceling the booking
        /// </summary>
        public string DisplayCancelDeadline { get => HanChotHuyVe.ToString("HH:mm dd/MM/yyyy", new System.Globalization.CultureInfo("vi-VN")); }

        public Book BookInfo { get => IoC.IoC.Get<BookDetailViewModel>().BookInfo; }

        public BookPayViewModel()
        {
        
        // Create commands
            ReturnCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.BookDetail);

            LoadCommand = new RelayCommand<object>((p) => true, async (p) =>
            {
                using (var context = new FlightTicketSellEntities())
                {
                    try
                    {
                        // Get deadline to cancel the booking
                        var thoiGianHuyDatVe = await context.THAMSOes.Where(ts => ts.TenThamSo == "ThoiGianHuyDatVe").FirstOrDefaultAsync();
                        _cancelDays = thoiGianHuyDatVe.GiaTri;
                    }
                    catch (EntityException e)
                    {
                        MessageBox.Show("Database access failed!", string.Format($"Exception: {e.Message}"), MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            });

            BookPay = new RelayCommand<object>((p) => true, async (p) =>
            {
                using (var context = new FlightTicketSellEntities())
                {
                    try
                    {
                        var khachDatCho = new KHACHHANG()
                        {
                            HoTen = BookInfo.ThongTinNguoiDat.HoTen,
                            SDT = BookInfo.ThongTinNguoiDat.SDT,
                            CMND = BookInfo.ThongTinNguoiDat.CMND,
                            Email = BookInfo.ThongTinNguoiDat.Email
                        };

                        context.KHACHHANGs.Add(khachDatCho);
                        await context.SaveChangesAsync();
                        var maNguoiDat = await context.KHACHHANGs.Where(kh => kh.CMND == BookInfo.ThongTinNguoiDat.CMND).Select(kh => kh.MaKhachHang).FirstOrDefaultAsync();

                        var ngayGioDat = DateTime.Now;

                        var datCho = new DATCHO()
                        {
                            MaHangVe = CurrentTicketTier.MaHangVe,
                            MaNguoiDat = maNguoiDat,
                            MaChuyenBay = FlightInfo.MaChuyenBay,
                            GiaTien = BookInfo.GiaTien,
                            NgayGioDat = ngayGioDat,
                            TrangThai = "ChuaDoi",
                            SoVeDat = Customers.Count
                        };

                        context.DATCHOes.Add(datCho);
                        await context.SaveChangesAsync();

                        var maDatCho = await context.DATCHOes
                            .Where(dc => 
                                dc.NgayGioDat.Year == ngayGioDat.Year &&
                                dc.NgayGioDat.Month == ngayGioDat.Month &&
                                dc.NgayGioDat.Day == ngayGioDat.Day &&
                                dc.NgayGioDat.Hour == ngayGioDat.Hour &&
                                dc.NgayGioDat.Minute == ngayGioDat.Minute &&
                                dc.MaChuyenBay == FlightInfo.MaChuyenBay && 
                                dc.MaNguoiDat == maNguoiDat)
                            .Select(dc => dc.MaDatCho).FirstOrDefaultAsync();

                        foreach (var khachHang in Customers)
                        {
                            var khach = new KHACHHANG()
                            {
                                HoTen = khachHang.HoTen,
                                SDT = khachHang.SDT,
                                CMND = khachHang.CMND,
                                Email = khachHang.Email
                            };

                            context.KHACHHANGs.Add(khach);
                            await context.SaveChangesAsync();
                            var MaKhachHangThuHuong = await context.KHACHHANGs.Where(kh => kh.CMND == khachHang.CMND).Select(kh => kh.MaKhachHang).FirstOrDefaultAsync();

                            var chitietDatCho = new CHITIETDATCHO()
                            {
                                MaDatCho = maDatCho ,
                                MaKhachHang = MaKhachHangThuHuong

                            };
                            context.CHITIETDATCHOes.Add(chitietDatCho);
                            await context.SaveChangesAsync();
                        }
                    }
                    catch (EntityException e)
                    {
                        MessageBox.Show("Database access failed!", string.Format($"Exception: {e.Message}"), MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            });
        }
        #endregion
    }
}
