using FlightTicketSell.Models;
using FlightTicketSell.ViewModels.Report;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Linq;
using System.Windows.Input;
using System;
using FlightTicketSell.Models.Enums;

namespace FlightTicketSell.ViewModels
{
    public class ReportViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// Month condition, with 0 mean every month
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// Year condition, with 0 mean every year
        /// </summary>
        public int Year { get; set; }
        
        /// <summary>
        /// Total revenue
        /// </summary>
        public string TotalRevenue 
        { 
            get
            {
                // Return null if Report is null
                if (Report == null)
                    return null;
                
                // Initialize total revenue
                var totalRevenue = 0;
                
                // Get report type
                var reportType = (Report as Report<object>).Type;
                

                // If report type is FlightReport
                if (reportType is ReportType.FlightReport)
                {
                    // Get report content
                    var collection = (Report as Report<object>).Content;

                    // For each item in input report
                    foreach (var item in collection)
                        totalRevenue += int.Parse((item as FlightReport).Revenue);

                    return totalRevenue.ToString();
                }

                // If report type is MonthReport
                if (reportType is ReportType.MonthReport)
                {
                    // Get report content
                    var collection = (Report as Report<object>).Content;

                    // For each item in input report
                    foreach (var item in collection)
                        totalRevenue += int.Parse((item as MonthReport).Revenue);

                    return totalRevenue.ToString();
                }

                // If report type is YearReport
                if (reportType is ReportType.YearReport)
                {
                    // Get report content
                    var collection = (Report as Report<object>).Content;

                    // For each item in input report
                    foreach (var item in collection)
                        totalRevenue += int.Parse((item as YearReport).Revenue);

                    return totalRevenue.ToString();
                }

                // Return null at default
                return null;
            }
            set { }
        }

        /// <summary>
        ///  Indicate current report type
        /// </summary>
        public ReportType CurrentReportType { get; set; }

        /// <summary>
        /// The flight revenue report
        /// </summary>
        public Report<object> Report { get; set; }

        #endregion

        #region Commands
        
        /// <summary>
        /// Execute when view load
        /// </summary>
        public ICommand LoadCommand { get; set; }

        /// <summary>
        /// Command to query new report when month, years changes
        /// </summary>
        public ICommand MonthYearChangedCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ReportViewModel()
        {
            // Create commands
            LoadCommand = new RelayCommand<object>((p) => true, async (p) =>
            {
                // Check if any flight departed to add a report if needed
                // ----code----

                // Update report
                Report = await GetReportAsync();
            });

            MonthYearChangedCommand = new RelayCommand<object>((p) => true, async (p) =>
            {
                Report = await GetReportAsync();
            });
        }

        #endregion

        #region Helper Class

        /// <summary>
        /// Get report, take in month and year for different case
        /// </summary>
        /// <param name="month">The month of the report with 0 mean every month</param>
        /// <param name="year">The year of the report with 0 mean every year</param>
        /// <returns></returns>
        public async Task<Report<object>> GetReportAsync()
        {
            // Check Month, Year validation
            if (Month < 0 || Month > 12)
                return null;

            if (Year < 0)
                return null;

            // If query a specific Month
            if (Month != 0)
            {
                // A specific Year
                if (Year != 0)
                {
                    // Return a Month report of that Year
                    CurrentReportType = ReportType.FlightReport;
                    return new Report<object> { Content = await GetFlightReport(Month, Year), Type = CurrentReportType };
                }

                // Return a report of a Month of every Year
                CurrentReportType = ReportType.YearReport;
                return new Report<object> { Content = await GetAllYearReport(Month), Type = CurrentReportType };
            }

            // Query all Month

            // Query for a specific Year
            if (Year != 0)
            {
                CurrentReportType = ReportType.MonthReport;
                return new Report<object> { Content = await GetMonthReport(Year), Type = CurrentReportType };
            }

            // Query all Year
            CurrentReportType = ReportType.YearReport;
            return new Report<object> { Content = await GetAllYearReport(), Type = CurrentReportType };
        }

