using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FlightTicketSell.ValueConverters
{
    public class DatePickerUI_To_DatePickerConverter : BaseValueConverter<DatePickerUI_To_DatePickerConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            return (DatePicker)value;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            return (DatePicker)value;
        }
    }
}
