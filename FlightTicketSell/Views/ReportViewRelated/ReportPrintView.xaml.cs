using FlightTicketSell.AttachedProperties;
using FlightTicketSell.Models.Enums;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FlightTicketSell.Views.ReportViewRelated
{
    /// <summary>
    /// Interaction logic for ReportPrintView.xaml
    /// </summary>
    public partial class ReportPrintView : UserControl
    {
        public ReportPrintView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Task to do when user control loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
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
    }
}
