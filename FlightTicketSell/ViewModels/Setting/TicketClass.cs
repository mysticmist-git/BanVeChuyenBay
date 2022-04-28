using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightTicketSell.ViewModels.Setting
{
    public class TicketClass : BaseViewModel
    {
        #region Private Members

        #endregion

        #region Public Properties
        public int Id { get; set; }
        public string Name { get; set; }
        public double Coefficient { get; set; }
        public bool Status { get; set; }
        #endregion

        #region Methods

        #endregion
    }
}
