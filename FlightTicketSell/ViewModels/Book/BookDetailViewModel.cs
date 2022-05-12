using FlightTicketSell.Models;
using FlightTicketSell.Models.SearchRelated;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;

namespace FlightTicketSell.ViewModels
{
    public class BookDetailViewModel : BaseViewModel
    {
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

        #region Public Properties        
        public FlightInfo FlightInfo { get => IoC.IoC.Get<FlightDetailViewModel>().FlightInfo; }    

        public ObservableCollection<TicketTier> TicketTiers { get => IoC.IoC.Get<FlightDetailViewModel>().TicketTier; }

        public TicketTier CurrentTicketTier { get; set; }

        public Book BookInfo { get; set; } = new Book();
 
        /// <summary>
        /// The customer list that we'll fill information
        /// </summary>
        public ObservableCollection<Customer> Customers { get; set; } = new ObservableCollection<Customer>() { new Customer { Index = 1 } };

        #endregion

        #region Constructor

        public BookDetailViewModel()
        {
            LoadCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                if (CurrentTicketTier is null)
                    CurrentTicketTier = TicketTiers.ElementAt(0);
            });

           // Create command
           ContinueCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.BookPay;

                // Update Book Information (SoVeDat)
                BookInfo.SoVeDat = Customers.Count;
                BookInfo.GiaTien_Ve = CurrentTicketTier.GiaTien;
            });

            ReturnCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.FlightDetail);

            AddCustomerCommand = new RelayCommand<object>((p) => true, (p) => 
            {
                // Add a new customer
                Customers.Add(new Customer { Index = Customers.Count + 1 });
            });
        } 
        #endregion
    }
}
