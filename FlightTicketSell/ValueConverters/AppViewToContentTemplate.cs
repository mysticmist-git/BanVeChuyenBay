using FlightTicketSell.Models;
using FlightTicketSell.Views;
using FlightTicketSell.Views.SearchViewMore;
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
                case AppView.FlightTicket:
                    return new DescriptionTicketFlight();
                // Customer
                case AppView.Customer:
                    return new DescriptionCustomer();
                //TicketsoldBooked
                case AppView.TickedSoldBooked:
                    return new TicketSoldBooked();
                // Book related
                case AppView.Book:
                    return new BookView();
                case AppView.BookDetail:
                    return new BookDetailView();
                case AppView.ReservePay:
                    return new BookPayView();
                // Sell related
                case AppView.Sell:
                    return new SellView();
                case AppView.SellPay:
                    return new SellPayView();
                case AppView.TicketInfo:
                    return new Views.SellViewRelated.TicketInfoView();
                case AppView.ChangeTicket:
                    return new Views.SellViewRelated.ChangeTicketView();
                // Schedule
                case AppView.Schedule:
                    return new ScheduleView();
                // Report realted
                case AppView.Report:
                    return new ReportView();
                // Settings
                case AppView.Setting:
                    return new SettingView();
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
