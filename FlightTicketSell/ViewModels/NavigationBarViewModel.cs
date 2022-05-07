using FlightTicketSell.Models;
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
            SearchCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = AppView.Search);
            //BookCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = AppView.Book);
            //SellCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = AppView.Sell);
            ScheduleCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = AppView.Schedule);
            ReportCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = AppView.Report);
            SettingCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = AppView.Setting);
        }

        #endregion
    }
}
