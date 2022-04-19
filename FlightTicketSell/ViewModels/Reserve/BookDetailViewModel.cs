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

        #endregion

        #region Constructor

        public BookDetailViewModel()
        {
            // Create commands
            ContinueCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.ReservePay);
            ReturnCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.Book);
        }

        #endregion
    }
}
