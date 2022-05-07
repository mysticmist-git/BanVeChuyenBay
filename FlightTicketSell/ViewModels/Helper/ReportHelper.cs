using FlightTicketSell.Models;
using FlightTicketSell.Models.Enums;
using FlightTicketSell.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FlightTicketSell.Views.Helper
{
    public static class ReportHelper
    {
        #region Report Access

        /// <summary>
        /// Get report, take in month and year for different case
        /// </summary>
        /// <param name="month">The month of the report with 0 mean every month</param>
        /// <param name="year">The year of the report with 0 mean every year</param>
        /// <returns></returns>
        public async static Task<Report<object>> GetReportAsync(int month, int year)
        {
            // Check month, year validation
            if (month < 0 || month > 12)
                return null;

            if (year < 0)
                return null;

            // If query a specific month
            if (month != 0)
            {
                // A specific year
                if (year != 0)
                {
                    // Return a month report of that year
                    return new Report<object> { Content = await GetFlightReport(month, year), Type = ReportType.FlightsOfOneMonth };
                }

                // Return a report of a month of every year
                return new Report<object> { Content = await GetAllYearReport(month), Type = ReportType.OneMonthOfAllYears };
            }

            // Query all month

            // Query for a specific year
            if (year != 0)
            {
                return new Report<object> { Content = await GetMonthReport(year), Type = ReportType.MonthsOfOneYear };
            }

            // Query all year
            return new Report<object> { Content = await GetAllYearReport(), Type = ReportType.AllYears };
        }

        /// <summary>
        /// Get the month report with specific month and year
        /// </summary>
        /// <param name="month">The month of the report</param>
        /// <param name="year">The year of the report</param>
        /// <returns></returns>
        public static async Task<ObservableCollection<object>> GetFlightReport(int month, int year)
        {
            // Declare report that'll be return later
            var report = new ObservableCollection<object>();

            // Get report
            using (var context = new FlightTicketSellEntities())
            {
                try
                {
                    // Get report from database

                    var objectList = await context.CHUYENBAYs
                        .Select(cb => new FlightReport
                        {
                            FlightCode = cb.DUONGBAY.SANBAY.VietTat + cb.DUONGBAY.SANBAY1.VietTat + "-" + cb.MaChuyenBay.ToString(),
                            DepartTime = cb.NgayGio.ToString(),
                            TicketSold = cb.VEs.Count() + cb.DATCHOes.Sum(dc => dc.SoVeDat).ToString(),
                            Revenue = cb.DOANHTHUCHUYENBAYs.FirstOrDefault().DoanhThu.ToString(),
                            Ratio = cb.DOANHTHUCHUYENBAYs.FirstOrDefault().TiLe.ToString()
                        }).ToListAsync();

                    report = new ObservableCollection<object>(objectList);
                }
                catch (EntityException e)
                {
                    MessageBox.Show("Database access failed!", string.Format($"Exception: {e.Message}"), MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }

            return report;
        }

        /// <summary>
        /// Get the all year report with specific month
        /// </summary>
        /// <param name="month">The month of the report</param>
        /// <returns></returns>
        private static async Task<ObservableCollection<object>> GetAllYearReport(int month)
        {
            // Declare report that'll be return later
            var report = new ObservableCollection<object>();

            // Get report
            using (var context = new FlightTicketSellEntities())
            {
                try
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
                                Year = item.DOANHTHUNAM.Nam,
                                FlightCount = item.SoChuyenBay,
                                Revenue = item.DoanhThu.ToString(),
                                Ratio = item.TiLe.ToString()
                            }
                        );
                    }
                }
                catch (EntityException e)
                {
                    MessageBox.Show("Database access failed!", string.Format($"Exception: {e.Message}"), MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            return report;
        }

        /// <summary>
        /// Get the year report with specific year
        /// </summary>
        /// <param name="year">The year of the report</param>
        /// <returns></returns>
        private static async Task<ObservableCollection<object>> GetMonthReport(int year)
        {
            // Declare report that'll be return later
            var report = new ObservableCollection<object>();

            // Get report
            using (var context = new FlightTicketSellEntities())
            {
                try
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
                                Month = item.Thang,
                                FlightCount = item.SoChuyenBay,
                                Revenue = item.DoanhThu.ToString(),
                                Ratio = (item.DoanhThu / item.DOANHTHUNAM.DoanhThu).ToString()
                            }
                        );
                    }
                }
                catch (EntityException e)
                {
                    MessageBox.Show("Database access failed!", string.Format($"Exception: {e.Message}"), MessageBoxButton.OK, MessageBoxImage.Error);
                }
            } 

            return report;
        }

        /// <summary>
        /// Get the year report of every year
        /// </summary>
        /// <returns></returns>
        private static async Task<ObservableCollection<object>> GetAllYearReport()
        {
            // Declare report that'll be return later
            var report = new ObservableCollection<object>();

            // Get report
            using (var context = new FlightTicketSellEntities())
            {
                try
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
                                Year = item.Nam,
                                FlightCount = item.SoChuyenBay,
                                Revenue = item.DoanhThu.ToString(),
                                Ratio = (item.DoanhThu / totalRevenue).ToString()
                            }
                        );
                    }
                }
                catch (EntityException e)
                {
                    MessageBox.Show("Database access failed!", string.Format($"Exception: {e.Message}"), MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            return report;
        }

        #endregion

        #region Total Revenue Calculator

        /// <summary>
        /// Calculate the revenue for input report
        /// </summary>
        /// <param name="report"></param>
        /// <returns></returns>
        public static decimal CalculateTotalRevenue(Report<object> report)
        {
            // Return 0 if Report is null
            if (report == null)
                return 0;

            // Initialize total revenue
            decimal totalRevenue = 0;

            // Get report type
            var reportType = (report as Report<object>).Type;


            // If report type is FlightReport
            if (reportType is ReportType.FlightsOfOneMonth)
            {
                // Get report content
                var collection = (report as Report<object>).Content;

                // For each item in input report
                foreach (var item in collection)
                    totalRevenue += (item as FlightReport).GetRevenue();

                return totalRevenue;
            }

            // If report type is MonthReport
            if (reportType is ReportType.MonthsOfOneYear)
            {
                // Get report content
                var collection = (report as Report<object>).Content;

                // For each item in input report
                foreach (var item in collection)
                    totalRevenue += (item as MonthReport).GetRevenue();

                return totalRevenue;
            }

            // If report type is YearReport
            if (reportType is ReportType.OneMonthOfAllYears || reportType is ReportType.AllYears)
                {
                // Get report content
                var collection = (report as Report<object>).Content;

                // For each item in input report
                foreach (var item in collection)
                    totalRevenue += (item as YearReport).GetRevenue();

                return totalRevenue;
            }

            // Return 0 for default
            return 0;
        }

        #endregion

        #region Report Check

        /// <summary>
        /// Check if the year report, month report existed
        /// Because flight report can only be added when there's
        /// already year, month report
        /// 
        /// Add one if there's none
        /// </summary>
        public async static Task ReportExistGuarantee()
        {
            using (var context = new FlightTicketSellEntities())
            {
                try
                {
                    // Find flight that departed but didn't have a report
                    var flights = await context.CHUYENBAYs
                        .Where(p => p.DaKhoiHanh == false && p.NgayGio <= DateTime.Now)
                        .ToListAsync();

                    // Stop if there's none
                    if (flights.Count == 0)
                        return;

                    // For each flights, check year, month existence and then add a new flight report
                    foreach (var item in flights)
                    {

                        // Check year existence
                        if (context.DOANHTHUNAMs.Where(p => p.Nam == item.NgayGio.Year).ToList().Count == 0)
                        {
                            var temp = new DOANHTHUNAM { Nam = item.NgayGio.Year };
                            context.DOANHTHUNAMs.Add(temp);
                            /// Save changes down to database
                            await context.SaveChangesAsync();
                        }

                        // Check month existence
                        if (context.DOANHTHUTHANGs.Where(p => p.Thang == item.NgayGio.Month && p.DOANHTHUNAM.Nam == item.NgayGio.Year).ToList().Count == 0)
                        {
                            // Get year report ID
                            var yearReportIDHolder = await (context.DOANHTHUNAMs
                                .Where(p => p.Nam == item.NgayGio.Year).FirstOrDefaultAsync());

                            context.DOANHTHUTHANGs.Add(new DOANHTHUTHANG { Thang = item.NgayGio.Month, MaDoanhThuNam = yearReportIDHolder.MaDoanhThuNam });

                            /// Save changes down to database
                            await context.SaveChangesAsync();

                        }

                        // Update flight's depart flag
                        item.DaKhoiHanh = true;
                        await context.SaveChangesAsync();

                        // Add flight report
                        var monthReportIDHolder = await context.DOANHTHUTHANGs.Where(p => p.Thang == item.NgayGio.Month && p.DOANHTHUNAM.Nam == item.NgayGio.Year).FirstOrDefaultAsync();

                        var newFlightReport = new DOANHTHUCHUYENBAY
                        {
                            MaDoanhThuThang = monthReportIDHolder.MaDoanhThuThang,
                            MaChuyenBay = item.MaChuyenBay,
                            SoVe = item.VEs.Count + item.DATCHOes.Sum(p => p.SoVeDat),
                            DoanhThu = Convert.ToInt32(
                                item.VEs.Sum(p => p.GiaTien) +
                                item.DATCHOes.Sum(p => p.GiaTien)
                            )
                        };
                        context.DOANHTHUCHUYENBAYs.Add(newFlightReport);

                        /// Save changes down to database
                        await context.SaveChangesAsync();
                    }
                }
                catch (EntityException e)
                {
                    MessageBox.Show("Database access failed!", string.Format($"Exception: {e.Message}"), MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        #endregion

        #region Formater

        /// <summary>
        /// Convert a decimal value to a vietnam currency format money
        /// </summary>
        /// <param name="money">The money needed to be converted</param>
        /// <returns></returns>
        public static string VietnamCurrencyConvert(decimal money) => string.Format(new CultureInfo("vi-VN"), "{0:#,##0.00}", money);

        #endregion
    }
}