        /// <summary>
        /// Get the month report with specific month and year
        /// </summary>
        /// <param name="month">The month of the report</param>
        /// <param name="year">The year of the report</param>
        /// <returns></returns>
        public async Task<ObservableCollection<object>> GetFlightReport(int month, int year)
        {
            // Declare report that'll be return later
            var report = new ObservableCollection<object>();

            // Get report
            using (var context = new FlightTicketSellEntities())
            {
                // Get report from database
                var objectList = await context.DOANHTHUCHUYENBAYs
                    .Where(p => p.DOANHTHUTHANG.Thang==month && p.DOANHTHUTHANG.DOANHTHUNAM.Nam==year)
                    .ToListAsync();

                // Convert to apporiate model
                foreach (var item in objectList)
                {
                    report.Add(
                        new FlightReport
                        {
                            FlightCode = item.MaChuyenBay.ToString(),
                            DepartTime = item.CHUYENBAY.NgayGio.ToString(),
                            TicketSold = item.SoVe.ToString(),
                            Revenue = item.DoanhThu.ToString(),
                            Ratio = (item.TiLe * 100).ToString()
                        }
                    );
                }

            }

            return report;
        }

        /// <summary>
        /// Get the all year report with specific month
        /// </summary>
        /// <param name="month">The month of the report</param>
        /// <returns></returns>
        private async Task<ObservableCollection<object>> GetAllYearReport(int month)
        {
            // Declare report that'll be return later
            var report = new ObservableCollection<object>();

            // Get report
            using (var context = new FlightTicketSellEntities())
            {
                // Get report from database
                var objectList = await context.DOANHTHUTHANGs
                    .Where(p => p.Thang == month)
                    .ToListAsync();

                // Convert to apporiate model
                foreach (var item in objectList)
                {
                    report.Add(
                        new YearReport
                        {
                            Year = item.DOANHTHUNAM.Nam.ToString(),
                            FlightCount = item.SoChuyenBay.ToString(),
                            Revenue = item.DoanhThu.ToString(),
                            Ratio = (item.TiLe * 100).ToString()
                        }
                    );
                }
            }

            return report;
        }

        /// <summary>
        /// Get the year report with specific year
        /// </summary>
        /// <param name="year">The year of the report</param>
        /// <returns></returns>
        private async Task<ObservableCollection<object>> GetMonthReport(int year)
        {
            // Declare report that'll be return later
            var report = new ObservableCollection<object>();

            // Get report
            using (var context = new FlightTicketSellEntities())
            {
                // Get all month of that year
                var objectList = await context.DOANHTHUTHANGs
                    .Where(p => p.DOANHTHUNAM.Nam == year)
                    .ToListAsync();

                // Convert to apporiate model
                foreach (var item in objectList)
                {
                    report.Add(
                        new MonthReport
                        {
                            Month = item.Thang.ToString(),
                            Revenue = item.DoanhThu.ToString(),
                            Ratio = ((item.DoanhThu / item.DOANHTHUNAM.DoanhThu) * 100).ToString()
                        }
                    );
                }
            }

            return report;
        }

        /// <summary>
        /// Get the year report of every year
        /// </summary>
        /// <returns></returns>
        private async Task<ObservableCollection<object>> GetAllYearReport()
        {
            // Declare report that'll be return later
            var report = new ObservableCollection<object>();

            // Get report
            using (var context = new FlightTicketSellEntities())
            {
                // Get all year report from database
                var objectList = await context.DOANHTHUNAMs.ToListAsync();

                // Calculate total revenue of every year
                var totalRevenue = objectList.Sum(p => p.DoanhThu);

                // Convert to apporiate model
                foreach (var item in objectList)
                {
                    report.Add(
                        new YearReport
                        {
                            Year = item.Nam.ToString(),
                            FlightCount = item.SoChuyenBay.ToString(),
                            Revenue = item.DoanhThu.ToString(),
                            Ratio = ((item.DoanhThu * 100) / totalRevenue).ToString()
                        }
                    );
                }
            }

            return report;
        }

        /// <summary>
        /// To check if any flight is already departed, and then insert a report if needed
        /// </summary>
        /// <returns></returns>
        private async Task CheckFlightDeparted()
        {
            //using (var context = new FlightTicketSellEntities())
            //{
            //    // Get flight that departed but don't have a report
            //    var objectList = context.CHUYENBAYs
            //        .Where(p => p.NgayGio > DateTime.Now && p.DOANHTHUCHUYENBAYs == null);

            //    foreach (var item in objectList)
            //    {
            //        context.DOANHTHUCHUYENBAYs.Add(new DOANHTHUCHUYENBAY
            //        {
            //            MaChuyenBay = item.MaChuyenBay,
            //            `
            //        });
            //    }
            //}
        }

        #endregion

    }
}
