using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FlightTicketSell.ViewModels
{
    public class ChangeTicketViewModel : BaseViewModel
    {
        #region Commands

        /// <summary>
        /// The command to continue
        /// </summary>
        public ICommand PrintTicketCommand { get; set; }

        public ICommand CancelTicketCommand { get; set; }

        public ICommand ReturnCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ChangeTicketViewModel()
        {
            // Create commands

            ReturnCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.SellPay);

            //PrintTicketCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.);

            //CancelTicketCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.);
        }

        #endregion

    }
}
