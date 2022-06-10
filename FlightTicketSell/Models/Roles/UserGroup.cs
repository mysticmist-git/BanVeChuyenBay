using FlightTicketSell.ViewModels;

namespace FlightTicketSell.Models
{
    public class UserGroup : BaseViewModel
    {
        /// <summary>
        /// The code of this user group
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// The name of this user group
        /// </summary>
        public string Name { get; set; }
    }
}
