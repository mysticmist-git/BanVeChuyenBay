using FlightTicketSell.Interface.Report;
using FlightTicketSell.Models.Enums;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FlightTicketSell.ViewModels
{
    public class ReportPrintViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// The type of the current report
        /// </summary>
        public ReportType CurrentReportType { get => IoC.IoC.Get<ReportViewModel>().CurrentReportType; }

        /// <summary>
        /// The report
        /// </summary>
        public ObservableCollection<object> Report { get => IoC.IoC.Get<ReportViewModel>().Report; }

        /// <summary>
        /// The text that indicate which period of time this report takes
        /// </summary>
        public string PrintPeriod
        {
            get
            {
                // Get report view model
                var reportVM = IoC.IoC.Get<ReportViewModel>();

                switch (reportVM.CurrentReportType)
                {
                    case ReportType.FlightsOfOneMonth:
                        return string.Format($"Trong tháng {reportVM.CurrentMonth}, năm {reportVM.CurrentYear}");
                    case ReportType.MonthsOfOneYear:
                        return string.Format($"Trong năm {reportVM.CurrentYear}");
                    case ReportType.OneMonthOfAllYears:
                        return
                            reportVM.Years[1] == reportVM.Years[reportVM.Years.Count - 1] ?
                            string.Format($"Trong tháng {reportVM.CurrentMonth} của năm {reportVM.Years[1]}") :
                            string.Format($"Trong tháng {reportVM.CurrentMonth} từ năm {reportVM.Years[1]} tới năm {reportVM.Years[reportVM.Years.Count - 1]}");
                    case ReportType.AllYears:
                        return
                            reportVM.Years[1] == reportVM.Years[reportVM.Years.Count - 1] ?
                            string.Format($"Trong năm {reportVM.Years[1]}") :
                            string.Format($"Từ năm {reportVM.Years[1]} tới năm {reportVM.Years[reportVM.Years.Count - 1]}");
                    default:
                        return null;
                }
            }
            set { }
        }

        /// <summary>
        /// The print date, which return DateTime.Now in a formated form
        /// </summary>
        public string PrintDate { get => DateTime.Now.ToString("dddd, dd MMMM yyyy", new CultureInfo("vi-VN")); } 

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ReportPrintViewModel()
        {
        }

        #endregion
    }
}
