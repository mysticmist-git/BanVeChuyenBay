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
        private bool firstLoad = true;

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
            // Create commands
            LoadCommand = new RelayCommand<object>((p) => true, async (p) =>
            {
                if (!firstLoad)
                {
                    OnPropertyChanged(nameof(Report));

                    return;
                }


                // Get months, years available
                using (var context = new FlightTicketSellEntities())
                {
                    try
                    {
                        // Get years
                        Years = new ObservableCollection<string>(await context.DOANHTHUNAMs.Select(x => x.Nam.ToString()).ToListAsync());
                        Years.Insert(0, "Tất cả");

                        // Get months
                        if (Year == 0)
                            Months = new ObservableCollection<string> { "Tất cả", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };
                        else
                        {
                            Months = new ObservableCollection<string>(await context.DOANHTHUNAMs
                                .Where(x => x.Nam == Year)
                                .Join(context.DOANHTHUTHANGs, nam => nam.MaDoanhThuNam, thang => thang.MaDoanhThuNam, (nam, thang) => thang.Thang.ToString()).ToListAsync()
                            );

                            Months.Insert(0, "Tất cả");
                        }
                    }
                    catch (EntityException e)
                    {
                        MessageBox.Show("Database access failed!", string.Format($"Exception: {e.Message}"), MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

                // Notify that Year index changed
                OnPropertyChanged(nameof(YearIndex));
                OnPropertyChanged(nameof(MonthIndex));

                // Check if any flight departed to add a report if needed
                await ReportHelper.ReportExistGuarantee();

                // Update report
                Report = await ReportHelper.GetReportAsync(Month, Year);

                YearIndex = 0;
                MonthIndex = 0;

                firstLoad = false;
            });

            MonthChangedCommand = new RelayCommand<object>((p) => true, async (p) =>
            {
                // Check if any flight departed to add a report if needed
                await ReportHelper.ReportExistGuarantee();

                // Update report
                Report = await ReportHelper.GetReportAsync(Month, Year);
            });

            YearChangedCommand = new RelayCommand<object>((p) => true, async (p) =>
            {
                using (var context = new FlightTicketSellEntities())
                {
                    // Get months
                    if (Year == 0)
                        Months = new ObservableCollection<string> { "Tất cả", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };
                    else
                    {
                        Months = new ObservableCollection<string>(await context.DOANHTHUNAMs
                            .Where(x => x.Nam == Year)
                            .Join(context.DOANHTHUTHANGs, nam => nam.MaDoanhThuNam, thang => thang.MaDoanhThuNam, (nam, thang) => thang.Thang.ToString()).ToListAsync()
                        );

                        Months.Insert(0, "Tất cả");
                    }

                    MonthIndex = 0;
                }

                // Notify that month index changed
                OnPropertyChanged(nameof(MonthIndex));

                // Check if any flight departed to add a report if needed
                await ReportHelper.ReportExistGuarantee();

                // Update report
                Report = await ReportHelper.GetReportAsync(Month, Year);
            });

            PrintCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = AppView.ReportPrint);
        }

        #endregion
    }
}
