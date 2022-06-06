using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightTicketSell.ValueConverters
{
    public class StatusTicketClass_BooleanToString : BaseValueConverter<StatusTicketClass_BooleanToString>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var boolean = System.Convert.ToBoolean(value);
            switch (boolean)
            {
                case true: return "Có sẵn";
                default:
                    return "Ngừng cung cấp";
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
