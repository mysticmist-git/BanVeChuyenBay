namespace FlightTicketSell.Models
{
    /// <summary>
    /// The view the app presenting
    /// </summary>
    public enum AppView
    {
        /// <summary>
        /// None
        /// </summary>
        None,
        /// <summary>
        /// The plane search view
        /// </summary>
        Search,
        /// <summary>
        /// The ticket book view
        /// </summary>
        Book,
        /// <summary>
        /// The view to enter book information
        /// </summary>
        Customer,
        /// <summary>
        /// The view to enter customer information
        /// </summary>
        FlightDetail,
        /// <summary>
        /// The view to edit the flight information
        /// </summary>
        FlightInfoEdit,
        /// <summary>
        /// The Ticket Flight to enter Detail
        /// </summary>
        TicketSoldOrBooked,
        /// <summary>
        /// The Ticket Sold and Booked to enter Button
        /// </summary>
        BookDetail,
        /// <summary>
        /// The reserve pay view
        /// </summary>
        BookPay,
        /// <summary>
        /// The ticket sell view
        /// </summary>
        SellPay,
        /// <summary>
        /// The view to get ticket paid
        /// </summary>
        TicketInfoFilling,
        /// <summary>
        /// The view shows detailed ticket info
        /// </summary>
        ChangeTicket,
        /// <summary>
        /// The view to change ticket
        /// </summary>
        Sell,
        /// <summary>
        /// The flight schedule view
        /// </summary>
        Schedule,
        /// <summary>
        /// The report view
        /// </summary>
        Report,
        /// <summary>
        /// The setting view
        /// </summary>
        Setting,
        /// <summary>
        /// The roles settings view
        /// </summary>
        Roles
    }
}
