using FlightTicketSell.Models;

namespace FlightTicketSell.ViewModels
{
    public class ApplicationViewModel : BaseViewModel
    {
        /// <summary>
        /// The current view of the application
        /// </summary>
        public AppView CurrentView { get; set; } = AppView.Search;
    }
}
