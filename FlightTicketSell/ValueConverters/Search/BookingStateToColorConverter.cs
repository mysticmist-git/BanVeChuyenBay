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
            switch (value)
            {
                case BookingState.NotChanged:
                    return Brushes.Crimson;
                case BookingState.Changed:
                    return Brushes.SpringGreen;
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
