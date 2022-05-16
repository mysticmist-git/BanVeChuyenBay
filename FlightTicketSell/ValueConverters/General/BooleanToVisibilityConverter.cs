using FlightTicketSell.Models.Enums;
using System;
using System.Globalization;
using System.Windows;

namespace FlightTicketSell.ValueConverters
{
    /// <summary>
    /// Receive a bool and inverse it
    /// </summary>
    public class BooleanToVisibilityConverter : BaseValueConverter<BooleanToVisibilityConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null) return null;
            
            bool booleanValue = System.Convert.ToBoolean(value);

            if (parameter != null)
            {
                var direction = (Parameter)Enum.Parse(typeof(Parameter), (string)parameter);

                if (direction == Parameter.Inverted)
                    booleanValue = !booleanValue; 
            }

            return booleanValue ? Visibility.Visible : Visibility.Hidden;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
