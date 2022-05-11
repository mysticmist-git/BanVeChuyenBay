using FlightTicketSell.Interface;
using FlightTicketSell.Interface.Report;
using System;
using System.Globalization;

namespace FlightTicketSell.ValueConverters
{
    public enum ViewInterface
    {
        TicketInfoFilling,
        Report
    }
    
    public class ViewToInterfaceConverter : BaseValueConverter<ViewToInterfaceConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (parameter as ViewInterface?)
            {
                case ViewInterface.TicketInfoFilling:
                    return value as ITicketInfoFilling;
                case ViewInterface.Report:
                    return value as IReport;
            }

            return null;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
