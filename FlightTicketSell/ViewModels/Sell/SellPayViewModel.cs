using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using FlightTicketSell.Models;
using System.Data.Entity.Core;
using System.Data.Entity;
using FlightTicketSell.Models.SearchRelated;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Net.Mail;
using System.Threading.Tasks;

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
        public KHACHHANG Customer { get => IoC.IoC.Get<TicketInfoFillingViewModel>().Customer; }

        /// <summary>
        /// Stores current selected ticket tier
        /// </summary>
        public TicketTier CurrentTicketTier { get => IoC.IoC.Get<TicketInfoFillingViewModel>().CurrentTicketTier; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public SellPayViewModel()
        {
            // Create commands
            ReturnCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.TicketInfoFilling);

            PayCommand = new RelayCommand<object>((p) => true, async (p) =>
            {
                // Save things
                await SaveTicketInformation();

                // Clear view models
                IoC.IoC.Get<TicketInfoFillingViewModel>().Customer = new Customer();
                IoC.IoC.Get<TicketInfoFillingViewModel>().DuplicatedCustomer = new Customer();

                // Navigate back to flight detail view
                IoC.IoC.Get<ApplicationViewModel>().CurrentView = AppView.FlightDetail;
            });
        }

        #endregion

        #region Methods

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
                        context.KHACHHANGs.Add(Customer);

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
                    DialogResult res = (DialogResult)System.Windows.MessageBox.Show("Mua ve thanh cong, quay lai trang chu", "He thong", MessageBoxButton.OK);

                    if (res == DialogResult.OK)
                        await SendMail();

                }
                catch (EntityException)
                {
                    // TODO: messagebox vo
                    return;
                }
            }
        }

        private async Task SendMail()
        {
            // Send email to user
            string to = Customer.Email; //To address    

            // enter your email
            string from = "flightsystem53@gmail.com"; //From address    

            MailMessage message = new MailMessage(from, to);

            string mailbody =
                "<body>" +
                "<h2>Payment comfirmation</h2>" + Environment.NewLine +
                "<a>Dear  </a>" + Customer.HoTen + Environment.NewLine +
                "<a>. Here are some information about the ticket</a>" + Environment.NewLine +
                "<a>. Your flight code is </a>" + FlightInfo.DisplayFlightCode + Environment.NewLine +
                "<a>. The flight will start at </a>" + FlightInfo.NgayGio + Environment.NewLine +
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

            try
            {
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
