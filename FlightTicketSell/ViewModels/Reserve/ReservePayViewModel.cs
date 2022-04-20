using System.Windows.Input;

namespace FlightTicketSell.ViewModels
{
    public class ReservePayViewModel : BaseViewModel
    {
        #region Commands

        /// <summary>
        /// The command to return to previous view
        /// </summary>
        public ICommand ReturnCommand { get; set; }

        #endregion

        #region Constructor

        public ReservePayViewModel()
        {
            // Create commands
            ReturnCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.BookDetail);
        }
        #endregion
    }
}
