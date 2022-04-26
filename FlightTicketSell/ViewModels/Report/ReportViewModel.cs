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
using System.Windows;
using System.Data.Entity.Core;

namespace FlightTicketSell.ViewModels
{
    public class ReportViewModel : BaseViewModel
    {
        #region Private Members

        /// <summary>
        /// Indicate if the view is first loaded
        /// </summary>
        private bool _firstLoad = true;

        /// <summary>
        /// Indicate that it is report loadable
        /// </summary>
        private bool _reportLoadAllow = true;

        #endregion

        #region Public Properties

        /// <summary>
        /// The years that have report
        /// </summary>
        public ObservableCollection<string> Years { get; set; }

        /// <summary>
        /// The months that have report of the current year
        /// </summary>
        public ObservableCollection<string> Months { get; set; }

        /// <summary>
        /// Current selected year
        /// </summary>
        public int Year { get { if (YearIndex <= 0) return 0; else return Convert.ToInt32(Years[YearIndex]); } }

        /// <summary>
        /// The index of the year combobox
        /// </summary>
        public int YearIndex { get; set; }

        /// <summary>
        /// Current selected month
        /// </summary>
        public int Month { get { if (MonthIndex <= 0) return 0; else return Convert.ToInt32(Months[MonthIndex]); } }

        /// <summary>
        /// The index of the month combobox
        /// </summary
        public int MonthIndex { get; set; }

        /// <summary>
        /// Total revenue
        /// </summary>
        public string TotalRevenue { get { return ReportHelper.VietnamCurrencyConvert(ReportHelper.CalculateTotalRevenue(Report)) + " VNĐ"; } set { } }

        /// <summary>
        ///  Indicate current report type
        /// </summary>
        public ReportType CurrentReportType { get { if (Report is null) return ReportType.None; return Report.Type; } }

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

        /// <summary>
        /// Query new report
        /// </summary>
        public ICommand MonthChangedCommand { get; set; }

        /// <summary>
        /// Get new month collection, query new report
        /// </summary>
        public ICommand YearChangedCommand { get; set; }

        /// <summary>
        /// Switch to the print report view
        /// </summary>
        public ICommand PrintCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>%
        public ReportViewModel()
        {
            LoadCommand = new RelayCommand<object>((p) => true, (p) => 
            {
                // Init everything when viewmodel first load
                if (_firstLoad) Init();
                
                OnPropertyChanged(nameof(Report));
            });
            
            MonthChangedCommand = new RelayCommand<object>((p) => true, async (p) =>
            {
                // Don't execute on first load view model
                if (_firstLoad || !_reportLoadAllow) return;

                // Check if any flight departed to add a report if needed
                await ReportHelper.ReportExistGuarantee();

                // Update report
                Report = await ReportHelper.GetReportAsync(Month, Year);
            });

            YearChangedCommand = new RelayCommand<object>((p) => true, async (p) =>
            {
                // Don't execute on first load view model
                if (_firstLoad) return;

                // Only allow load report when everything is done
                _reportLoadAllow = false;

                // A buffer to store current month value selected
                var month = Month;

                // Load months
                using (var context = new FlightTicketSellEntities())
                {
                    // Get months
                    if (Year == 0)
                        Months = new ObservableCollection<string> { "Tất cả", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };
                    else
                    {
                        var yearReport = await context.DOANHTHUNAMs.Where(year => year.Nam == Year).FirstOrDefaultAsync();
                        var latestMonth = yearReport.DOANHTHUTHANGs.Max(thang => thang.Thang);

                        Months = new ObservableCollection<string> { "Tất cả" };
                        for (int i = 1; i <= latestMonth; i++)
                            Months.Add(i.ToString());
                    }
                }

                // Allow report load
                _reportLoadAllow = true;

                // Reload previous month if another year also have it
                if (Months.Contains(month.ToString()))
                    MonthIndex = Months.IndexOf(month.ToString());
                else
                    MonthIndex = 0;

                // Check if any flight departed to add a report if needed
                await ReportHelper.ReportExistGuarantee();

                // Update report
                Report = await ReportHelper.GetReportAsync(Month, Year);
            });

            PrintCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = AppView.ReportPrint);
        }

        /// <summary>
        ///  Load years, months and report
        /// </summary>
        private async void Init()
        {
            // Get months, years available
            using (var context = new FlightTicketSellEntities())
            {
                try
                {
                    // Check if any flight departed to add a report if needed
                    await ReportHelper.ReportExistGuarantee();

                    // Get years
                    Years = new ObservableCollection<string>(await context.DOANHTHUNAMs.Select(x => x.Nam.ToString()).ToListAsync());
                    Years.Insert(0, "Tất cả");

                    // Set default index for year
                    YearIndex = 0;

                    // Get months
                    Months = new ObservableCollection<string> { "Tất cả", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };

                    // Set default index for month
                    MonthIndex = 0;
                }
                catch (EntityException e)
                {
                    MessageBox.Show("Database access failed!", string.Format($"Exception: {e.Message}"), MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            // Get report
            Report = await ReportHelper.GetReportAsync(Month, Year);

            // No longer first load
            _firstLoad = false;
        }

        #endregion
    }
}
