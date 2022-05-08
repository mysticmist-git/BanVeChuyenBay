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

        /// <summary>
        /// The command to save ticket, customer
        /// </summary>
        public ICommand PayCommand { get; set; }

        /// <summary>
        /// The command execute on view load
        /// </summary>
        public ICommand LoadCommand { get; set; }

        #endregion

        #region Public Properties

        /// <summary>
        /// Stores flight information
        /// </summary>
        public DetailFlilghtInfo FlightInfo { get; set; }

        /// <summary>
        /// Stores overlay airports of this flight
        /// </summary>
        public ObservableCollection<OverlayAirport_Search> OverlayAirports { get; set; }

        /// <summary>
        /// Customer name
        /// </summary>
        public string HoTen { get; set; }

        /// <summary>
        /// The customer ID
        /// </summary>
        public string CMND { get; set; }

        /// <summary>
        /// The phone number of the customer
        /// </summary>
        public string SDT { get; set; }

        /// <summary>
        /// The email of the customer
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The ticket tier this customer buy
        /// </summary>
        public string HangVe { get; set; }

        /// <summary>
        /// The flight route this flight take
        /// </summary>
        public string MaDuongBay { get; set; }

        /// <summary>
        /// The price of the ticket this customer buy
        /// </summary>
        public string GiaTien { get; set; } 

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public SellPayViewModel()
        {
            // Create commands
            ReturnCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.TicketInfoFilling);

            PayCommand = new RelayCommand<object>((p) => true, (p) => SaveTicketInformation());

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

        /// <summary>
        /// Saves customer information, ticket information down database
        /// </summary>
        private void SaveTicketInformation()
        {
            // TODO: Cài đặt "Thanh toán vé"
            MessageBox.Show("Thanh toán vé chưa được cài đặt", "Chưa cài đặt");
        }

        #endregion
    }
}
