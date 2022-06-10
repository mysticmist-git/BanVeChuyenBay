using System;
using System.Globalization;
using System.Windows;

namespace FlightTicketSell.ValueConverters
{
    public class PermissionToVisibilityConverter : BaseValueConverter<PermissionToVisibilityConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (System.Convert.ToBoolean(value))
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
