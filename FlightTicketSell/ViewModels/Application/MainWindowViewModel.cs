using FlightTicketSell.Interface;
using FlightTicketSell.Views;
using System.Windows;
using System.Windows.Input;

namespace FlightTicketSell.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        public string DisplayUser { get => IoC.IoC.Get<ApplicationViewModel>().CurrentUserGroup.Name + " - " + IoC.IoC.Get<ApplicationViewModel>().CurrentUser.Username; }

        #region Commands

        /// <summary>
        /// The command executes on view load
        /// </summary>
        public ICommand LoadCommand { get; set; }

        public ICommand LogoutCommand { get; set; }

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

            LogoutCommand = new RelayCommand<object>(p => true, p =>
           {
               var login = new LoginView();
               Application.Current.MainWindow.Close();
               Application.Current.MainWindow = login;
               Application.Current.MainWindow.Show();
           });
        }

        #endregion
    }
}
