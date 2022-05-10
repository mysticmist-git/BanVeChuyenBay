using FlightTicketSell.Views.Helper;
using System;

namespace FlightTicketSell.Models.SearchRelated
{
    public class TicketTier : HANGVE
    {
        /// <summary>
        /// The display ticket tier coefficient
        /// </summary>
        public string DisplayCoefficient { get => Convert.ToInt32(HeSo * 100) +  "%"; }
        
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
