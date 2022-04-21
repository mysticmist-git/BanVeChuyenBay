using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FlightTicketSell.ViewModels
{
    public class TicketInfoViewModel : BaseViewModel
    {
        #region Commands

        /// <summary>
        /// The command to continue
        /// </summary>
        public ICommand ContinueCommand { get; set; }

        public ICommand ReturnCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public TicketInfoViewModel()
        {
            // Create commands
            ContinueCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.SellPay);

            ReturnCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.Sell);
        }

        #endregion

    }
}
