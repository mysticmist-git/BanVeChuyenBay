namespace FlightTicketSell.ViewModels
{
    public class ViewModelLocator
    {
        public static ViewModelLocator Instance { get; private set; } = new ViewModelLocator();

        public static ApplicationViewModel ApplicationViewModel => IoC.IoC.Get<ApplicationViewModel>();
        public static ReportViewModel ReportViewModel => IoC.IoC.Get<ReportViewModel>();
        public static BookViewModel BookViewModel => IoC.IoC.Get<BookViewModel>();
        public static BookDetailViewModel BookDetailViewModel => IoC.IoC.Get<BookDetailViewModel>();
    }
}
