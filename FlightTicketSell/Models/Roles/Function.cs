using FlightTicketSell.Models.Enums;
using FlightTicketSell.ViewModels;
using System.Collections.Generic;

namespace FlightTicketSell.Models
{
    /// <summary>
    /// The functions that user can have
    /// </summary>
    public class Function : BaseViewModel
    {
        /// <summary>
        /// The code of this function
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// The name of this function
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The view this function can loads
        /// </summary>
        public List<ViewType> LoadableView { get;set; } 
    }
}
