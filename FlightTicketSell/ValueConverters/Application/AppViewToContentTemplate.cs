using FlightTicketSell.Models;
using FlightTicketSell.Views;
using System;
using System.Globalization;

namespace FlightTicketSell.ValueConverters
{
    public class AppViewToContentTemplate : BaseValueConverter<AppViewToContentTemplate>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Convert value to AppView
            var appView = (AppView)value;

            // Return approriate content template
            switch (appView)
            {
                // Search
                case AppView.Search:
                    return new SearchView();
                // Ticket Flight
                case AppView.FlightDetail:
                    return new DescriptionTicketFlight();
                // Flight info edit
                case AppView.FlightInfoEdit:
                    return new FlightInfoEditView();
                //TicketsoldBooked
                case AppView.TicketSoldOrBooked:
                    return new FlightTicketAndBookView();
                // Book related
                case AppView.Book:
                    return new BookView();
                case AppView.BookDetail:
                    return new BookDetailView();
                case AppView.BookPay:
                    return new BookPayView();
                // Sell related
                case AppView.Sell:
                    return new SellView();
                case AppView.SellPay:
                    return new SellPayView();
                case AppView.TicketInfoFilling:
                    return new Views.TicketInfoFillingView();
                case AppView.ChangeTicket:
                    return new Views.ChangeTicketView();
                // Schedule
                case AppView.Schedule:
                    return new ScheduleView();
                // Report realted
                case AppView.Report:
                    return new ReportView();
                // Settings
                case AppView.Setting:
                    return new SettingView();
                // Roles
                case AppView.Roles:
                    return new RolesView();
                // Default
                default:
                    return new SearchView();
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
