using FlightTicketSell.Helpers;
using FlightTicketSell.Models;
using FlightTicketSell.Models.Enums;
using FlightTicketSell.Models.Report;
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
    /// <summary>
    /// A helper class that help query report from database
    /// </summary>
    public static class ReportHelper
    {
        #region Report Access

        /// <summary>
        /// Get flights of one month report
        /// </summary>
        /// <param name="month">The month</param>
        /// <param name="year">The year</param>
        /// <returns>The flights of one month report</returns>
        public static async Task<ObservableCollection<object>> GetFlighsOfOneMonthReport(int month, int year)
        {
            try
            {
                using (var context = new FlightTicketSellEntities())
                {
                    var flightList = await context.DOANHTHUCHUYENBAYs
                        .Where(dt => dt.DOANHTHUTHANG.Thang == month && dt.DOANHTHUTHANG.DOANHTHUNAM.Nam == year)
                        .Select(dt => new FlightReport()
                        {
                            DisplayFlightCode =
                                dt.CHUYENBAY.DUONGBAY.SANBAY.VietTat +
                                dt.CHUYENBAY.DUONGBAY.SANBAY1.VietTat +
                                "-" +
                                dt.CHUYENBAY.MaChuyenBay,
                            DepartDate = dt.CHUYENBAY.NgayGio,
                            TicketCount = dt.SoVe,
                            Revenue = dt.DoanhThu,
                            Ratio = dt.TiLe,
                        })
                        .ToListAsync();

                    return new ObservableCollection<object>(flightList.Cast<object>());
                }
            }
            catch (EntityException e)
            {
                MessageBox.Show("Database access failed!", string.Format($"Exception: {e.Message}"), MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        /// <summary>
        /// Get months of one year report
        /// </summary>
        /// <param name="year">The year</param>
        /// <returns>The months of one year report</returns>
        public static async Task<ObservableCollection<object>> GetMonthsOfOneYearReport(int year)
        {
            try
            {
                using (var context = new FlightTicketSellEntities())
                {
                    var list = await context.DOANHTHUTHANGs
                        .Where(dt => dt.DOANHTHUNAM.Nam == year)
                        .Select(dt => new MonthReport()
                        {
                            Month = dt.Thang,
                            FlightCount = dt.SoChuyenBay,
                            Revenue = dt.DoanhThu,
                            Ratio = dt.TiLe
                        })
                        .ToListAsync();

                    return new ObservableCollection<object>(list.Cast<object>());
                }
            }
            catch (EntityException e)
            {
                MessageBox.Show("Database access failed!", string.Format($"Exception: {e.Message}"), MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        /// <summary>
        /// Get one month of all year report
        /// </summary>
        /// <param name="month">The month</param>
        /// <returns>The one month of all year report</returns>
        public static async Task<ObservableCollection<object>> GetOneMonthOfAllYear(int month)
        {
            try
            {
                using (var context = new FlightTicketSellEntities())
                {
                    var list = await context.DOANHTHUTHANGs
                        .Where(dt => dt.Thang == month)
                        .Select(dt => new YearReport()
                        {
                            Year = dt.DOANHTHUNAM.Nam,
                            FlightCount = dt.SoChuyenBay,
                            Revenue = dt.DoanhThu,
                            Ratio = dt.TiLe
                        })
                        .ToListAsync();

                    return new ObservableCollection<object>(list.Cast<object>());
                }
            }
            catch (EntityException e)
            {
                MessageBox.Show("Database access failed!", string.Format($"Exception: {e.Message}"), MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        /// <summary>
        /// Get all year report
        /// </summary>
        /// <returns>The all year report</returns>
        public static async Task<ObservableCollection<object>> GetAllYearReport()
        {
            try
            {
                using (var context = new FlightTicketSellEntities())
                {
                    var totalRevenue = await context.DOANHTHUNAMs.SumAsync(dt => dt.DoanhThu);

                    var list = await context.DOANHTHUNAMs
                        .Select(dt => new YearReport()
                        {
                            Year = dt.Nam,
                            FlightCount = dt.SoChuyenBay,
                            Revenue = dt.DoanhThu,
                            Ratio = dt.DoanhThu / totalRevenue
                        })
                        .ToListAsync();

                    return new ObservableCollection<object>(list.Cast<object>());
                }
            }
            catch (EntityException e)
            {
                MessageBox.Show("Database access failed!", string.Format($"Exception: {e.Message}"), MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        #endregion

        #region Calculate Total Revenue

        public static decimal CalculateTotalRevenue(ObservableCollection<object> report, ReportType reportType)
        {
            switch (reportType)
            {
                case ReportType.FlightsOfOneMonth:
                    {
                        var castedReport = report.Cast<FlightReport>();
                        return castedReport.Sum(rp => rp.Revenue);
                    }
                case ReportType.MonthsOfOneYear:
                    {
                        var castedReport = report.Cast<MonthReport>();
                        return castedReport.Sum(rp => rp.Revenue);

                    }
                case ReportType.OneMonthOfAllYears:
                case ReportType.AllYears:
                    {
                        var castedReport = report.Cast<YearReport>();
                        return castedReport.Sum(rp => rp.Revenue);
                    }
                default:
                    return (decimal)0;
            }
        }

        #endregion

        #region Converters

        /// <summary>
        /// Convert a string month, year to an int
        /// </summary>
        /// <param name="value">The month, year in string type</param>
        /// <param name="allValue">The string value represnt 0</param>
        /// <returns></returns>
        public static int MonthYearToIntConverter(string value, string allValue)
        {
            if (value == allValue)
                return 0;
            return Convert.ToInt32(value);
        }

        #endregion
    }
}
