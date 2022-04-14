using FlightTicketSell.Views.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightTicketSell.ViewModels.Report
{
    public class FlightReport 
    {
        #region Private Members

        /// <summary>
        /// The Date, Time this flight depart
        /// </summary>
        private DateTime departTime;

        /// <summary>
        /// The amount of ticket sold
        /// </summary>
        private int ticketSold;

        /// <summary>
        /// The Revenue of this flight
        /// </summary>
        private decimal revenue;

        /// <summary>
        /// This flight revenue compare to month's revenue
        /// </summary>
        private decimal ratio { get; set; }

        #endregion

        #region Public Properties

        /// <summary>
        /// The code of the this flight
        /// </summary>
        public string FlightCode { get; set; }

        /// <summary>
        /// The display Date, Time this flight depart
        /// </summary>
        public string DepartTime
        {
            get
            {
                return departTime.ToString("HH:mm dd/mm/yyyy");
            }
            set => departTime = DateTime.Parse(value);
        }

        /// <summary>
        /// The amount of ticket sold
        /// </summary>
        public string TicketSold { get => ticketSold.ToString(); set => ticketSold = int.Parse(value); }

        /// <summary>
        /// The Revenue of this flight
        /// </summary>
        public string Revenue { get => ReportHelper.VietnamCurrencyConvert(revenue) + " VNĐ"; set => revenue = decimal.Parse(value);  }

        /// <summary>
        /// This flight revenue compare to month's revenue
        /// </summary>
        public string Ratio { get => (ratio * 100).ToString() + "%"; set => ratio = decimal.Parse(value);  }

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
