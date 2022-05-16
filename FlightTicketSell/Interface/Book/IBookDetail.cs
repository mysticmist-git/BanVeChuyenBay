namespace FlightTicketSell.Interface
{
    public interface IBookDetail
    {
        /// <summary>
        /// Indicates if all field filled
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// Force all field update for validating
        /// </summary>
        void ForceUpdateSource();
    }
}
