using FlightTicketSell.Models;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using System.Data.Entity.Core;
using FlightTicketSell.Models.SearchRelated;
using System.Data.Entity;
using FlightTicketSell.Helpers;

namespace FlightTicketSell.ViewModels
{
    /// <summary>
    /// The view model for the flight detail view
    /// </summary>
    public class FlightDetailViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// Store flight infos
        /// </summary>
        public DetailFlilghtInfo FlightInfo { get; set; }

        /// <summary>
        /// Overlay airport infos
        /// </summary>
        public ObservableCollection<OverlayAirport_Search> OverlayAirport { get; set; }

        /// <summary>
        /// Ticket Tier infos
        /// </summary>
        public ObservableCollection<TicketTier> TicketTier { get; set; }

        public bool IsJustEdited { get; set; } = false;

        public bool IsAbleToModify 
        {
            get
            {
                if (FlightInfo is null || IoC.IoC.Get<ApplicationViewModel>().CurrentUserGroup is null)
                    return false;

                return 
                    (FlightInfo.TrangThai == 1)  &&
                    IoC.IoC.Get<ApplicationViewModel>().CurrentUserGroup.CanEditFlight;
            }
                
        }

        public bool IsCancelable { get => FlightInfo is null ? false : FlightInfo.TrangThai == 1; }

        public bool IsAbleToSellAndBook
        {
            get
            {
                if (FlightInfo is null || IoC.IoC.Get<ApplicationViewModel>().CurrentUserGroup is null)
                    return false;

                return
                    FlightInfo.IsAbleToSellAndBook &&
                    IoC.IoC.Get<ApplicationViewModel>().CurrentUserGroup.CanEditFlight;
            }
                
        }

        #endregion

        #region Commands

        /// <summary>
        /// What to do when first load
        /// </summary>
        public ICommand LoadCommand { get; set; }

        /// <summary>
        /// Return to previous view
        /// </summary>
        public ICommand ReturnCommand { get; set; }

        /// <summary>
        /// Navigate to <see cref="AppView.BookDetail"/> View
        /// </summary>
        public ICommand PlaceBookCommand { get; set; }

        /// <summary>
        /// Navigate to <see cref="AppView.TicketInfoFilling"/> View
        /// </summary>
        public ICommand TicketBuyCommand { get; set; }

        /// <summary>
        /// Navigate to <see cref="AppView.TicketSoldOrBooked"/> View
        /// </summary>
        public ICommand TickedSoldBookedCommand { get; set; }

        /// <summary>
        /// Edit the flight's info
        /// </summary>
        public ICommand EditCommand { get; set; }

        /// <summary>
        /// Cancel flight command
        /// </summary>
        public ICommand CancelFlightCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FlightDetailViewModel()
        {
            // Create commands
            ReturnCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.Search);

            PlaceBookCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = AppView.BookDetail);

            TicketBuyCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                IoC.IoC.Get<TicketInfoFillingViewModel>().ClearData();
                IoC.IoC.Get<ApplicationViewModel>().CurrentView = AppView.TicketInfoFilling;
            });

            TickedSoldBookedCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                IoC.IoC.Get<ApplicationViewModel>().CurrentView = AppView.TicketSoldOrBooked;
            });

            LoadCommand = new RelayCommand<object>((p) => true, async (p) =>
            {
                using (var context = new FlightTicketSellEntities())
                {
                    try
                    {
                        FlightInfo.ThoiGianBay = context.CHUYENBAYs.Where(result => result.MaChuyenBay == FlightInfo.MaChuyenBay).FirstOrDefault().DUONGBAY.ThoiGianBay;

                        FlightInfo.MaDuongBay = context.CHUYENBAYs.Where(result => result.MaChuyenBay == FlightInfo.MaChuyenBay).FirstOrDefault().MaDuongBay;

                        if (IsJustEdited)
                        {
                            
                            
                            //var flight = await context.CHUYENBAYs.Where(cb => cb.MaChuyenBay == FlightInfo.MaChuyenBay).FirstOrDefaultAsync();

                            //FlightInfo.GiaVe = flight.GiaVe;
                            //FlightInfo.MaSanBayDi = flight.DUONGBAY.MaSanBayDi;
                            //FlightInfo.MaSanBayDen = flight.DUONGBAY.MaSanBayDen;
                            //FlightInfo.SanBayDen = flight.DUONGBAY.SANBAY.TenSanBay;
                            //FlightInfo.SanBayDi = flight.DUONGBAY.SANBAY1.TenSanBay;
                            //FlightInfo.SanBayDenVietTat = flight.DUONGBAY.SANBAY.VietTat;
                            //FlightInfo.SanBayDiVietTat = flight.DUONGBAY.SANBAY1.VietTat;

                            ////FlightInfo.GheTrong = TicketTier.Sum(tt => tt.GheTrong);
                            //FlightInfo.GheTrong =
                            //    flight.CHITIETHANGVEs.Select(cthv => cthv.SoGhe).DefaultIfEmpty(0).Sum() -
                            //    flight.DATCHOes.Select(dc => dc.SoVeDat).DefaultIfEmpty(0).Sum() -
                            //    flight.VEs.Count;

                            //FlightInfo.NgayGio = await context.CHUYENBAYs.Where(cb => cb.MaChuyenBay == FlightInfo.MaChuyenBay).Select(cb => cb.NgayGio).FirstOrDefaultAsync();

                            //IsJustEdited = false;

                            //OnPropertyChanged(nameof(FlightInfo));
                            //OnPropertyChanged(nameof(TicketTier));
                            //OnPropertyChanged(nameof(OverlayAirport));
                        }

                        OverlayAirport = new ObservableCollection<OverlayAirport_Search>(
                                                                context.SANBAYTGs.Where(result =>
                                                                result.MaDuongBay == FlightInfo.MaDuongBay)
                                                                .Select(result => new OverlayAirport_Search
                                                                {
                                                                    ThuTu = result.ThuTu.ToString(),
                                                                    TenSanBay = context.SANBAYs.Where(x => x.MaSanBay == result.MaSanBay).FirstOrDefault().TenSanBay,
                                                                    GhiChu = result.GhiChu.ToString(),
                                                                    ThoiGianDung = result.ThoiGianDung.ToString(),
                                                                    MaSanBay = result.MaSanBay,
                                                                    MaDuongBay = result.MaDuongBay
                                                                }).ToList()
                                                           );
                        TicketTier = new ObservableCollection<TicketTier>(
                                                                context.CHUYENBAYs.Where(result =>
                                                                result.MaChuyenBay== FlightInfo.MaChuyenBay).FirstOrDefault()
                                                                .CHITIETHANGVEs.Select(cthv => new TicketTier
                                                                {
                                                                    TenHangVe = cthv.HANGVE.TenHangVe,
                                                                    GheTrong = (cthv.SoGhe -
                                                                    context.VEs
                                                                        .Where(ve => ve.MaChuyenBay== FlightInfo.MaChuyenBay && ve.HANGVE.MaHangVe == cthv.HANGVE.MaHangVe).Count() -
                                                                            (context.DATCHOes
                                                                                .Where(dc => dc.MaChuyenBay== FlightInfo.MaChuyenBay &&
                                                                                             dc.HANGVE.MaHangVe == cthv.HANGVE.MaHangVe)
                                                                                .Sum(dcc => (int?)dcc.SoVeDat) ?? 0)),
                                                                    GiaTien = ((cthv.HANGVE.HeSo) * FlightInfo.GiaVe),
                                                                    HeSo = cthv.HANGVE.HeSo,
                                                                    MaHangVe = cthv.HANGVE.MaHangVe,
                                                                    TrangThai = cthv.HANGVE.TrangThai,
                                                                }).ToList()
                            );


                    }
                    catch (EntityException e)
                    {
                        NotifyHelper.ShowEntityException(e);
                    }
                }
            });

            EditCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = AppView.FlightInfoEdit);

            CancelFlightCommand = new RelayCommand<object>(p => true, async p =>
            {
                var result = MessageBox.Show("Bạn có chắc muốn hủy chuyến bay này?", "Hủy chuyến bay", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.No)
                    return;

                using (var context = new FlightTicketSellEntities())
                {
                    try
                    {
                        var flight = await context.CHUYENBAYs.Where(cb => cb.MaChuyenBay == FlightInfo.MaChuyenBay).FirstOrDefaultAsync();

                        // Cập nhật trạng thái
                        flight.TrangThai = 3;

                        foreach (var book in flight.DATCHOes)
                            book.TrangThai = BookHelper.BookingStateToString(Models.Enums.BookingState.Cancel);

                        await context.SaveChangesAsync();

                        FlightInfo.TrangThai = 3;
                    }
                    catch (EntityException ex)
                    {
                        NotifyHelper.ShowEntityException(ex);
                    }
                }

                OnPropertyChanged(nameof(IsAbleToModify));
                OnPropertyChanged(nameof(IsAbleToSellAndBook));
                OnPropertyChanged(nameof(IsCancelable));

            });
        }

        #endregion
    }
}
