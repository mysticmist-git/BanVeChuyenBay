using System;
using System.Globalization;

namespace FlightTicketSell.ValueConverters
{
    public class CustomerNameDisplayConverter : BaseValueConverter<CustomerNameDisplayConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null || string.IsNullOrEmpty(value.ToString()))
                return "Tên khách hàng";
            else
                return value.ToString();
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
