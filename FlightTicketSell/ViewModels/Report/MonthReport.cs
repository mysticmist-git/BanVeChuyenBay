using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightTicketSell.ViewModels.Report
{
    public class MonthReport
    {
        #region Public Properties

        /// <summary>
        /// The month of the report
        /// </summary>
        public string Month { get; set; }

        /// <summary>
        /// Flight count of the month
        /// </summary>
        public string FlightReport { get; set; }

        /// <summary>
        /// The revennue of the month
        /// </summary>
        public string Revenue { get; set; }

        /// <summary>
        /// This month revenue compare to this year revenue
        /// </summary>
        public string Ratio { get; set; }

        #endregion
    }
}
