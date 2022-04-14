using System;
using System.Globalization;
using System.Windows.Controls;

namespace FlightTicketSell.ValueConverters
{
    public class WidthToOrientationConverter : BaseValueConverter<WidthToOrientationConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (System.Convert.ToInt32(value) < System.Convert.ToInt32(parameter))
                return Orientation.Vertical;
            return Orientation.Horizontal;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
