using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FlightTicketSell.ValueConverters
{
    // TODO: Delete this file if you don't use it in the future
    public class StringMonthYearToIntConverter : BaseValueConverter<StringMonthYearToIntConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            if (value.ToString() == "Tất cả")
                return 0;

            return int.Parse((string)value.ToString());
        }
    }
}
