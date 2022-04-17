using FlightTicketSell.Models.Enums;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Input;

namespace FlightTicketSell.ViewModels
{
    public class ReportPrintViewModel : BaseViewModel
    {
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
                        return string.Format($"Trong tháng {reportVM.Month}, năm {reportVM.Year}");
                    case ReportType.MonthsOfOneYear:
                        return string.Format($"Trong năm {reportVM.Year}");
                    case ReportType.OneMonthOfAllYears:
                        return
                            reportVM.Years[1] == reportVM.Years[reportVM.Years.Count - 1] ?
                            string.Format($"Trong tháng {reportVM.Month} của năm {reportVM.Years[1]}"):
                            string.Format($"Trong tháng {reportVM.Month} từ năm {reportVM.Years[1]} tới năm {reportVM.Years[reportVM.Years.Count - 1]}");
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

        #region Commands

        /// <summary>
        /// Return to the report view
        /// </summary>
        public ICommand ReturnCommand { get; set; }

        /// <summary>
        /// The command executed when user control load
        /// </summary>
        public ICommand LoadCommand { get; set; }

        /// <summary>
        /// Export PDF
        /// </summary>
        public ICommand PrintCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ReportPrintViewModel()
        {
            // Create commands
            ReturnCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.Report );

            // TODO: rất tà đạo, nên sửa nếu có thời gian
            LoadCommand = new RelayCommand<object>((p) => true, (p) =>  IoC.IoC.Get<ReportViewModel>().LoadCommand.Execute(null) );

            // TODO: Implemented this later
            PrintCommand = new RelayCommand<object>((p) => true, (p) => MessageBox.Show("Xuất PDF", "In báo cáo", MessageBoxButton.OK, MessageBoxImage.Information) );
        }

        #endregion
    }
}
