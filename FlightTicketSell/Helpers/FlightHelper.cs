using FlightTicketSell.Models;
using System;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FlightTicketSell.Helpers
{
    public class FlightHelper
    {
        #region Flight Check

        /// <summary>
        /// Check if the year report, month report existed
        /// Because flight report can only be added when there's
        /// already year, month report
        /// 
        /// Update flight state
        /// 
        /// Add one if there's none
        /// </summary>
        public async static Task FlightDepartedRefresh()
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

                    // Action on each flights
                    foreach (var item in flights)
                    {

                        #region Guarantees month, year existence

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

                        #endregion

                        // Update flight's depart flag
                        item.DaKhoiHanh = true;
                        await context.SaveChangesAsync();

                        // Cancel all bookings
                        var bookings = await context.DATCHOes.Where(dc => dc.MaChuyenBay == item.MaChuyenBay).ToListAsync();
                        for (int i = 0; i < bookings.Count; i++)
                            bookings[i].TrangThai = BookHelper.BookingStateToString(Models.Enums.BookingState.Cancel);
                        await context.SaveChangesAsync();
    
                        // Add flight report
                        var monthReportID = await context.DOANHTHUTHANGs
                            .Where(p => p.Thang == item.NgayGio.Month && p.DOANHTHUNAM.Nam == item.NgayGio.Year)
                            .Select(p => p.MaDoanhThuThang)
                            .FirstOrDefaultAsync();

                        var newFlightReport = new DOANHTHUCHUYENBAY
                        {
                            MaDoanhThuThang = monthReportID,
                            MaChuyenBay = item.MaChuyenBay,
                            SoVe = item.VEs.Count + item.DATCHOes.Sum(p => p.SoVeDat),
                            DoanhThu = Convert.ToInt32(
                                item.VEs.Sum(p => p.GiaTien) +
                                item.DATCHOes.Sum(p => p.GiaTien)
                            ),
                        };

                        // Update month, year revenue, flight count
                        var yearReport = await context.DOANHTHUNAMs
                            .Where(dtn => dtn.Nam == item.NgayGio.Year)
                            .FirstOrDefaultAsync();

                        yearReport.DoanhThu += newFlightReport.DoanhThu;
                        yearReport.SoChuyenBay++;

                        var monthReport = await context.DOANHTHUTHANGs
                            .Where(dtt => dtt.MaDoanhThuThang == monthReportID)
                            .FirstOrDefaultAsync();
                        
                        monthReport.DoanhThu += newFlightReport.DoanhThu;
                        monthReport.SoChuyenBay++;

                        // Update every month of that year's revenue ratio
                        var monthReports = await context.DOANHTHUTHANGs
                            .Where(dtt => dtt.DOANHTHUNAM.Nam == yearReport.Nam)
                            .ToListAsync();

                        for (int i = 0; i < monthReports.Count; i++)
                            monthReports[i].TiLe = yearReport.DoanhThu == (decimal)0.0 ? (decimal)0.0 : monthReports[i].DoanhThu / yearReport.DoanhThu;

                        // Update every flight of that month's revenue ratio
                        var flightReports = await context.DOANHTHUCHUYENBAYs
                            .Where(dtcb => dtcb.DOANHTHUTHANG.Thang == monthReport.Thang && dtcb.DOANHTHUTHANG.DOANHTHUNAM.Nam == yearReport.Nam)
                            .ToListAsync();

                        for (int i = 0; i < flightReports.Count; i++)
                            flightReports[i].TiLe = monthReport.DoanhThu == (decimal)0.0 ? (decimal)0.0 : flightReports[i].DoanhThu / monthReport.DoanhThu;

                        // Add new flight report
                        newFlightReport.TiLe = monthReport.DoanhThu == (decimal)0.0 ? (decimal)0.0 : newFlightReport.DoanhThu / monthReport.DoanhThu;
                        context.DOANHTHUCHUYENBAYs.Add(newFlightReport);
                        
                        /// Save changes down to database
                        await context.SaveChangesAsync();
                    }
                }
                catch (EntityException e)
                {
                    MessageBox.Show("Database access failed!", string.Format($"Exception: {e.Message}"), MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
            }
        }

        #endregion
    }
}
