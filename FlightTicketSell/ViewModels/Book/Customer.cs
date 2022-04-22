using FlightTicketSell.Models;

namespace FlightTicketSell.ViewModels
{
    /// <summary>
    /// A specialize version of <see cref="KHACHHANG"/> for book view
    /// </summary>
    public class Customer : KHACHHANG
    {
        /// <summary>
        /// The index of the customer in the list
        /// </summary>
        public int Index { get; set; }
    }
}
