namespace FlightTicketSell.Models
{

    /// <summary>
    /// The user group with some boolean indicates what they permission they granted
    /// </summary>
    public class UserGroupModified : UserGroup
        {
            #region Private Members

            /// <summary>
            /// Indicates if this user group can search for flights
            /// </summary>
            private bool _canSearchFlight { get; set; }

            /// <summary>
            /// Indicates if this user group can search for flights
            /// </summary>
            private bool _canEditFlight { get; set; }

            /// <summary>
            /// Indicates if this user group can schedule flights
            /// </summary>
            private bool _canScheduleFlight { get; set; }

            /// <summary>
            /// Indicates if this user group can view reports
            /// </summary>
            private bool _canViewReport { get; set; }

            /// <summary>
            /// Indicates if this user group can change settings
            /// </summary>
            private bool _canSettings { get; set; }

            /// <summary>
            /// Indicates if this user group can manage users
            /// </summary>
            private bool _canManageUser { get; set; }

            #endregion

            #region Public Properties

            /// <summary>
            /// Indicates if this user group is modified
            /// </summary>
            public bool IsPermissionChanged { get; set; }

            /// <summary>
            /// Indicates if this user group can search for flights
            /// </summary>
            public bool CanSearchFlight
            {
                get
                {
                    return _canSearchFlight;
                }
                set
                {
                    _canSearchFlight = value;
                    IsPermissionChanged = true;
                }
            }

            /// <summary>
            /// Indicates if this user group can search for flights
            /// </summary>
            public bool CanEditFlight
            {
                get
                {
                    return _canEditFlight;
                }
                set
                {
                    _canEditFlight = value;
                    IsPermissionChanged = true;
                }
            }


            /// <summary>
            /// Indicates if this user group can schedule flights
            /// </summary>
            public bool CanScheduleFlight
            {
                get
                {
                    return _canScheduleFlight;
                }
                set
                {
                    _canScheduleFlight = value;
                    IsPermissionChanged = true;
                }
            }


            /// <summary>
            /// Indicates if this user group can view reports
            /// </summary>
            public bool CanViewReport
            {
                get
                {
                    return _canViewReport;
                }
                set
                {
                    _canViewReport = value;
                    IsPermissionChanged = true;
                }
            }


            /// <summary>
            /// Indicates if this user group can change settings
            /// </summary>
            public bool CanSettings
            {
                get
                {
                    return _canSettings;
                }
                set
                {
                    _canSettings = value;
                    IsPermissionChanged = true;
                }
            }


            /// <summary>
            /// Indicates if this user group can manage users
            /// </summary>
            public bool CanManageUser
            {
                get
                {
                    return _canManageUser;
                }
                set
                {
                    _canManageUser = value;
                    IsPermissionChanged = true;
                }
            }

            #endregion    
        }
}
