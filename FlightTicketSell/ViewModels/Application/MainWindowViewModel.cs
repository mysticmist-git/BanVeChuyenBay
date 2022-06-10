using System.Windows.Input;

namespace FlightTicketSell.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        #region Commands
        
        /// <summary>
        /// The command executes on view load
        /// </summary>
        public ICommand LoadCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MainWindowViewModel()
        {
            LoadCommand = new RelayCommand<object>(p => true, p =>
            {
                IoC.IoC.Get<ApplicationViewModel>().LoadFirstAvailableView();
            });
        }

        #endregion
    }
}
