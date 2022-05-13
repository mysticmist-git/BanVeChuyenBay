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
                    MessageBox.Show("Database access failed!", string.Format($"Exception: {e.Message}"), MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
            }
        }

        #endregion
    }
}
