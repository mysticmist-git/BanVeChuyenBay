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
                case 2:
                    return "Đã khởi hành";
                case 1:
                    return "Chưa khởi hành";
                case 3:
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
