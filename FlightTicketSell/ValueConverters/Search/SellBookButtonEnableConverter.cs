using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FlightTicketSell.ValueConverters
{
    /// <summary>
    /// Return true only if all conditon is true
    /// </summary>
    public class AndLogicConverter : BaseMultiValueConverter<AndLogicConverter>
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            foreach (var condition in values)
            {
                if (System.Convert.ToBoolean(condition) == false)
                    return false;
            }

            return true;
        }

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
