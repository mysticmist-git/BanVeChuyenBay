using FlightTicketSell.Interface;
using FlightTicketSell.Models;
using FlightTicketSell.Models.Roles;
using FlightTicketSell.Views;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FlightTicketSell.ViewModels
{
    public class ApplicationViewModel : BaseViewModel
    {
        public IMainWindow MainWindowInterface { get; set; }
        
        #region Public Properites

        /// <summary>
        /// The current view of the application
        /// </summary>
        public AppView CurrentView { get; set; }

        /// <summary>
        /// The current user logging in the application
        /// </summary>
        public User CurrentUser { get; set; }

        /// <summary>
        /// The current user group of the current user logging in the application
        /// </summary>
        public UserGroupModified CurrentUserGroup { get; set; }

        #endregion

        #region Commands

        

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ApplicationViewModel()
        {
            
        }

        #endregion

        #region Methods

        public void LoadFirstAvailableView()
        {
            if (CurrentUserGroup is null)
            {
                CurrentView = AppView.None;
                return;
            }    

            if (CurrentUserGroup.CanSearchFlight)
            {
                CurrentView = AppView.Search;
                return;
            }

            if (CurrentUserGroup.CanScheduleFlight)
            {
                CurrentView = AppView.Schedule;
                return;
            }

            if (CurrentUserGroup.CanViewReport)
            {
                CurrentView = AppView.Report;
                return;
            }

            if (CurrentUserGroup.CanSettings)
            {
                CurrentView = AppView.Setting;
                return;
            }

            if (CurrentUserGroup.CanManageUser)
            {
                CurrentView = AppView.Roles;
                return;
            }

            CurrentView = AppView.None;
        }

        #endregion
    }
}
