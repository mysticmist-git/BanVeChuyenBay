using FlightTicketSell.Views.Helper;

namespace FlightTicketSell.Models.SearchRelated
{
    public class TicketTier_Search
    {
        /// <summary>
        /// The name of the ticket tier
        /// </summary>
        public string TenHangVe { get; set; }

        /// <summary>
        /// The price of the ticket tier for a specific flight
        /// </summary>
        public decimal GiaTien { get; set; }

        /// <summary>
        /// The display of the price of the ticket tier for a specific flight
        /// </summary>
        public string DisplayTicketPrice { get => ReportHelper.VietnamCurrencyConvert(GiaTien) + " VNĐ"; }

        /// <summary>
        /// Empty sits of this ticket tier of a specific flight
        /// </summary>
        public int GheTrong { get; set; }
    }
}
