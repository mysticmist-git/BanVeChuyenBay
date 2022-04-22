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

        #endregion

        #region Public Properties

        /// <summary>
        /// The customer list that we'll fill information
        /// </summary>
        public ObservableCollection<Customer> Customers { get; set; } = new ObservableCollection<Customer>();

        #endregion

        #region Constructor

        public BookDetailViewModel()
        {
            // Create command
            ContinueCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.ReservePay);
            ReturnCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.Book);
            LoadCommand = new RelayCommand<object>((p) => true, (p) => 
            {
            });
        } 
        #endregion
    }
}
