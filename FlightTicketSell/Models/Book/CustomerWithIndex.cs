using FlightTicketSell.ViewModels;

namespace FlightTicketSell.Models
{
    public class CustomerWithIndex : Customer
    {
        #region Public Properties

        /// <summary>
        /// The index of this customer in some list
        /// </summary>
        public int Index { get; set; }
        public string DisplayIndex { get => "Khách hàng " + Index; }
        #endregion

        #region Constructor

        /// <summary>
        /// Copy constructor
        /// </summary>
        public CustomerWithIndex(KHACHHANG kh) : base(kh) { }

        /// <summary>
        /// Default constructor
        /// </summary>
        public CustomerWithIndex()
        {

        }

        #endregion
    }
}