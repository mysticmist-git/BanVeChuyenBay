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
using FlightTicketSell.Models.SearchRelated;
using System.Data.Entity;

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

        public DetailFlilghtInfo FlightInfo { get; set; }

        public ObservableCollection<OverlayAirport_Search> MidAirports { get; set; }

        public ObservableCollection<Customer> Customers { get; set; }

        public string MaDuongBay { get; set; }

        /// <summary>
        /// The reservatoin ID
        /// </summary>
        public int MaDatCho { get; set; }

        /// <summary>
        /// Display reservation ID
        /// </summary>
        

        /// <summary>
        /// The name of the customer who book the reservation
        /// </summary>
        public string TenKhachDat { get; set; }

        /// <summary>
        /// The number of places reserved
        /// </summary>
        public int SoCho { get; set; }

        /// <summary>
        /// The name of the ticket tier reserved
        /// </summary>
        public string TenHangVe { get; set; }

        /// <summary>
        /// The price of each reservation
        /// </summary>
        public string GiaTien_Ve { get; set; }

        public string GiaTong { get; set; }
        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ChangeTicketViewModel()
        {
            // Create commands

            ReturnCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.TicketSoldOrBooked);

            //PrintTicketCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.);

            //CancelTicketCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.);

            LoadCommand = new RelayCommand<object>((p) => true, async (p) =>
            {
                using (var context = new FlightTicketSellEntities())
                {
                    try
                    {
                        MaDuongBay = context.CHUYENBAYs.Where(result => result.MaChuyenBay== FlightInfo.MaChuyenBay).FirstOrDefault().MaDuongBay.ToString();

                        MidAirports = new ObservableCollection<OverlayAirport_Search>(
                                                                context.SANBAYTGs.Where(result =>
                                                                result.MaDuongBay.ToString() == MaDuongBay)
                                                                .Select(result => new OverlayAirport_Search
                                                                {
                                                                    ThuTu = result.ThuTu.ToString(),
                                                                    TenSanBay = context.SANBAYs.Where(x => x.MaSanBay == result.MaSanBay).FirstOrDefault().TenSanBay,
                                                                    GhiChu = result.GhiChu.ToString(),
                                                                    ThoiGianDung = result.ThoiGianDung.ToString()
                                                                }).ToList());                      

                        /*var results = await context.DATCHOes
                                .Where(dc => dc.MaChuyenBay == FlightInfo.MaChuyenBay && dc.MaDatCho == 1)
                                .Select(dc => dc.CHITIETDATCHOes
                                    .Select(ctdc => ctdc.KHACHHANG)
                                    )
                                .FirstOrDefaultAsync();
                        for (int i = 1; i <= results.Count(); i++)
                            Customers.Add(new Customer(results.ElementAt(i - 1)) { Index = i });*/

                        // Convert KHACHHANG to Customer and add it to Customers list


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
