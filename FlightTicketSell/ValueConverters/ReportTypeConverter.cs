using FlightTicketSell.Models.Enums;
using FlightTicketSell.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightTicketSell.ValueConverters
{
    public class ReportTypeConverter : BaseValueConverter<ReportTypeConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // If value and parameter is null, return null
            if (value == null)
                return null;

            // Get report type
            var reportType = (value as Report<object>).Type;


            // If report type is FlightReport
            if (reportType is ReportType.FlightsOfOneMonth)
            {
                // Create an empty collection
                var collection = new ObservableCollection<FlightReport>();

                // For each item in input report
                foreach (var item in (value as Report<object>).Content)
                {
                    // Cast it
                    var castedItem = item as FlightReport;

                    // Add it to collection
                    collection.Add(castedItem);
                }

                return collection;
            }

            // If report type is MonthReport
            if (reportType is ReportType.MonthsOfOneYear)
            {
                // Create an empty collection
                var collection = new ObservableCollection<MonthReport>();

                // For each item in input report
                foreach (var item in (value as Report<object>).Content)
                {
                    // Cast it
                    var castedItem = item as MonthReport;

                    // Add it to collection
                    collection.Add(castedItem);
                }

                return collection;
            }

            // If report type is YearReport
            if (reportType is ReportType.OneMonthOfAllYears || reportType is ReportType.AllYears)
            {
                // Create an empty collection
                var collection = new ObservableCollection<YearReport>();

                // For each item in input report
                foreach (var item in (value as Report<object>).Content)
                {
                    // Cast it
                    var castedItem = item as YearReport;

                    // Add it to collection
                    collection.Add(castedItem);
                }

                return collection;
            }

            return null;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
