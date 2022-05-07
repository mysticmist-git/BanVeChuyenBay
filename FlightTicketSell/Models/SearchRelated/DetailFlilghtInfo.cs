using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightTicketSell.Models.SearchRelated
{
    public class DetailFlilghtInfo : FlightInfo
    {
        #region Public Properties

        /// <summary>
        /// Flight time
        /// </summary>
        public int ThoiGianBay { get; set; }

        /// <summary>
        /// Flight route code
        /// </summary>
        public int MaDuongBay { get; set; }

        #endregion

        #region Construcotr

        /// <summary>
        /// Copy constructor
        /// </summary>
        public DetailFlilghtInfo(FlightInfo f) : base(f) { }

        /// <summary>
        /// Default constructor
        /// </summary>
        public DetailFlilghtInfo() { }

        #endregion
    }
}
