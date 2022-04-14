using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightTicketSell.ViewModels.Report
{
    public class FlightReportViewModel : BaseViewModel
    {
        /// <summary>
        /// The code of the this flight
        /// </summary>
        public string FlightCode { get; set; }

        /// <summary>
        /// The Date, Time this flight depart
        /// </summary>
        public string DepartTime { get; set; }

        /// <summary>
        /// The amount of ticket sold
        /// </summary>
        public string TicketSold { get; set; }

        /// <summary>
        /// The revennue of this flight
        /// </summary>
        public string Revenue { get; set; }

        /// <summary>
        /// This flight revenue compare to month's revenue
        /// </summary>
        public string Ratio { get; set; }
    }
}
