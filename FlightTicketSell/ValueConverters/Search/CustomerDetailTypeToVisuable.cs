using FlightTicketSell.Models;
using FlightTicketSell.Models.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FlightTicketSell.ValueConverters
{
    public class CustomerDetailTypeToVisuable : BaseValueConverter<CustomerDetailTypeToVisuable>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (CustomerWithIndex.CustomerDetailType)
            {
                case CustomerDetailType.Reservation:
                    return Visibility.Visible;
                default:
                    return Visibility.Hidden;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
