using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace FlightTicketSell.ValueConverters
{
    public class StatusAirport_BooleanToColor : BaseValueConverter<StatusAirport_BooleanToColor>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var boolean = System.Convert.ToBoolean(value);
            var converter = new BrushConverter();
            switch (boolean)
            {
                case true: return (System.Windows.Media.Brush)converter.ConvertFromString("#24B44C");
                default:
                    return (System.Windows.Media.Brush)converter.ConvertFromString("#C7282A");
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
