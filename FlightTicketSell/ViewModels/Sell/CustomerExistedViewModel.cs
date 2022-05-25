using FlightTicketSell.Models;
using MaterialDesignThemes.Wpf;
using System.Windows.Input;

namespace FlightTicketSell.ViewModels
{
    public class CustomerExistedViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// The customer whose CMND duplicated
        /// </summary>
        public Customer DuplicatedCustomer { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// The command to close the dialog host
        /// </summary>
        public ICommand CloseCommand { get; set; }
        public ICommand ReuseCommand { get; set; }


        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public CustomerExistedViewModel()
        {
            // Create commands
            CloseCommand = new RelayCommand<object>(p => true, p => DialogHost.CloseDialogCommand.Execute(0, null));
            ReuseCommand = new RelayCommand<object>(p => true, p => DialogHost.CloseDialogCommand.Execute(1, null));
        }

        #endregion
    }
}
