using FlightTicketSell.ViewModels;

namespace FlightTicketSell.Models.SearchRelated
{
    public class Airport_Search : BaseViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Province { get; set; }
        public bool Status { get; set; }
    }
}
