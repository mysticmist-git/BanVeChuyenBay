using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FlightTicketSell.ValueConverters
{
    public class StringMonthYearToIntConverter : BaseValueConverter<StringMonthYearToIntConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            if ((int)value == 0)
                return new ComboBoxItem { Content = "Tất cả" };

            return new ComboBoxItem { Content = ((int)value).ToString() };
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            // Get combobox item value
            string stringValue = (value as ComboBoxItem).Content.ToString();

            if (stringValue == "Tất cả")
                return 0;

            return int.Parse((string)stringValue);
        }
    }
}
