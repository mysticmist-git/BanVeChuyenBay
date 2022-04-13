namespace FlightTicketSell.Models
{
    public class DataProvider
    {
        #region Private Members 

        private static DataProvider _ins;

        #endregion

        #region Public Properties

        public static DataProvider Ins
        {
            get
            {
                if (_ins == null)
                    _ins = new DataProvider();
                return _ins;
            }
            set
            {
                _ins = value;
            }
        }

        public FlightTicketSellEntities DB { get; set; }

        #endregion

        #region Constructor

        private DataProvider()
        {
            DB = new FlightTicketSellEntities();
        }

        #endregion
    }
}
