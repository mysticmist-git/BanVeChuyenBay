using FlightTicketSell.ViewModels;
using System.Collections.ObjectModel;

namespace FlightTicketSell.Models
{
    /// <summary>
    /// The user of this application
    /// </summary>
    public class User : BaseViewModel
    {
        #region Public Properties

        public string Username { get; set; }

        public string Password { get; set; }

        public string UserGroupID { get; set; }

        #endregion
    }

}
