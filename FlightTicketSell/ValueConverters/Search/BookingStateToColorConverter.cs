using FlightTicketSell.Models.Enums;
using System;
using System.Globalization;
using System.Windows.Media;

namespace FlightTicketSell.ValueConverters
{
    public class BookingStateToColorConverter : BaseValueConverter<BookingStateToColorConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var converter = new BrushConverter();
            switch (value)
            {
                case BookingState.NotChanged:
                    return (System.Windows.Media.Brush)converter.ConvertFromString("#C7282A");
                case BookingState.Changed:
                    return (System.Windows.Media.Brush)converter.ConvertFromString("#24B44C");
                case BookingState.Cancel:
                    return Brushes.DarkGray;
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
