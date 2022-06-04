using FlightTicketSell.ViewModels;

namespace FlightTicketSell.ViewModels
{
    public class ViewModelLocator
    {
        public static ViewModelLocator Instance { get; private set; } = new ViewModelLocator();

        public static ApplicationViewModel ApplicationViewModel => IoC.IoC.Get<ApplicationViewModel>();
        public static ReportViewModel ReportViewModel => IoC.IoC.Get<ReportViewModel>();
        public static BookViewModel BookViewModel => IoC.IoC.Get<BookViewModel>();
        public static BookDetailViewModel BookDetailViewModel => IoC.IoC.Get<BookDetailViewModel>();
        public static ChangeTicketViewModel ChangeTicketViewModel => IoC.IoC.Get<ChangeTicketViewModel>();
        public static SellViewModel SellViewModel => IoC.IoC.Get<SellViewModel>();
        public static SellPayViewModel SellPayViewModel => IoC.IoC.Get<SellPayViewModel>();
        public static SearchViewModel SearchViewModel => IoC.IoC.Get<SearchViewModel>();
        public static ScheduleViewModel ScheduleViewModel  => IoC.IoC.Get<ScheduleViewModel>();  
        public static FlightInfoEditViewModel FlightInfoEditViewModel => IoC.IoC.Get<FlightInfoEditViewModel>();  
        public static TicketInfoFillingViewModel TicketInfoFillingViewModel => IoC.IoC.Get<TicketInfoFillingViewModel>();
        public static FlightDetailViewModel FlightTicket_Search => IoC.IoC.Get<FlightDetailViewModel>();
        public static FlightTicketAndBookViewModel FlightTicketAndBookViewModel => IoC.IoC.Get<FlightTicketAndBookViewModel>();
    }
}
