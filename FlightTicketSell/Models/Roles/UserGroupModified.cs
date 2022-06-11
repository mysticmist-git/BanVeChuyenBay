namespace FlightTicketSell.Models
{

    /// <summary>
    /// The user group with some boolean indicates what they permission they granted
    /// </summary>
    public class UserGroupModified : UserGroup
    {

        #region Public Properties

        /// <summary>
        /// Indicates if this user group can search for flights
        /// </summary>
        public bool CanSearchFlight { get; set; }

        /// <summary>
        /// Indicates if this user group can search for flights
        /// </summary>
        public bool CanEditFlight { get; set; }


        /// <summary>
        /// Indicates if this user group can schedule flights
        /// </summary>
        public bool CanScheduleFlight { get; set; }


        /// <summary>
        /// Indicates if this user group can view reports
        /// </summary>
        public bool CanViewReport { get; set; }


        /// <summary>
        /// Indicates if this user group can change settings
        /// </summary>
        public bool CanSettings { get; set; }


        /// <summary>
        /// Indicates if this user group can manage users
        /// </summary>
        public bool CanManageUser { get; set; }

        #endregion
    }
}
