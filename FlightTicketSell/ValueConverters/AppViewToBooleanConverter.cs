using FlightTicketSell.Models;
using System;
using System.Globalization;

namespace FlightTicketSell.ValueConverters
{
    public class AppViewToBooleanConverter : BaseValueConverter<AppViewToBooleanConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (AppView)value == (AppView)parameter;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
