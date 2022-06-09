using FlightTicketSell.Models;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Windows.Input;
using FlightTicketSell.Models.Enums;
using FlightTicketSell.Views.Helper;
using System.Windows;
using System.Data.Entity.Core;
using System.Threading.Tasks;
using FlightTicketSell.Helpers;
using FlightTicketSell.Interface;

namespace FlightTicketSell.ViewModels
{
    public class ReportViewModel : BaseViewModel
    {
        #region Private Members

        #region Constants

        /// <summary>
        /// The "All" text in months and years itemsources
        /// </summary>
        private const string _all = "Tất cả";

        #endregion

        #endregion

        #region Public Properties

        /// <summary>
        /// The year list
        /// </summary>
        public ObservableCollection<string> Years { get; set; }

        /// <summary>
        /// The month list
        /// </summary>
        public ObservableCollection<string> Months { get; set; }

        /// <summary>
        /// The current year selected
        /// </summary>
        public string CurrentYear { get; set; }

        /// <summary>
        /// The current month selected
        /// </summary>
        public string CurrentMonth { get; set; }

        /// <summary>
        /// The totla revenue
        /// </summary>
        public decimal TotalRevenue { get; set; }

        /// <summary>
        /// The display total revenue
        /// </summary>
        public string DisplayTotalRevenue { get => FormatHelper.VietnamCurrencyFormat(TotalRevenue) + " VNĐ"; }

        /// <summary>
        /// The type of the current report
        /// </summary>
        public ReportType CurrentReportType
        {
            get
            {
                if (CurrentMonth == _all && CurrentYear == _all)
                    return ReportType.AllYears;

                if (CurrentMonth != _all && CurrentYear == _all)
                    return ReportType.OneMonthOfAllYears;

                if (CurrentMonth == _all && CurrentYear != _all)
                    return ReportType.MonthsOfOneYear;

                if (CurrentMonth != _all && CurrentYear != _all)
                    return ReportType.FlightsOfOneMonth;

                return ReportType.AllYears;
            }
        }

        /// <summary>
        /// The report
        /// </summary>
        public ObservableCollection<object> Report { get; set; } = new ObservableCollection<object>();

        #endregion

        #region Commands

        /// <summary>
        /// Executes on view load
        /// </summary>
        public ICommand LoadCommand { get; set; }

        /// <summary>
        /// Executes on selected month changed
        /// </summary>
        public ICommand MonthChangedCommand { get; set; }

        /// <summary>
        /// Executes on selected year changed
        /// </summary>
        public ICommand YearChangedCommand { get; set; }

        /// <summary>
        /// The print command, open print preview
        /// </summary>
        public ICommand PrintCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ReportViewModel()
        {
            // Create commands
            LoadCommand = new RelayCommand<object>(p => true, async p =>
            {
                // Refresh flight to update departed flight
                await FlightHelper.FlightDepartedRefresh();
                
                // Load months, years itemsources
                Months = new ObservableCollection<string> { _all, "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };

                try
                {
                    using (var context = new FlightTicketSellEntities())
                    {
                        var yearList = await context.DOANHTHUNAMs.Select(dtn => dtn.Nam.ToString()).ToListAsync();
                        yearList.Insert(0, _all);
                        Years = new ObservableCollection<string>(yearList);
                    }
                }
                catch (EntityException e)
                {
                    NotifyHelper.ShowEntityException(e);;
                }

                // Default current month, year
                CurrentMonth = _all;
                CurrentYear = _all;

                await RefreshReport();
            });

            MonthChangedCommand = new RelayCommand<object>(p => true, async p =>
            {
                // Refresh flight to update departed flight
                await FlightHelper.FlightDepartedRefresh();

                await RefreshReport();
            });

            YearChangedCommand = new RelayCommand<object>(p => true, async p =>
            {
                // Refresh flight to update departed flight
                await FlightHelper.FlightDepartedRefresh();

                var yearInt = ReportHelper.MonthYearToIntConverter(CurrentYear, _all);

                try
                {
                    using (var context = new FlightTicketSellEntities())
                    {
                        // Load new month list and refresh report
                        if (CurrentYear == _all)
                        {
                            Months = new ObservableCollection<string> { _all, "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };

                        }
                        else
                        {

                            // Load new month list
                            var maxMonth = await context.DOANHTHUTHANGs.Where(dtt => dtt.DOANHTHUNAM.Nam == yearInt).MaxAsync(dtt => dtt.Thang);

                            var monthList = new ObservableCollection<string> { _all };
                            for (int i = 1; i <= maxMonth; i++)
                                monthList.Add(i.ToString());

                            Months = new ObservableCollection<string>(monthList);
                        }

                        // Get new report
                        await RefreshReport();
                    }
                }
                catch (EntityException e)
                {
                    NotifyHelper.ShowEntityException(e);;
                }
            });

            PrintCommand = new RelayCommand<object>(p => true, async p =>
            {
                await (p as IReport).OpenPrintPreview();
            });
        }

        /// <summary>
        /// Refresh report data
        /// </summary>
        /// <returns></returns>
        private async Task RefreshReport()
        {
            if (CurrentReportType == ReportType.AllYears)
                Report = await ReportHelper.GetAllYearReport();

            if (CurrentReportType == ReportType.OneMonthOfAllYears)
                Report = await ReportHelper.GetOneMonthOfAllYear(ReportHelper.MonthYearToIntConverter(CurrentMonth, _all));

            if (CurrentReportType == ReportType.MonthsOfOneYear)
                Report = await ReportHelper.GetMonthsOfOneYearReport(ReportHelper.MonthYearToIntConverter(CurrentYear, _all));

            if (CurrentReportType == ReportType.FlightsOfOneMonth)
                Report = await ReportHelper.GetFlighsOfOneMonthReport(
                    ReportHelper.MonthYearToIntConverter(CurrentMonth, _all),
                    ReportHelper.MonthYearToIntConverter(CurrentYear, _all)
                    );

            TotalRevenue = ReportHelper.CalculateTotalRevenue(Report, CurrentReportType);

        }

        #endregion

        #region Methods

        /// <summary>
        /// Notify view to update datagrid columns for report
        /// </summary>
        public void NotifyReport()
        {
            OnPropertyChanged(nameof(Report));
        }

        #endregion
    }
}
