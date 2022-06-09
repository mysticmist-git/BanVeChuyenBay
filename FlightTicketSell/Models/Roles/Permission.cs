using FlightTicketSell.Models.Enums;
using FlightTicketSell.ViewModels;
using System.Collections.Generic;

namespace FlightTicketSell.Models.Roles
{

    public class Permission : BaseViewModel
    {
        /// <summary>
        /// The permission code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// The permission name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The function code this permission contains
        /// </summary>
        public string FunctionCode { get; set; }
    }
}
