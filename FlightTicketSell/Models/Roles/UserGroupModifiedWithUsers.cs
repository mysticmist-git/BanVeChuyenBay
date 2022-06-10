using System.Collections.ObjectModel;

namespace FlightTicketSell.Models
{
    /// <summary>
    /// User group with a user list
    /// </summary>
    public class UserGroupModifiedWithUsers : UserGroupModified
        {
            #region Public Properties

            /// <summary>
            /// The number of user in this user group
            /// </summary>
            public int UserCount { get; set; }

            /// <summary>
            /// The user list of this user group
            /// </summary>
            public ObservableCollection<User> Users { get; set; }

            #endregion    
        }
}
