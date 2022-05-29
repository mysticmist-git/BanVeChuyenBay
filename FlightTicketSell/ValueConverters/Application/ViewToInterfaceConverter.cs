using FlightTicketSell.Interface;
using System;
using System.Globalization;

namespace FlightTicketSell.ValueConverters
{
    /// <summary>
    /// Indicates the view interface
    /// </summary>
    public enum ViewInterface
    {
        TicketInfoFilling,
        Report,
        BookDetail,
        CustomerDetail_Search
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
                case ViewInterface.BookDetail:
                    return value as IBookDetail;
                case ViewInterface.CustomerDetail_Search:
                    return value as ICustomerDetail;
            }

            return null;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
