using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FlightTicketSell.ValueConverters
{
    public class BorderExpand : BaseValueConverter<BorderExpand>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new GridLength((double)value + 10);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
