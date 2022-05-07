using FlightTicketSell.Models.Enums;
using System;
using System.Globalization;

namespace FlightTicketSell.ValueConverters
{
    public class BookingStateToStringConverter : BaseValueConverter<BookingStateToStringConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case BookingState.NotChanged:
                    return "Chưa đổi";
                case BookingState.Changed:
                    return "Đã đổi";
                case BookingState.Cancel:
                    return "Đã hủy";
                default:
                    return "Lỗi";
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
