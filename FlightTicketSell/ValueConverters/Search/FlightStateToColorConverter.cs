using FlightTicketSell.Models.Enums;
using System;
using System.Globalization;
using System.Windows.Media;

namespace FlightTicketSell.ValueConverters
{
    public class FlightStateToColorConverter : BaseValueConverter<FlightStateToColorConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case true:
                    return Brushes.Crimson;
                case false:
                    return Brushes.SpringGreen;
                default:
                    return Brushes.DeepPink;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
