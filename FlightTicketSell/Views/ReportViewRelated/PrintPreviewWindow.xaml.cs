using FlightTicketSell.AttachedProperties;
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

namespace FlightTicketSell.Views.ReportViewRelated
{
    /// <summary>
    /// Interaction logic for PrintPreviewWindow.xaml
    /// </summary>
    public partial class ReportPrintPreviewWindow : UserControl
    {
        public ReportPrintPreviewWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Task to do when window loaded
        /// </summary>-
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(reportDataGrid, OnItemSourceChanged);
            }
        }

        /// <summary>
        /// This is called when item source is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnItemSourceChanged(object sender, EventArgs e)
        {
            // Get report type
            var reportType = reportDataGrid.GetValue(ReportTypeProperty.ValueProperty);

            if (reportType is ReportType.None)
                return;

            // Changes header accordingly
            switch (reportType)
            {
                case ReportType.FlightsOfOneMonth:
                    reportDataGrid.Columns[0].Header = "Mã chuyến bay";
                    reportDataGrid.Columns[1].Header = "Khởi hành";
                    reportDataGrid.Columns[2].Header = "Vé đã bán";
                    reportDataGrid.Columns[3].Header = "Doanh thu";
                    reportDataGrid.Columns[4].Header = "Tỉ lệ";
                    break;
                case ReportType.MonthsOfOneYear:
                    reportDataGrid.Columns[0].Header = "Tháng";
                    reportDataGrid.Columns[1].Header = "Số chuyến bay";
                    reportDataGrid.Columns[2].Header = "Doanh thu";
                    reportDataGrid.Columns[3].Header = "Tỉ lệ";
                    break;
                case ReportType.OneMonthOfAllYears:
                case ReportType.AllYears:
                    reportDataGrid.Columns[0].Header = "Năm";
                    reportDataGrid.Columns[1].Header = "Số chuyến bay";
                    reportDataGrid.Columns[2].Header = "Doanh thu";
                    reportDataGrid.Columns[3].Header = "Tỉ lệ";
                    break;
            }

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
            catch (Exception ex) { }
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
    }
}
