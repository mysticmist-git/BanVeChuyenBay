using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightTicketSell.ViewModels.Schedule
{
    public class TicketClassDetail : BaseViewModel
    {
        #region Private Members

        #endregion

        #region Public Properties
        public int Id { get; set; }
        public int Id_TicketClass { get; set; }
        public int Id_Flight { get; set; }
        public string TicketClassName { get; set; }
        public double TicketClassCoefficient { get; set; }
        public double Prices { get; set; }
        public int Seats { get; set; }
        #endregion

        #region Methods
        #endregion
    }
}
