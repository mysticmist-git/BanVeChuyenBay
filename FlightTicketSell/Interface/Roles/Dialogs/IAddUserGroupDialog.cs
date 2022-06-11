using FlightTicketSell.Models;
using FlightTicketSell.Models.Enums;
using System.Windows;

namespace FlightTicketSell.Interface
{
    public interface IAddUserGroupDialog
    {
        /// <summary>
        /// Close the dialog
        /// </summary>
        void Close();

        /// <summary>
        /// When user cancel and want to close dialog, ask them if they certain about it
        /// </summary>
        MessageBoxResult CancelConfirm();

        /// <summary>
        /// Notify user with a messagebox  
        /// </summary>
        /// <param name="result">Result lead to this notify</param>
        /// <param name="args">Arguments for the notify, possibly <see cref="UserGroup"/></param>
        void Notify(ActionResult result, object args = null);
    }
}
