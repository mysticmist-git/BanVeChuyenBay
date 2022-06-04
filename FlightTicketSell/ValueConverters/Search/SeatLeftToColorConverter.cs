using System;
using System.Globalization;
using System.Windows.Media;

namespace FlightTicketSell.ValueConverters
{
    public class SeatLeftToColorConverter : BaseValueConverter<SeatLeftToColorConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var converter = new BrushConverter();

            if (value is null)
                return (System.Windows.Media.Brush)converter.ConvertFromString("#0003ED");

            int seatLeft;
            var result = Int32.TryParse(value.ToString(), out seatLeft);

            // Error case
            if (!result)
                return (System.Windows.Media.Brush)converter.ConvertFromString("#0003ED");

            // Success Case
            if (seatLeft <= 0)
                return (System.Windows.Media.Brush)converter.ConvertFromString("#C7282A");
            else
                return (System.Windows.Media.Brush)converter.ConvertFromString("#000000");
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
