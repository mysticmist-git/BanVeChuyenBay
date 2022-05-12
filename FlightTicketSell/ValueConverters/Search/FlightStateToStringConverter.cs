using FlightTicketSell.Models.Enums;
using System;
using System.Globalization;

namespace FlightTicketSell.ValueConverters
{
    public class FlightStateToStringConverter : BaseValueConverter<FlightStateToStringConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case true:
                    return "Đã khởi hành";
                case false:
                    return "Chưa khởi hành";
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
