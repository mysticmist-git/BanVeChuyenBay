using FlightTicketSell.Models;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FlightTicketSell.ViewModels
{
    public class NavigationBarViewModel : BaseViewModel
    {
        #region Public Properties
        

        #endregion

        #region Commands

        /// <summary>
        /// To open the search view
        /// </summary>
        public ICommand SearchCommand { get; set; }

        ///// <summary>
        ///// To open the ticket book view
        ///// </summary>
        //public ICommand BookCommand { get; set; }

        ///// <summary>
        ///// To open the ticket sell view
        ///// </summary>
        //public ICommand SellCommand { get; set; }

        /// <summary>
        /// To open the flight schedule view
        /// </summary>
        public ICommand ScheduleCommand { get; set; }

        /// <summary>
        /// To open the report view
        /// </summary>
        public ICommand ReportCommand { get; set; }

        /// <summary>
        /// To open the setting view
        /// </summary>
        public ICommand SettingCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public NavigationBarViewModel()
        {
            // Create command
            SearchCommand = new RelayCommand<object>((p) => true, async (p) =>
            {
                var checkResult = await Check();
                if (checkResult)
                    IoC.IoC.Get<ApplicationViewModel>().CurrentView = AppView.Search; 
            });
            //BookCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = AppView.Book);
            //SellCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = AppView.Sell);
            ScheduleCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = AppView.Schedule);
            ReportCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = AppView.Report);
            SettingCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = AppView.Setting);
        }

        #endregion

        #region Methods

        public async Task<bool> Check()
        {
            switch (IoC.IoC.Get<ApplicationViewModel>().CurrentView)
            {
                case AppView.FlightInfoEdit:
                    if (await IoC.IoC.Get<FlightInfoEditViewModel>().IsFlightInfoEdited())
                        if (IoC.IoC.Get<FlightInfoEditViewModel>().AskWhenLeaveUnsave() != System.Windows.MessageBoxResult.Cancel)
                            return true;
                    return false;
                default:
                    return true;
            }
        }

        #endregion
    }
}
