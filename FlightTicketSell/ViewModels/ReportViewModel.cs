using FlightTicketSell.Models;
using FlightTicketSell.ViewModels.Report;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Linq;
using System.Windows.Input;
using System;
using FlightTicketSell.Models.Enums;
using FlightTicketSell.Views.Helper;
using System.ComponentModel;

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
        public string TotalRevenue { get { return ReportHelper.VietnamCurrencyConvert(ReportHelper.CalculateTotalRevenue(Report)) + " VNĐ"; } set { } }

        /// <summary>
        ///  Indicate current report type
        /// </summary>
        public ReportType CurrentReportType { get { if (Report is null) return ReportType.None; return Report.Type; } set { } }

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
                await ReportHelper.ReportExistGuarantee();

                // Update report
                Report = await ReportHelper.GetReportAsync(Month, Year);
            });

            MonthYearChangedCommand = new RelayCommand<object>((p) => true, async (p) =>
            {
                // Check if any flight departed to add a report if needed
                await ReportHelper.ReportExistGuarantee();

                // Update report
                Report = await ReportHelper.GetReportAsync(Month, Year);
            });
        }

        #endregion
    }
}
