using FlightTicketSell.Models.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightTicketSell.ViewModels
{
    public class Report<T>
    {
        /// <summary>
        /// Content of the report
        /// </summary>
        public ObservableCollection<T> Content { get; set; }

        /// <summary>
        /// Type of the report
        /// </summary>
        public ReportType Type { get; set; }

    }
}
