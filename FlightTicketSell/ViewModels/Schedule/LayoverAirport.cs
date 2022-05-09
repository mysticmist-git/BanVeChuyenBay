using FlightTicketSell.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FlightTicketSell.ViewModels.Schedule
{
    public class LayoverAirport : BaseViewModel
    {

        #region Private Members

        #endregion

        #region Public Properties
        public int Id { get; set; }
        public int Id_Route { get; set; }
        public int Id_Airport { get; set; }
        public string AirportName { get; set; }
        public int Order { get; set; }
        public int StopTime { get; set; }
        public string Note { get; set; }
        #endregion

        #region Methods
        #endregion
    }
}
