using FlightTicketSell.Models;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using System.Data.Entity.Core;
using FlightTicketSell.Models.SearchRelated;
using FlightTicketSell.Views.SearchViewMore;

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
        public ObservableCollection<TicketTier_Search> TicketTier { get; set; }

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
                IoC.IoC.Get<ApplicationViewModel>().CurrentView = AppView.TicketInfoFilling;
                
               IoC.IoC.Get<TicketInfoViewModel>().FlightInfo = new DetailFlilghtInfo(FlightInfo);

            });

            TickedSoldBookedCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = AppView.TicketSoldOrBooked);
            
            LoadCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                using (var context = new FlightTicketSellEntities())
                {
                    try
                    {
                        FlightInfo.ThoiGianBay = context.CHUYENBAYs.Where(result => result.MaChuyenBay == FlightInfo.MaChuyenBay).FirstOrDefault().DUONGBAY.ThoiGianBay;

                        FlightInfo.MaDuongBay = context.CHUYENBAYs.Where(result => result.MaChuyenBay == FlightInfo.MaChuyenBay).FirstOrDefault().MaDuongBay;

                        //context.CHITIETHANGVEs.Where(result => result.MaChuyenBay.ToString() == MaChuyenBay).FirstOrDefault().MaHangVe.ToString();

                        OverlayAirport = new ObservableCollection<OverlayAirport_Search>(
                                                                context.SANBAYTGs.Where(result =>
                                                                result.MaDuongBay == FlightInfo.MaDuongBay)
                                                                .Select(result => new OverlayAirport_Search
                                                                {
                                                                    ThuTu = result.ThuTu.ToString(),
                                                                    TenSanBay = context.SANBAYs.Where(x => x.MaSanBay == result.MaSanBay).FirstOrDefault().TenSanBay,
                                                                    GhiChu = result.GhiChu.ToString(),
                                                                    ThoiGianDung = result.ThoiGianDung.ToString()
                                                                }).ToList()
                                                           );
                        TicketTier = new ObservableCollection<TicketTier_Search>(
                                                                context.CHUYENBAYs.Where(result =>
                                                                result.MaChuyenBay== FlightInfo.MaChuyenBay).FirstOrDefault()
                                                                .CHITIETHANGVEs.Select(cthv => new TicketTier_Search
                                                                {
                                                                    TenHangVe = cthv.HANGVE.TenHangVe,
                                                                    GheTrong = (cthv.SoGhe -
                                                                    context.VEs.Where(ve => ve.MaChuyenBay== FlightInfo.MaChuyenBay && ve.HANGVE.MaHangVe == cthv.HANGVE.MaHangVe).Count() -
                                                                    (context.DATCHOes.Where(dc =>
                                                                    dc.MaChuyenBay== FlightInfo.MaChuyenBay &&
                                                                    dc.HANGVE.MaHangVe == cthv.HANGVE.MaHangVe).Sum(dcc => (int?)dcc.SoVeDat) ?? 0)).ToString(),
                                                                    GiaTien = ((cthv.HANGVE.HeSo) * FlightInfo.GiaVe).ToString()
                                                                }).ToList()
                            );


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
