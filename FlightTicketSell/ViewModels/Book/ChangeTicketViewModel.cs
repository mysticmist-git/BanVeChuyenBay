using FlightTicketSell.Views;
using FlightTicketSell.Models;
using FlightTicketSell.ViewModels.Sell;
using FlightTicketSell.ViewModels.Search;
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
using iText.Layout.Properties;
using Paragraph = iText.Layout.Element.Paragraph;
using TextAlignment = System.Windows.TextAlignment;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf.Canvas.Draw;

using iText.Pdfa;
using FlightTicketSell.ViewModels;
using System.Drawing;
using iText.IO.Font.Constants;

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


        public ICommand BookPay{ get; set; }
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
        public ObservableCollection<Customer> Customers { get; set; }

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

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ChangeTicketViewModel()
        {
            // Create commands

            ReturnCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.TicketSoldOrBooked);

            BookPay = new RelayCommand<object>((p) => true, (p) =>
            {
                PdfWriter writer = new PdfWriter("demo.pdf");
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);
          
                
                Paragraph header = new Paragraph("FLIGHT TICKET").SetBold()
                   .SetFontSize(16).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);

                document.Add(header);
                LineSeparator ls = new LineSeparator(new SolidLine());

                Paragraph NAME = new Paragraph($"BOOKER NAME:   {convertText(BookingInfo.ThongTinNguoiDat.HoTen)}                    BOOK ID: {BookingInfo.MaDatCho}").SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT).SetFontSize(14);
                Paragraph FROM = new Paragraph($"FROM:   {convertText(FlightInfo.SanBayDi)}").SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT).SetFontSize(14);
                Paragraph TO = new Paragraph($"TO:   {convertText(FlightInfo.SanBayDen)}").SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT).SetFontSize(14);
                Paragraph NUMBER_OF_SEATS = new Paragraph($"NUMBER OF SEATS:   {BookingInfo.SoVeDat}       ").SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT).SetFontSize(14);
                document.Add(NAME);
                document.Add(FROM);
                document.Add(TO);
                document.Add(NUMBER_OF_SEATS);

                document.Add(ls);
                Paragraph DETAIL_HEADER = new Paragraph("FLIGHT CODE              DATE              CLASS").SetBold().SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER).SetFontSize(14);
                document.Add(DETAIL_HEADER);
                Paragraph DETAIL = new Paragraph($"{FlightInfo.DisplayFlightCode}           {FlightInfo.DisplayDepartDate}           {convertText(BookingInfo.TenHangVe)}").SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER).SetFontSize(14);
                document.Add(DETAIL);
        
                document.Add(ls);
                document.Close();
                System.Diagnostics.Process.Start("demo.pdf");

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
                        Customers = new ObservableCollection<Customer>();
                        for (int i = 1; i <= result.Count(); i++)
                            Customers.Add(new Customer(result.ElementAt(i - 1)) { Index = i });
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
                        
                        datCho.TrangThai = BookingStateToString(BookingState.Cancel);
                        await context.SaveChangesAsync();

                        // Update client
                        BookingInfo.BookingState = BookingState.Cancel;
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

        public string BookingStateToString(BookingState state)
        {
            switch (state)
            {
                case BookingState.NotChanged:
                    return "ChuaDoi";
                case BookingState.Changed:
                    return "DaDoi";
                case BookingState.Cancel:
                    return "DaHuy";
                default:
                    return null;

            }
        }

        private static readonly string[] VietnameseSigns = new string[]
        {

            "aAeEoOuUiIdDyY",

            "áàạảãâấầậẩẫăắằặẳẵ",

            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",

            "éèẹẻẽêếềệểễ",

            "ÉÈẸẺẼÊẾỀỆỂỄ",

            "óòọỏõôốồộổỗơớờợởỡ",

            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",

            "úùụủũưứừựửữ",

            "ÚÙỤỦŨƯỨỪỰỬỮ",

            "íìịỉĩ",

            "ÍÌỊỈĨ",

            "đ",

            "Đ",

            "ýỳỵỷỹ",

            "ÝỲỴỶỸ"
        };

        public static string convertText(string str)
        {
            for (int i = 1; i < VietnameseSigns.Length; i++)
            {
                for (int j = 0; j < VietnameseSigns[i].Length; j++)
                    str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
            }
            return str;
        }

        #endregion

    }
}
