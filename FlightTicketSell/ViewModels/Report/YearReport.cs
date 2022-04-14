using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightTicketSell.ViewModels.Report
{
    public class YearReport
    {
        #region Public Properties 

        /// <summary>
        /// The year of the report
        /// </summary>
        public string Year { get; set; }

        /// <summary>
        /// Flight count of the year
        /// </summary>
        public string FlightCount { get; set; }

        /// <summary>
        /// The revennue of the year
        /// </summary>
        public string Revenue { get; set; }

        /// <summary>
        /// This year's revenue compare to total revenue
        /// </summary>
        public string Ratio { get; set; }

        #endregion
    }
}
