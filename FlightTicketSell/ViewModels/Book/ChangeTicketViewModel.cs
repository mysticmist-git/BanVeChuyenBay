using FlightTicketSell.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;
using System.Data.Entity.Core;
using System.Windows;
using System;
using FlightTicketSell.Models.SearchRelated;
using System.Data.Entity;
using System.Globalization;
using FlightTicketSell.Models.Enums;
using FlightTicketSell.Helpers;
using FlightTicketSell.ViewModels.Search;
using System.Windows.Controls;
using FlightTicketSell.Views;
using System.Printing;
using System.Drawing.Printing;

namespace FlightTicketSell.ViewModels
{
    public class ChangeTicketViewModel : BaseViewModel
    {
        #region Print
        public HANGVE print_HANGVE { get; set; }
        public ObservableCollection<KHACHHANG> print_KHACHHANG { get; set; } = new ObservableCollection<KHACHHANG>();
        public CHUYENBAY print_CHUYENBAY { get; set; }
        public ObservableCollection<PrintTicket> list_printTickets { get; set; } = new ObservableCollection<PrintTicket>();
        public ICommand PrintLoadCommand { get; set; }
        #endregion

        #region Private Members

        /// <summary>
        /// The intertal of time the customer can cancel the booking
        /// </summary>
        private int _cancelDays;

        #endregion

        #region Commands

        /// <summary>
        /// The command to continue
        /// </summary>
        public ICommand ChangeForTicketCommand { get; set; }


        /// <summary>
        /// Print the booking's ticket
        /// </summary>
        public ICommand PrintCommand { get; set; }

        /// <summary>
        /// The command to cancel current booking
        /// </summary>
        public ICommand CancelTicketCommand { get; set; }

        /// <summary>
        /// Return to previous view: Sold ticket, bookings
        /// </summary>
        public ICommand ReturnCommand { get; set; }

        /// <summary>
        /// Command executed on view load
        /// </summary>
        public ICommand LoadCommand { get; set; }

        #endregion

        #region Public Properties

        /// <summary>
        /// Contains flight information
        /// </summary>
        public DetailFlilghtInfo FlightInfo { get => IoC.IoC.Get<FlightDetailViewModel>().FlightInfo; }

        /// <summary>
        /// The overlay airports of this flight
        /// </summary>
        public ObservableCollection<OverlayAirport_Search> OverlayAirports { get => IoC.IoC.Get<FlightDetailViewModel>().OverlayAirport; }

        /// <summary>
        /// Stores customers that this book contains
        /// </summary>
        public ObservableCollection<CustomerWithIndex> Customers { get; set; }

        /// <summary>
        /// Stores booking information
        /// </summary>
        public BookSearchVariant BookingInfo { get; set; }

        /// <summary>
        /// The deadline for canceling the booking
        /// </summary>
        public DateTime HanChotHuyVe { get => FlightInfo.NgayGio.AddDays(-_cancelDays); }

        /// <summary>
        /// The display deadline for canceling the booking
        /// </summary>
        public string DisplayCancelDeadline { get => HanChotHuyVe.ToString("HH:mm dd/MM/yyyy", new CultureInfo("vi-VN")); }

        #region Boolean stuffs

        /// <summary>
        /// Indicates if this booking is already changed for tickets
        /// </summary>
        public bool BookPrinted { get => BookingInfo.BookingState == BookingState.Changed; }

        /// <summary>
        /// Indicates if this booking is already canceled
        /// </summary>
        public bool IsBookCanceled { get => BookingInfo.BookingState == BookingState.Cancel; }

        /// <summary>
        /// Indicates if it's already too late to cancel this booking
        /// </summary>
        public bool IsBookCancelDeadlinePassed { get => DateTime.Now >= HanChotHuyVe; }

        /// <summary>
        /// Indicates if this booking can be interacted
        /// </summary>
        public bool IsInteractable { get => !IsBookCanceled && !BookPrinted && !IsBookCancelDeadlinePassed; }

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ChangeTicketViewModel()
        {
            PrintLoadCommand = new RelayCommand<object>((p) => true, (p) =>
            {

            });
            // Create commands

            ReturnCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.TicketSoldOrBooked);

