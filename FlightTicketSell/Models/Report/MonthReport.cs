using FlightTicketSell.Helpers;

namespace FlightTicketSell.Models.Report
{
    public class MonthReport
    {
        #region Public Properties

        /// <summary>
        /// The month this report take in
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// The flight count
        /// </summary>
        public int FlightCount { get; set; }

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
        public MonthReport() { }
        
        #endregion
    }
}
