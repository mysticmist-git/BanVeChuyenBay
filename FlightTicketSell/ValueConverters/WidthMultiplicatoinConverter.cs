using System;
using System.Globalization;

namespace FlightTicketSell.ValueConverters
{
    public class WidthMultiplicatoinConverter : BaseValueConverter<WidthMultiplicatoinConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value * (double)parameter;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
