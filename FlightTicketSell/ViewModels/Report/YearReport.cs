    using FlightTicketSell.Views.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightTicketSell.ViewModels
{
    public class YearReport
    {
        #region Private Members

        /// <summary>
        /// The revennue of the year
        /// </summary>
        private decimal revenue;

        /// <summary>
        /// This year's revenue compare to total revenue
        /// </summary>
        private decimal ratio;

        #endregion

        #region Public Properties 

        /// <summary>
        /// The year of the report
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Flight count of the year
        /// </summary>
        public int FlightCount { get; set; }


        /// <summary>
        /// The display revennue of the year
        /// </summary>
        public string Revenue { get => ReportHelper.VietnamCurrencyConvert(revenue) + " VNĐ"; set => revenue = decimal.Parse(value); }

        /// <summary>
        /// This display year's revenue compare to total revenue
        /// </summary>
        public string Ratio { get => (ratio * 100).ToString() + "%"; set => ratio = decimal.Parse(value); }

        #endregion

        #region Methods

        /// <summary>
        /// Get the revenue of the report
        /// </summary>
        /// <returns></returns>
        public decimal GetRevenue() => revenue;

        #endregion
    }
}
