using FlightTicketSell.AttachedProperties;
using FlightTicketSell.Interface.Report;
using FlightTicketSell.Models.Enums;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FlightTicketSell.Views
{
    /// <summary>
    /// Interaction logic for PrintPreviewWindow.xaml
    /// </summary>
    public partial class ReportPrintPreviewWindow : UserControl, IReportPrintPreview
    {
        public ReportPrintPreviewWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Print report
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Print_Confirm(object sender, RoutedEventArgs e)
        {
            try
            {
                // Disable button so it won't be clicked many times
                this.IsEnabled = false;

                var printDialog = new PrintDialog();

                if (printDialog.ShowDialog() == true)
                {
                    printDialog.PrintVisual(print, "Invoice");
                }
            }
            catch (Exception) { }
            finally
            {
                this.IsEnabled = true;
            }
        }

        /// <summary>
        /// Cancel print and close window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel_Print(object sender, RoutedEventArgs e)
        {
            DialogHost.CloseDialogCommand.Execute(null, null);
        }

        public void UpdateColumns()
        {
            // Get report type
            var reportType = reportDataGrid.GetValue(ReportTypeProperty.ValueProperty);

            if (reportType is ReportType.None)
                return;

            // Changes header accordingly
            switch (reportType)
            {
                case ReportType.FlightsOfOneMonth:
                    reportDataGrid.Columns.Clear();
                    reportDataGrid.Columns.Add(new MaterialDesignThemes.Wpf.DataGridTextColumn
                    {
                        Header = "Mã chuyến bay",
                        Binding = new Binding("DisplayFlightCode") { Mode = BindingMode.OneTime }
                    });

                    reportDataGrid.Columns.Add(new MaterialDesignThemes.Wpf.DataGridTextColumn
                    {
                        Header = "Khởi hành",
                        Binding = new Binding("DisplayDepartDate") { Mode = BindingMode.OneTime }
                    });

                    reportDataGrid.Columns.Add(new MaterialDesignThemes.Wpf.DataGridTextColumn
                    {
                        Header = "Vé đã bán",
                        Binding = new Binding("TicketCount") { Mode = BindingMode.OneTime }
                    });

                    reportDataGrid.Columns.Add(new MaterialDesignThemes.Wpf.DataGridTextColumn
                    {
                        Header = "Doanh thu",
                        Binding = new Binding("DisplayRevenue") { Mode = BindingMode.OneTime }
                    });

                    reportDataGrid.Columns.Add(new MaterialDesignThemes.Wpf.DataGridTextColumn
                    {
                        Header = "Tỉ lệ",
                        Binding = new Binding("DisplayRatio") { Mode = BindingMode.OneTime }
                    });

                    break;
                case ReportType.MonthsOfOneYear:
                    reportDataGrid.Columns.Clear();
                    reportDataGrid.Columns.Add(new MaterialDesignThemes.Wpf.DataGridTextColumn
                    {
                        Header = "Tháng",
                        Binding = new Binding("Month") { Mode = BindingMode.OneTime }
                    });

                    reportDataGrid.Columns.Add(new MaterialDesignThemes.Wpf.DataGridTextColumn
                    {
                        Header = "Số chuyến bay",
                        Binding = new Binding("FlightCount") { Mode = BindingMode.OneTime }
                    });

                    reportDataGrid.Columns.Add(new MaterialDesignThemes.Wpf.DataGridTextColumn
                    {
                        Header = "Doanh thu",
                        Binding = new Binding("DisplayRevenue") { Mode = BindingMode.OneTime }
                    });

                    reportDataGrid.Columns.Add(new MaterialDesignThemes.Wpf.DataGridTextColumn
                    {
                        Header = "Tỉ lệ",
                        Binding = new Binding("DisplayRatio") { Mode = BindingMode.OneTime }
                    });

                    break;
                case ReportType.OneMonthOfAllYears:
                case ReportType.AllYears:
                    reportDataGrid.Columns.Clear();
                    reportDataGrid.Columns.Add(new MaterialDesignThemes.Wpf.DataGridTextColumn
                    {
                        Header = "Năm",
                        Binding = new Binding("Year") { Mode = BindingMode.OneTime }
                    });

                    reportDataGrid.Columns.Add(new MaterialDesignThemes.Wpf.DataGridTextColumn
                    {
                        Header = "Số chuyến bay",
                        Binding = new Binding("FlightCount") { Mode = BindingMode.OneTime }
                    });

                    reportDataGrid.Columns.Add(new MaterialDesignThemes.Wpf.DataGridTextColumn
                    {
                        Header = "Doanh thu",
                        Binding = new Binding("DisplayRevenue") { Mode = BindingMode.OneTime }
                    });

                    reportDataGrid.Columns.Add(new MaterialDesignThemes.Wpf.DataGridTextColumn
                    {
                        Header = "Tỉ lệ",
                        Binding = new Binding("DisplayRatio") { Mode = BindingMode.OneTime }
                    });

                    break;
            }
        }
    }
}
