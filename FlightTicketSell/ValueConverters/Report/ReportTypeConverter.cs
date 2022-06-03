using FlightTicketSell.Models.Enums;
using FlightTicketSell.Models.Report;
using FlightTicketSell.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace FlightTicketSell.ValueConverters
{
    public class ReportTypeConverter : BaseValueConverter<ReportTypeConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
                return null;
            
            // TODO: Rất tà đạo, sửa nếu được.
            switch(IoC.IoC.Get<ReportViewModel>().CurrentReportType)
            {
                case ReportType.FlightsOfOneMonth:
                    return (value as ObservableCollection<object>).Cast<FlightReport>();
                case ReportType.MonthsOfOneYear:
                    return (value as ObservableCollection<object>).Cast<MonthReport>();
                case ReportType.OneMonthOfAllYears:
                case ReportType.AllYears:
                    return (value as ObservableCollection<object>).Cast<YearReport>();
                default:
                    return null;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
