using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightTicketSell.ViewModels.Setting
{
    public class Airport: BaseViewModel
    {
        #region Private Members

        #endregion

        #region Public Properties
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Province { get; set; }
        public bool Status { get; set; }
        #endregion

        #region Methods
        public Airport() { }
        public Airport(Airport airport)
        {
            Id= airport.Id;
            Name= airport.Name;
            Code= airport.Code;    
            Province= airport.Province;
            Status= airport.Status;
        }
        #endregion
    }
}
