using System.Windows.Input;

namespace FlightTicketSell.ViewModels
{
    public class BookViewModel : BaseViewModel
    {
        public string Title { get; } = "ĐẶT CHỖ";

        #region Commands

        /// <summary>
        /// The command to continue
        /// </summary>
        public ICommand ContinueCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public BookViewModel()
        {
            // Create commands
            ContinueCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.BookDetail);

        }

        #endregion
    }
}
