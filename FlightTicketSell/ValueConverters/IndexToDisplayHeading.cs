using System;
using System.Globalization;

namespace FlightTicketSell.ValueConverters
{
    public class IndexToDisplayHeading : BaseValueConverter<IndexToDisplayHeading>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string index = value.ToString();

            index = index.Insert(0, "00");
            index = index.Substring(index.Length - 2, 2);

            return "Khách hàng " + index;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
