namespace FlightTicketSell.Models.Roles
{
    public class UserGroupWithCount : UserGroup
    {
        /// <summary>
        /// The number of user in this user group
        /// </summary>
        public int UserCount { get; set; }
    }
}
