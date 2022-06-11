using FlightTicketSell.Models.Enums;
using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Media;

namespace FlightTicketSell.ValueConverters
{
    public class FlightStateToColorConverter : BaseValueConverter<FlightStateToColorConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var converter = new BrushConverter();
            switch (value)
            {
                case 2:
                    return (System.Windows.Media.Brush)converter.ConvertFromString("#C7282A");
                case 1:
                    return (System.Windows.Media.Brush)converter.ConvertFromString("#24B44C");
                case 3:
                    return (System.Windows.Media.Brush)converter.ConvertFromString("#06b4f4");
                default:
                    return System.Drawing.Brushes.DeepPink;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
