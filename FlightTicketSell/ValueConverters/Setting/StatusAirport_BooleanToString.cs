using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightTicketSell.ValueConverters
{
    public class StatusAirport_BooleanToString : BaseValueConverter<StatusAirport_BooleanToString>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var boolean = System.Convert.ToBoolean(value);
            switch (boolean)
            {
                case true: return "Đang hoạt động";
                default:
                    return "Ngừng hoạt động";
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
