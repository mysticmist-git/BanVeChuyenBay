using FlightTicketSell.Views.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightTicketSell.ViewModels
{
    public class MonthReport
    {
        #region Private Members

        /// <summary>
        /// The revenue of the month
        /// </summary>
        private decimal revenue;

        /// <summary>
        /// This month revenue compare to this year revenue
        /// </summary>
        private decimal ratio;

        #endregion

        #region Public Properties

        /// <summary>
        /// The month of the report
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// Flight count of the month
        /// </summary>
        public int FlightCount { get; set; }

        /// <summary>
        /// The display revenue of the month
        /// </summary>
        public string Revenue { get => ReportHelper.VietnamCurrencyConvert(revenue) + " VNĐ"; set => revenue = decimal.Parse(value); }

        /// <summary>
        /// This display month revenue compare to this year revenue
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
