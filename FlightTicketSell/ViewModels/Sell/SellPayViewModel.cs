using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using FlightTicketSell.Models;
using System.Data.Entity.Core;
using FlightTicketSell.Models.SearchRelated;
using FlightTicketSell.Views.SearchViewMore;
using FlightTicketSell.Views;
using FlightTicketSell.ViewModels.Sell;
using FlightTicketSell.ViewModels.Search;
using System.Collections.ObjectModel;

namespace FlightTicketSell.ViewModels
{
    public class SellPayViewModel : BaseViewModel 
    {
        #region Commands

        /// <summary>
        /// The command to continue
        /// </summary>
        public ICommand ReturnCommand { get; set; }

        public ICommand PayCommand { get; set; }

        public ICommand LoadCommand { get; set; }


        #endregion
        public DetailFlilghtInfo FlightInfo { get; set; }
        public ObservableCollection<OverlayAirport_Search> OverlayAirports { get; set; }
        public string HoTen { get; set; }
        public string CMND { get; set; }
        public string SDT { get; set; }
        public string Email { get; set; }
        
        public string HangVe { get; set; }
        public string MaDuongBay { get; set; }

        public string GiaTien { get; set; }
        #region Constructor


        /// <summary>
        /// Default constructor
        /// </summary>
        public SellPayViewModel()
        {
            // Create commands
            ReturnCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.TicketInfoFilling);

            PayCommand = new RelayCommand<object>((p) => true, (p) => SaveTicketPay());

            LoadCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                using (var context = new FlightTicketSellEntities())
                {
                    try
                    {
                        MaDuongBay = context.CHUYENBAYs.Where(result => result.MaChuyenBay == FlightInfo.MaChuyenBay).FirstOrDefault().MaDuongBay.ToString();

                        OverlayAirports = new ObservableCollection<OverlayAirport_Search>(
                                                                context.SANBAYTGs.Where(result =>
                                                                result.MaDuongBay.ToString() == MaDuongBay)
                                                                .Select(result => new OverlayAirport_Search
                                                                {
                                                                    ThuTu = result.ThuTu.ToString(),
                                                                    TenSanBay = context.SANBAYs.Where(x => x.MaSanBay == result.MaSanBay).FirstOrDefault().TenSanBay,
                                                                    GhiChu = result.GhiChu.ToString(),
                                                                    ThoiGianDung = result.ThoiGianDung.ToString()
                                                                }).ToList());

                        

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

        #region Methods

        private void SaveTicketPay()
        {
            // TODO: Cài đặt "Thanh toán vé"
            MessageBox.Show("Thanh toán vé chưa được cài đặt", "Chưa cài đặt");
        }

        #endregion
    }
}
