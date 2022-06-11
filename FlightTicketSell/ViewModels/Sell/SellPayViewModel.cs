using System;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using FlightTicketSell.Models;
using System.Data.Entity.Core;
using System.Data.Entity;
using FlightTicketSell.Models.SearchRelated;
using System.Collections.ObjectModel;
using System.Net.Mail;
using System.Threading.Tasks;
using FlightTicketSell.Helpers;
using FlightTicketSell.ViewModels.Search;
using System.Drawing.Printing;
using FlightTicketSell.Views;
using System.Printing;
using System.Windows;

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
        /// Indicates if the customer filled is already in database or not
        /// </summary>
        public bool IsCustomerNew { get => IoC.IoC.Get<TicketInfoFillingViewModel>().IsCustomerNew; }

        /// <summary>
        /// Stores flight information
        /// </summary>
        public DetailFlilghtInfo FlightInfo { get => IoC.IoC.Get<FlightDetailViewModel>().FlightInfo; }

        /// <summary>
        /// Stores overlay airports of this flight
        /// </summary>
        public ObservableCollection<OverlayAirport_Search> OverlayAirports { get => IoC.IoC.Get<FlightDetailViewModel>().OverlayAirport; }

        /// <summary>
        /// Stores customer information
        /// </summary>
        public Customer Customer { get => IoC.IoC.Get<TicketInfoFillingViewModel>().Customer; }

        /// <summary>
        /// Stores current selected ticket tier
        /// </summary>
        public TicketTier CurrentTicketTier { get => IoC.IoC.Get<TicketInfoFillingViewModel>().CurrentTicketTier; }

        public bool IsSellable { get; set; } = true;

        #endregion

        #region Print
        public HANGVE hANGVE { get; set; }
        public KHACHHANG kHACHHANG { get; set; }
        public CHUYENBAY cHUYENBAY { get; set; }
        public ObservableCollection<PrintTicket> list_printTickets { get; set; } = new ObservableCollection<PrintTicket>();
        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public SellPayViewModel()
        {
            LoadCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                IsSellable = false;

                using (var context = new FlightTicketSellEntities())
                {
                    try
                    {
                        hANGVE = context.HANGVEs.Where(h => h.MaHangVe == CurrentTicketTier.MaHangVe).FirstOrDefault();
                        kHACHHANG = new KHACHHANG()
                        {
                            HoTen=Customer.HoTen
                        };
                        cHUYENBAY = context.CHUYENBAYs.Where(h => h.MaChuyenBay == FlightInfo.MaChuyenBay).FirstOrDefault();
                    }
                    catch (EntityException e)
                    {
                        NotifyHelper.ShowEntityException(e);
                    }
                }

                IsSellable = true;
            });

            // Create commands
            ReturnCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                //IoC.IoC.Get<TicketInfoFillingViewModel>().ClearData();
                IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.TicketInfoFilling;
            });

            PayCommand = new RelayCommand<object>((p) => true, async (p) =>
            {
                IsSellable = false;

                // Save things
                await SaveTicketInformation();
                PrintTicket();

                // Navigate back to flight detail view
                IoC.IoC.Get<ApplicationViewModel>().CurrentView = AppView.FlightDetail;

                // Cache customer and flight info
                var flightInfo = FlightInfo;
                var customer = Customer;

                // Send mail
                //await SendMail(flightInfo, customer);

                IsSellable = true;
            });
        }

        #endregion

        #region Methods

        private void PrintTicket()
        {
            try
            {
                list_printTickets.Clear();
                PrintMultipleTicket printMultipleTicket = new PrintMultipleTicket() { DataContext = this };
                list_printTickets.Add(new PrintTicket(hANGVE, kHACHHANG, cHUYENBAY));
                printMultipleTicket.Show();
                PrintDialog printDialog = new PrintDialog();
                PrinterSettings settings = new PrinterSettings();
                printDialog.PrintQueue = new PrintQueue(new PrintServer(), settings.PrinterName);
                printDialog.PrintVisual(printMultipleTicket, "In vé");
                printMultipleTicket.Close();
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Saves customer information, ticket information down database
        /// </summary>
        private async Task SaveTicketInformation()
        {
            using (var context = new FlightTicketSellEntities())
            {
                try
                {
                    // Add new customer to database
                    if (IsCustomerNew)
                    {
                        context.KHACHHANGs.Add(new KHACHHANG()
                        {
                            HoTen = Customer.HoTen,
                            CMND = Customer.CMND,
                            SDT = Customer.SDT,
                            Email = Customer.Email
                        });

                        await context.SaveChangesAsync();
                    }

                    // Customer code for add new ticket later
                    var CustomerCode = await context.KHACHHANGs.Where(kh => kh.CMND == Customer.CMND).Select(kh => kh.MaKhachHang).FirstOrDefaultAsync();

                    /*--------INSERT VE -----------*/
                    // insert new ticket to database
                    var ve = new VE()
                    {
                        MaChuyenBay = FlightInfo.MaChuyenBay,
                        MaKhachHang = CustomerCode,
                        GiaTien = CurrentTicketTier.GiaTien,
                        NgayThanhToan = DateTime.Now,
                        MaHangVe = CurrentTicketTier.MaHangVe,
                    };

                    context.VEs.Add(ve);
                    await context.SaveChangesAsync();

                    // messagebox to notify sucessful payment
                    System.Windows.MessageBox.Show("Mua vé thành công", "Cảnh báo", MessageBoxButton.OK);

                    FlightInfo.GheTrong--;
                }
                catch (EntityException e)
                {
                    NotifyHelper.ShowEntityException(e);
                }
            }
        }

        private async Task SendMail(DetailFlilghtInfo flightInfo, Customer customer)
        {
            try
            {
                // Send email to user
                string to = customer.Email; //To address
                if (to == null)
                    return;

                // enter your email
                string from = "flightsystem53@gmail.com"; //From address    

                MailMessage message = new MailMessage(from, to);

                string mailbody =
                    "<body>" +
                    "<h2>Payment comfirmation</h2>" + Environment.NewLine +
                    "<a>Dear  </a>" + customer.HoTen + Environment.NewLine +
                    "<a>. Here are some information about the ticket</a>" + Environment.NewLine +
                    "<a>. Your flight code is </a>" + flightInfo.DisplayFlightCode + Environment.NewLine +
                    "<a>. The flight will start at </a>" + flightInfo.NgayGio + Environment.NewLine +
                    "<a>. Please take note the information above!!!  </a>" + Environment.NewLine +
                    "<a>. Hope you have a great flight !!! </a>" +
                    "</body>";

                message.Subject = "Flight ticket payment";

                message.Body = mailbody;

                message.BodyEncoding = Encoding.UTF8;

                message.IsBodyHtml = true;

                SmtpClient client = new SmtpClient("smtp.gmail.com", 587); //Gmail smtp    

                System.Net.NetworkCredential basicCredential1 = new

                System.Net.NetworkCredential("flightsystem53@gmail.com", "flightsystem123");

                client.EnableSsl = true;

                client.UseDefaultCredentials = false;

                client.Credentials = basicCredential1;
                await client.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
    #endregion
}
