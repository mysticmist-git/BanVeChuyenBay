using FlightTicketSell.Views;
using FlightTicketSell.Models;
using FlightTicketSell.Views.SearchViewMore;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;
using System.Data.Entity.Core;
using System.Windows;
using FlightTicketSell.ViewModels.Search;

namespace FlightTicketSell.ViewModels
{
    public class FlightTicketAndReservationViewModel : BaseViewModel
    {
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
        public ObservableCollection<PlaceReservation> PlaceReservations { get; set; }

        #region Commands

        public ICommand ReturnCommand { get; set; }
        public ICommand LoadCommand { get; set; }
        public ICommand TicketSearchCommand { get; set; } 
        public ICommand PlaceReservationSearchCommand { get; set; } 

        #endregion

        public FlightTicketAndReservationViewModel()
        {
            ReturnCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.FlightTicket);

            LoadCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                var MaChuyenBay = IoC.IoC.Get<FlightDetailViewModel>().FlightInfo.MaChuyenBay;

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

                        PlaceReservations = new ObservableCollection<PlaceReservation>(
                                                                context.DATCHOes
                                                                .Where(result => result.MaChuyenBay == MaChuyenBay)
                                                                .Select(result => new PlaceReservation
                                                                {
                                                                    MaDatCho = result.MaDatCho,
                                                                    TenKhachDat = context.KHACHHANGs.Where(x => x.MaKhachHang == result.MaNguoiDat).FirstOrDefault().HoTen,
                                                                    SoCho = result.SoVeDat,
                                                                    TenHangVe = context.HANGVEs.Where(x => x.MaHangVe == result.MaHangVe).FirstOrDefault().TenHangVe,
                                                                    NgayDatVe = result.NgayGioDat,
                                                                    GiaTien_Ve = result.GiaTien
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
