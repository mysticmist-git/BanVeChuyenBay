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
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Paragraph = iText.Layout.Element.Paragraph;
using iText.Kernel.Pdf.Canvas.Draw;
using FlightTicketSell.Helpers;

namespace FlightTicketSell.ViewModels
{
    public class ChangeTicketViewModel : BaseViewModel
    {
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
                        MessageBox.Show("Database access failed!", string.Format($"Exception: {e.Message}"), MessageBoxButton.OK, MessageBoxImage.Error);
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
                        MessageBox.Show("Database access failed!", string.Format($"Exception: {e.Message}"), MessageBoxButton.OK, MessageBoxImage.Error);
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
                    }
                    catch (EntityException e)
                    {
                        MessageBox.Show("Database access failed!", string.Format($"Exception: {e.Message}"), MessageBoxButton.OK, MessageBoxImage.Error);
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

            PdfWriter writer = new PdfWriter("demo.pdf");
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf);


            Paragraph header = new Paragraph("FLIGHT TICKET").SetBold()
               .SetFontSize(16).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);

            document.Add(header);
            LineSeparator ls = new LineSeparator(new SolidLine());

            Paragraph NAME = new Paragraph($"BOOKER NAME:   {BookHelper.convertText(BookingInfo.ThongTinNguoiDat.HoTen)}                    BOOK ID: {BookingInfo.MaDatCho}").SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT).SetFontSize(14);
            Paragraph FROM = new Paragraph($"FROM:   {BookHelper.convertText(FlightInfo.SanBayDi)}").SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT).SetFontSize(14);
            Paragraph TO = new Paragraph($"TO:   {BookHelper.convertText(FlightInfo.SanBayDen)}").SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT).SetFontSize(14);
            Paragraph NUMBER_OF_SEATS = new Paragraph($"NUMBER OF SEATS:   {BookingInfo.SoVeDat}       ").SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT).SetFontSize(14);
            document.Add(NAME);
            document.Add(FROM);
            document.Add(TO);
            document.Add(NUMBER_OF_SEATS);

            document.Add(ls);
            Paragraph DETAIL_HEADER = new Paragraph("FLIGHT CODE              DATE              CLASS").SetBold().SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER).SetFontSize(14);
            document.Add(DETAIL_HEADER);
            Paragraph DETAIL = new Paragraph($"{FlightInfo.DisplayFlightCode}           {FlightInfo.DisplayDepartDate}           {BookHelper.convertText(BookingInfo.TenHangVe)}").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER).SetFontSize(14);
            document.Add(DETAIL);

            document.Add(ls);
            document.Close();
            System.Diagnostics.Process.Start("demo.pdf");
        }

        #endregion

    }
}
