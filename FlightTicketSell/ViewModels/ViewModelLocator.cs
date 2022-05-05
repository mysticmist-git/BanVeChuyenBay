using FlightTicketSell.ViewModels.Search;

namespace FlightTicketSell.ViewModels
{
    public class ViewModelLocator
    {
        public static ViewModelLocator Instance { get; private set; } = new ViewModelLocator();

        public static ApplicationViewModel ApplicationViewModel => IoC.IoC.Get<ApplicationViewModel>();
        public static ReportViewModel ReportViewModel => IoC.IoC.Get<ReportViewModel>();
        public static BookViewModel BookViewModel => IoC.IoC.Get<BookViewModel>();
        public static BookDetailViewModel BookDetailViewModel => IoC.IoC.Get<BookDetailViewModel>();

        public static FlightTicket_Search FlightTicket_Search => IoC.IoC.Get<FlightTicket_Search>();
        public static TickedSoldBooked_Search TickedSoldBooked_Search => IoC.IoC.Get<TickedSoldBooked_Search>();
        public static Customer_Search Customer_Search => IoC.IoC.Get<Customer_Search>();
    }
}