            PrintCommand = new RelayCommand<object>((p) => true, async (p) =>
            {
                PrintTicket();

                // Stop if the booking has been already changed for tickets
                if (BookingInfo.BookingState == BookingState.Changed)
                    return;

                // Update booking's state
                using (var context = new FlightTicketSellEntities())
                {
                    try
                    {
                        // Update book state in database
                        var book = await context.DATCHOes.Where(dc => dc.MaDatCho == BookingInfo.MaDatCho).FirstOrDefaultAsync();
                        book.TrangThai = BookHelper.BookingStateToString(BookingState.Changed);
                        await context.SaveChangesAsync();

                        // Update book state in the app
                        BookingInfo.BookingState = BookingState.Changed;

                        OnPropertyChanged(nameof(IsInteractable));
                    }
                    catch (EntityException e)
                    {
                        NotifyHelper.ShowEntityException(e); ;
                    }
                }
            });

            //CancelTicketCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.);

            LoadCommand = new RelayCommand<object>((p) => true, async (p) =>
            {
                using (var context = new FlightTicketSellEntities())
                {
                    try
                    {
                        print_CHUYENBAY = context.CHUYENBAYs.Where(h => h.MaChuyenBay == FlightInfo.MaChuyenBay).FirstOrDefault();
                        print_HANGVE = context.HANGVEs.Where(h => h.TenHangVe == BookingInfo.TenHangVe).FirstOrDefault();
                        // Get deadline to cancel the booking
                        _cancelDays = context.THAMSOes.Where(ts => ts.TenThamSo == "ThoiGianHuyDatVe").FirstOrDefault().GiaTri;
                        OnPropertyChanged(nameof(DisplayCancelDeadline));

                        // Get KHACHAHNG
                        var result = await context.DATCHOes
                                .Where(dc => dc.MaChuyenBay == FlightInfo.MaChuyenBay && dc.MaDatCho == BookingInfo.MaDatCho)
                                .Select(dc => dc.CHITIETDATCHOes
                                    .Select(ctdc => ctdc.KHACHHANG)
                                    )
                                .FirstOrDefaultAsync();
                        print_KHACHHANG = new ObservableCollection<KHACHHANG>(result);

                        // Convert KHACHHANG to Customer and add it to Customers list
                        Customers = new ObservableCollection<CustomerWithIndex>();
                        for (int i = 1; i <= result.Count(); i++)
                            Customers.Add(new CustomerWithIndex()
                            {
                                Index = i,
                                HoTen = result.ElementAt(i - 1).HoTen,
                                CMND = result.ElementAt(i - 1).CMND,
                                SDT = result.ElementAt(i - 1).SDT,
                                Email = result.ElementAt(i - 1).Email,
                            });
                    }
                    catch (EntityException e)
                    {
                        NotifyHelper.ShowEntityException(e); ;
                    }
                }
            });

            CancelTicketCommand = new RelayCommand<object>(p => true, async p =>
            {
                using (var context = new FlightTicketSellEntities())
                {
                    try
                    {
                        // Update database                        
                        var datCho = await context.DATCHOes
                            .Where(dc => dc.MaDatCho == BookingInfo.MaDatCho)
                            .FirstOrDefaultAsync();

                        datCho.TrangThai = BookHelper.BookingStateToString(BookingState.Cancel);
                        await context.SaveChangesAsync();

                        // Update client
                        BookingInfo.BookingState = BookingState.Cancel;

                        OnPropertyChanged(nameof(IsInteractable));
                        OnPropertyChanged(nameof(IsBookCanceled));
                    }
                    catch (EntityException e)
                    {
                        NotifyHelper.ShowEntityException(e); ;
                    }
                }

            });

        }

        #endregion

        #region Helpers

        /// <summary>
        /// Print ticket
        /// </summary>
        private void PrintTicket()
        {
            list_printTickets.Clear();
            PrintMultipleTicket printMultipleTicket = new PrintMultipleTicket() { DataContext = this };
            
            foreach (var item in print_KHACHHANG)
            {
                PrintTicket temp = new PrintTicket(print_HANGVE, item, print_CHUYENBAY);
                list_printTickets.Add(temp);
            }
            printMultipleTicket.Show();
            
            PrintDialog printDialog = new PrintDialog();
            PrinterSettings settings = new PrinterSettings();
            printDialog.PrintQueue = new PrintQueue(new PrintServer(), settings.PrinterName);
            printDialog.PrintVisual(printMultipleTicket, "In vé");
            printMultipleTicket.Close();
        }

        #endregion

    }


}
