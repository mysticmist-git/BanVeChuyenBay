using FlightTicketSell.Models;
using FlightTicketSell.Models.SearchRelated;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;
using System.ComponentModel;
using System.Windows.Data;
using FlightTicketSell.Interface;
using FlightTicketSell.IoC;

namespace FlightTicketSell.ViewModels
{
    public class BookDetailViewModel : BaseViewModel
    {
        #region Interface

        /// <summary>
        /// The book detail interface
        /// </summary>
        public IBookDetail BookDetailInterface { get; set; }

        #endregion

        #region Private Members

        /// <summary>
        /// Indicates if this list included the booking person
        /// </summary>
        private bool _isBookingCustomerIncluded;

        #endregion

        #region Public Properties        

        /// <summary>
        /// Stores flight info
        /// </summary>
        public FlightInfo FlightInfo { get => IoC.IoC.Get<FlightDetailViewModel>().FlightInfo; }    

        /// <summary>
        /// Stores ticket tiers info
        /// </summary>
        public ObservableCollection<TicketTier> TicketTiers { get => IoC.IoC.Get<FlightDetailViewModel>().TicketTier; }

        /// <summary>
        /// Stores current ticket tier being selected
        /// </summary>
        public TicketTier CurrentTicketTier { get; set; }

        /// <summary>
        /// Store booking info
        /// </summary>
        public Book BookInfo { get; set; } = new Book();

        /// <summary>
        /// The customer list that we'll fill information
        /// </summary>
        public ObservableCollection<CustomerBookVariant> Customers { get; set; } = new ObservableCollection<CustomerBookVariant>();

        /// <summary>
        /// Indicates if this list included the booking person
        /// </summary>
        public bool IsBookingCustomerIncluded
        {
            get => _isBookingCustomerIncluded;
            set
            {
                _isBookingCustomerIncluded = value;

                ToggleIncludeBookingCustomer();
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// The command to return to previous view
        /// </summary>
        public ICommand ReturnCommand { get; set; }

        /// <summary>
        /// The command to continue
        /// </summary>
        public ICommand ContinueCommand { get; set; }

        /// <summary>
        /// The command which is execute when user control load
        /// </summary>
        public ICommand LoadCommand { get; set; }

        /// <summary>
        /// Add a new customer to book list
        /// </summary>
        public ICommand AddCustomerCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Defualt constuctor
        /// </summary>
        public BookDetailViewModel()
        {
            // Create commands
            LoadCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                // Initializes
                IsBookingCustomerIncluded = true;

                if (CurrentTicketTier is null)
                    CurrentTicketTier = TicketTiers.ElementAt(0);
            });

            // Create command
            ContinueCommand = new RelayCommand<IBookDetail>((p) => true, (p) =>
             {
                // Check if all field filled
                if (!p.IsValid)
                 {
                     p.ForceUpdateSource();
                     return;
                 }

                 IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.BookPay;

                // Update Book Information (SoVeDat)
                BookInfo.SoVeDat = Customers.Count;
                BookInfo.GiaTien_Ve = CurrentTicketTier.GiaTien;
             });

            ReturnCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.FlightDetail;

                IoC.IoC.Rebind<BookDetailViewModel>();
            });

            AddCustomerCommand = new RelayCommand<object>((p) => true, (p) => 
            {
                // Add a new customer
                Customers.Add(new CustomerBookVariant { Index = Customers.Count + 1 });
            });
        }

        #endregion

        #region Methods

        /// <summary>
        /// Includes, or excluds the booking customer in the customer list
        /// </summary>
        private void ToggleIncludeBookingCustomer()
        {
            if (IsBookingCustomerIncluded)
            {
                var bookingCustomer = new CustomerBookVariant()
                {
                    Index = 1,
                    HoTen = BookInfo.ThongTinNguoiDat.HoTen,
                    CMND = BookInfo.ThongTinNguoiDat.CMND,
                    SDT = BookInfo.ThongTinNguoiDat.SDT,
                    Email = BookInfo.ThongTinNguoiDat.Email,
                    IsBookingCustomer = true
                };

                Customers.Insert(0, bookingCustomer);
            }
            else
            {
                Customers.RemoveAt(0);
            }
        }

        /// <summary>
        /// Clear this view information
        /// </summary>
        public void ClearData()
        {
            BookInfo.ThongTinNguoiDat = new Customer();
            CurrentTicketTier = null;
            Customers = new ObservableCollection<CustomerBookVariant>();
        }

        #endregion

    }
}
