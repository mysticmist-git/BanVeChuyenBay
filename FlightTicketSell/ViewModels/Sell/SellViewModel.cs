using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace FlightTicketSell.ViewModels
{
    public class SellViewModel : BaseViewModel
    {
        public string Title { get; } = "BÁN VÉ";
        #region Commands

        /// <summary>
        /// The command to continue
        /// </summary>
        public ICommand ContinueCommand { get; set; }

        public ICommand ChangeTicketCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public SellViewModel()
        {
            // Create commands
            ContinueCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.TicketInfo);

            ChangeTicketCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.ChangeTicket);
        }

        #endregion

    }
}
