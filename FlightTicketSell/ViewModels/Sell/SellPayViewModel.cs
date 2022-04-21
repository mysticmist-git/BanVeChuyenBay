using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FlightTicketSell.ViewModels
{
    public class SellPayViewModel : BaseViewModel 
    {
        #region Commands

        /// <summary>
        /// The command to continue
        /// </summary>
        public ICommand ReturnCommand { get; set; }

        public ICommand PayCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public SellPayViewModel()
        {
            // Create commands
            ReturnCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.TicketInfo);

            PayCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.ChangeTicket);
        }

        #endregion
    }
}
