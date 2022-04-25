using FlightTicketSell.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;
using System.Windows;

namespace FlightTicketSell.ViewModels
{
    public class BookViewModel : BaseViewModel
    {
        #region Public Properties
        #endregion

        #region Commands

        /// <summary>
        /// The command to continue
        /// </summary>
        public ICommand ContinueCommand { get; set; }

        /// <summary>
        /// The command which execute when view loaded
        /// </summary>
        public ICommand LoadCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public BookViewModel()
        {
            // Create commands
            ContinueCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                // Change to next view
                IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.BookDetail;
 
            });
        }

        #endregion
    }
}
