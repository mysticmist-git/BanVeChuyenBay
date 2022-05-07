using FlightTicketSell.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

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

        /// <summary>
        /// The customer list that we'll fill information
        /// </summary>
        public ObservableCollection<Customer> Customers { get; set; } = new ObservableCollection<Customer>() { new Customer { Index = 1 } };

        #endregion

        #region Constructor

        public BookDetailViewModel()
        {
            // Create command
            ContinueCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.ReservePay);
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
