using FlightTicketSell.Helpers;
using System;

namespace FlightTicketSell.Models.Report
{
    public class FlightReport
    {
        #region Public Properties

        /// <summary>
        /// The display flight code
        /// </summary>
        public string DisplayFlightCode { get; set; }

        /// <summary>
        /// The ticket count
        /// </summary>
        public int TicketCount { get; set; }

        /// <summary>
        /// The depart date
        /// </summary>
        public DateTime DepartDate { get; set; }

        /// <summary>
        /// The display depart time
        /// </summary>
        public string DisplayDepartDate { get => FormatHelper.DateFormat(DepartDate); }

        /// <summary>
        /// The revenue
        /// </summary>
        public decimal Revenue { get; set; }

        /// <summary>
        /// The display revenue
        /// </summary>
        public string DisplayRevenue { get => FormatHelper.VietnamCurrencyFormat(Revenue) + " VNĐ"; }

        /// <summary>
        /// The ratio
        /// </summary>
        public decimal Ratio { get; set; }

        /// <summary>
        /// The display ratio
        /// </summary>
        public string DisplayRatio { get => FormatHelper.DecimalToPercentConverter(Ratio); }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FlightReport() { }

        #endregion
    }
}
