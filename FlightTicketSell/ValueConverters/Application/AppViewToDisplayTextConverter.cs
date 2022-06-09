using FlightTicketSell.Models;
using System;
using System.Globalization;

namespace FlightTicketSell.ValueConverters
{
    public class AppViewToDisplayTextConverter : BaseValueConverter<AppViewToDisplayTextConverter>
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
                    return "TRA CỨU";
                // Ticket Flight
                case AppView.FlightDetail:
                    return @"...\CHI TIẾT CHUYẾN BAY";
                case AppView.FlightInfoEdit:
                    return @"...\SỬA CHUYẾN BAY";
                //TicketsoldBooked
                case AppView.TicketSoldOrBooked:
                    return @"...\VÉ ĐÃ ĐẶT & BÁN";
                // Book related
                case AppView.Book:
                    return @"...\ĐẶT CHỖ";
                case AppView.BookDetail:
                    return @"...\CHI TIẾT ĐẶT CHỖ";
                case AppView.BookPay:
                    return @"...\THANH TOÁN ĐẶT CHỖ";
                // Sell related
                case AppView.Sell:
                    return @"...\MUA VÉ";
                case AppView.SellPay:
                    return @"...\THANH TOÁN VÉ";
                case AppView.TicketInfoFilling:
                    return @"...\ĐIỀN THÔNG TIN VÉ";
                case AppView.ChangeTicket:
                    return @"...\ĐỔI VÉ";
                // Schedule
                case AppView.Schedule:
                    return "NHẬN LỊCH";
                // Report realted
                case AppView.Report:
                    return "BÁO CÁO DOANH THU";
                // Settings
                case AppView.Setting:
                    return "CÀI ĐẶT";
                case AppView.Roles:
                    return "PHÂN QUYỀN";
                // Default
                default:
                    return "TRA CỨU";
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
