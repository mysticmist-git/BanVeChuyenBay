using FlightTicketSell.Models.Enums;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace FlightTicketSell.Interface
{
    public interface IReport
    {
        Task OpenPrintPreview();
    }
}
