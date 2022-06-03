using FlightTicketSell.Views;
using FlightTicketSell.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;
using System.Data.Entity.Core;
using System.Windows;
using FlightTicketSell.ViewModels.Search;
using FlightTicketSell.Models.SearchRelated;
using System;

namespace FlightTicketSell.ViewModels
{
    public class FlightTicketAndBookViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// The place reservation id for searching
        /// </summary>
        public int PlaceReservationID { get; set; }

        /// <summary>
        /// The ticket id for searching
        /// </summary>
        public int TicketID { get; set; }

        /// <summary>
        /// The tickets sold
        /// </summary>
        public ObservableCollection<Ticket> Tickets { get; set; }

        /// <summary>
        /// The places reserved
        /// </summary>
        public ObservableCollection<BookSearchVariant> Books { get; set; }

        /// <summary>
        /// Contains information for 
        /// </summary>
        public DetailFlilghtInfo FlightInfo { get => IoC.IoC.Get<FlightDetailViewModel>().FlightInfo; }

        /// <summary>
        /// The overlay airport
        /// </summary>
        public ObservableCollection<OverlayAirport_Search> OverlayAirport { get => IoC.IoC.Get<FlightDetailViewModel>().OverlayAirport; }

        public string _search_id;
        public string Search_IDBooked
        {
            get
            {
                return _search_id;
            }
            set
            {
                _search_id = value;
            }
        }

        public string _search_id_sold;
        public string Search_IDSold
        {
            get
            {
                return _search_id_sold;
            }
            set
            {
                _search_id_sold = value;
            }
        }
        #endregion



        #region Commands

        public ICommand ReturnCommand { get; set; }
        public ICommand LoadCommand { get; set; }
        public ICommand LoadCommand1 { get; set; }
        public ICommand TicketSearchCommand { get; set; }
        public ICommand PlaceReservationSearchCommand { get; set; }
        public ICommand ShowMoreCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FlightTicketAndBookViewModel()
        {
            ReturnCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.FlightDetail);

            LoadCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var MaChuyenBay = IoC.IoC.Get<FlightDetailViewModel>().FlightInfo.MaChuyenBay;
                Search_IDSold = string.Empty;
                Search_IDBooked = string.Empty;
                using (var context = new FlightTicketSellEntities())
                {
                    try
                    {
                        Tickets = new ObservableCollection<Ticket>(
                                                                context.VEs
                                                                .Where(result => result.MaChuyenBay == MaChuyenBay)
                                                                .Select(result => new Ticket
                                                                {
                                                                    MaVe = result.MaVe,
                                                                    TenKhachHang = context.KHACHHANGs.Where(x => x.MaKhachHang == result.MaKhachHang).FirstOrDefault().HoTen,
                                                                    TenHangVe = context.HANGVEs.Where(x => x.MaHangVe == result.MaHangVe).FirstOrDefault().TenHangVe,
                                                                    NgayThanhToan = result.NgayThanhToan,
                                                                    GiaTien = result.GiaTien
                                                                }).ToList()
                                                           );

                        Books = new ObservableCollection<BookSearchVariant>(
                                                                context.DATCHOes
                                                                .Where(result => result.MaChuyenBay == MaChuyenBay)
                                                                .Select(result => new BookSearchVariant
                                                                {
                                                                    MaDatCho = result.MaDatCho,
                                                                    ThongTinNguoiDat = new Customer
                                                                    {
                                                                        HoTen = context.KHACHHANGs.Where(x => x.MaKhachHang == result.MaNguoiDat).FirstOrDefault().HoTen
                                                                    },
                                                                    SoVeDat = result.SoVeDat,
                                                                    TenHangVe = context.HANGVEs.Where(x => x.MaHangVe == result.MaHangVe).FirstOrDefault().TenHangVe,
                                                                    NgayGioDat = result.NgayGioDat,
                                                                    GiaTien_Ve = result.HANGVE.HeSo * result.CHUYENBAY.GiaVe,
                                                                    TrangThai = result.TrangThai
                                                                }).ToList()
                                                           );
                    }
                    catch
                    {

                    }
                }
            });

            LoadCommand1 = new RelayCommand<object>((p) => true, (p) =>
            {
                var MaChuyenBay = IoC.IoC.Get<FlightDetailViewModel>().FlightInfo.MaChuyenBay;
                using (var context = new FlightTicketSellEntities())
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(Search_IDSold))
                        {
                            var list = new ObservableCollection<Ticket>(
                                                                context.VEs
                                                                .Where(result => result.MaChuyenBay == MaChuyenBay)
                                                                .Select(result => new Ticket
                                                                {
                                                                    MaVe = result.MaVe,
                                                                    TenKhachHang = context.KHACHHANGs.Where(x => x.MaKhachHang == result.MaKhachHang).FirstOrDefault().HoTen,
                                                                    TenHangVe = context.HANGVEs.Where(x => x.MaHangVe == result.MaHangVe).FirstOrDefault().TenHangVe,
                                                                    NgayThanhToan = result.NgayThanhToan,
                                                                    GiaTien = result.GiaTien
                                                                }).ToList());
                            list = new ObservableCollection<Ticket>(list.Where(h => h.DisplayTicketID.ToLower().Contains(Search_IDSold.ToLower())).ToList());
                            if (list.Count > 0)
                            {
                                Tickets = new ObservableCollection<Ticket>(list);
                            }
                            else
                            {
                                Tickets = null;
                            }
                            
                        }
                        else
                        {
                            Tickets = new ObservableCollection<Ticket>(
                                                                context.VEs
                                                                .Where(result => result.MaChuyenBay == MaChuyenBay)
                                                                .Select(result => new Ticket
                                                                {
                                                                    MaVe = result.MaVe,
                                                                    TenKhachHang = context.KHACHHANGs.Where(x => x.MaKhachHang == result.MaKhachHang).FirstOrDefault().HoTen,
                                                                    TenHangVe = context.HANGVEs.Where(x => x.MaHangVe == result.MaHangVe).FirstOrDefault().TenHangVe,
                                                                    NgayThanhToan = result.NgayThanhToan,
                                                                    GiaTien = result.GiaTien
                                                                }).ToList()
                                                           );
                        }

                        if (Search_IDBooked != null)
                        {
                            var list = new ObservableCollection<BookSearchVariant>(
                                                                context.DATCHOes
                                                                .Where(result => result.MaChuyenBay == MaChuyenBay)
                                                                .Select(result => new BookSearchVariant
                                                                {
                                                                    MaDatCho = result.MaDatCho,
                                                                    ThongTinNguoiDat = new Customer
                                                                    {
                                                                        HoTen = context.KHACHHANGs.Where(x => x.MaKhachHang == result.MaNguoiDat).FirstOrDefault().HoTen
                                                                    },
                                                                    SoVeDat = result.SoVeDat,
                                                                    TenHangVe = context.HANGVEs.Where(x => x.MaHangVe == result.MaHangVe).FirstOrDefault().TenHangVe,
                                                                    NgayGioDat = result.NgayGioDat,
                                                                    GiaTien_Ve = result.HANGVE.HeSo * result.CHUYENBAY.GiaVe,
                                                                    TrangThai = result.TrangThai,
                                                                }).ToList());
                            list = new ObservableCollection<BookSearchVariant>(list.Where(h => h.DisplayReservationID.ToLower().Contains(Search_IDBooked.ToLower())).ToList());
                            if (list.Count > 0)
                            {
                                Books = new ObservableCollection<BookSearchVariant>(list);
                            }
                            else
                            {
                                Books = null;
                            }
                        }
                        else
                        {
                            Books = new ObservableCollection<BookSearchVariant>(
                                                                context.DATCHOes
                                                                .Where(result => result.MaChuyenBay == MaChuyenBay)
                                                                .Select(result => new BookSearchVariant
                                                                {
                                                                    MaDatCho = result.MaDatCho,
                                                                    ThongTinNguoiDat = new Customer
                                                                    {
                                                                        HoTen = context.KHACHHANGs.Where(x => x.MaKhachHang == result.MaNguoiDat).FirstOrDefault().HoTen
                                                                    },
                                                                    SoVeDat = result.SoVeDat,
                                                                    TenHangVe = context.HANGVEs.Where(x => x.MaHangVe == result.MaHangVe).FirstOrDefault().TenHangVe,
                                                                    NgayGioDat = result.NgayGioDat,
                                                                    GiaTien_Ve = result.HANGVE.HeSo * result.CHUYENBAY.GiaVe,
                                                                    TrangThai = result.TrangThai,
                                                                }).ToList()
                                                           );
                        }
                    }
                    catch
                    {

                    }
                }
            });
        }

        #endregion
    }
}
