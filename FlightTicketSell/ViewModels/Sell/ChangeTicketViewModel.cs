using FlightTicketSell.Views;
using FlightTicketSell.Models;
using FlightTicketSell.ViewModels.Sell;
using FlightTicketSell.ViewModels.Search;
using FlightTicketSell.Views.SearchViewMore;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;
using System.Data.Entity.Core;
using System.Windows;
using System;

namespace FlightTicketSell.ViewModels
{
    public class ChangeTicketViewModel : BaseViewModel
    {
        #region Commands

        /// <summary>
        /// The command to continue
        /// </summary>
        public ICommand PrintTicketCommand { get; set; }

        public ICommand CancelTicketCommand { get; set; }

        public ICommand ReturnCommand { get; set; }

        public ICommand LoadCommand { get; set; }
        public DateTime TicketDeadline { get; set; }

        public ObservableCollection<MidAirport_Search> MidAirports { get; set; }

        public ObservableCollection<TicketSold> Customers { get; set; }

        public string MaDuongBay { get; set; }
        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ChangeTicketViewModel()
        {
            // Create commands

            ReturnCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.Sell);

            //PrintTicketCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.);

            //CancelTicketCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.);

            LoadCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                using (var context = new FlightTicketSellEntities())
                {
                    try
                    {
                        MaDuongBay = context.CHUYENBAYs.Where(result => result.MaChuyenBay== 1).FirstOrDefault().MaDuongBay.ToString();

                        MidAirports = new ObservableCollection<MidAirport_Search>(
                                                                context.SANBAYTGs.Where(result =>
                                                                result.MaDuongBay.ToString() == MaDuongBay)
                                                                .Select(result => new MidAirport_Search
                                                                {
                                                                    ThuTu = result.ThuTu.ToString(),
                                                                    TenSanBay = context.SANBAYs.Where(x => x.MaSanBay == result.MaSanBay).FirstOrDefault().TenSanBay,
                                                                    GhiChu = result.GhiChu.ToString(),
                                                                    ThoiGianDung = result.ThoiGianDung.ToString()
                                                                }).ToList());

                        Customers = new ObservableCollection<TicketSold>(
                                                                context.VEs.Where(result =>
                                                                result.MaChuyenBay == 1)
                                                                .Select(result => new TicketSold
                                                                {                                                                  
                                                                    TenKhachHang = context.KHACHHANGs.Where(x => x.MaKhachHang == result.MaKhachHang).FirstOrDefault().HoTen,
                                                                    MaKhachHang = result.MaKhachHang.ToString(),                                                                
                                                                }).ToList()
                                                           );

                    }
                    catch (EntityException)
                    {
                        // TODO: messagebox vo
                        return;
                    }
                }
            });

        }


        #endregion

    }
}
