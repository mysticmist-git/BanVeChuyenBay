using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FlightTicketSell.ValueConverters
{
    public class StringAirfaresToIntAirfares : BaseValueConverter<StringAirfaresToIntAirfares>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((int)value == 0)
                return null;

            return ((int)value).ToString();
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return 0;

            return int.Parse((string)value);
        }
    }
}
