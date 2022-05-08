namespace FlightTicketSell.Models.Enums
{
    public enum BookingState
    {
        /// <summary>
        /// For unexpected state
        /// </summary>
        None,
        /// <summary>
        /// The booking has been changed for a ticket
        /// </summary>
        Changed,
        /// <summary>
        /// THe booking hasn't been changed for a ticket
        /// </summary>
        NotChanged,
        /// <summary>
        /// The booking has been canceled
        /// </summary>
        Cancel
    }
}
