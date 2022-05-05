using System;
using System.Globalization;

namespace FlightTicketSell.ValueConverters
{
    public class StringToIntConverter : BaseValueConverter<StringToIntConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((int)value).ToString();
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToInt32(value as string);
        }
    }
}
