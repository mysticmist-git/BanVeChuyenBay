using FlightTicketSell.AttachedProperties;
using FlightTicketSell.Interface.Report;
using FlightTicketSell.Models.Enums;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace FlightTicketSell.Views
{
    /// <summary>
    /// Interaction logic for ReportView.xaml
    /// </summary>
    public partial class ReportView : UserControl, IReport
    {
        public ReportView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Task to do when user control loaded
        /// </summary>-
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
                    (sender as DataGrid).Columns.Clear();
                    (sender as DataGrid).Columns.Add(new MaterialDesignThemes.Wpf.DataGridTextColumn
                    {
                        Header = "Mã chuyến bay",
                        Binding = new Binding("DisplayFlightCode") { Mode = BindingMode.OneTime }
                    });

                    (sender as DataGrid).Columns.Add(new MaterialDesignThemes.Wpf.DataGridTextColumn
                    {
                        Header = "Khởi hành",
                        Binding = new Binding("DisplayDepartDate") { Mode = BindingMode.OneTime }
                    });

                    (sender as DataGrid).Columns.Add(new MaterialDesignThemes.Wpf.DataGridTextColumn
                    {
                        Header = "Vé đã bán",
                        Binding = new Binding("TicketCount") { Mode = BindingMode.OneTime }
                    });

                    (sender as DataGrid).Columns.Add(new MaterialDesignThemes.Wpf.DataGridTextColumn
                    {
                        Header = "Doanh thu",
                        Binding = new Binding("DisplayRevenue") { Mode = BindingMode.OneTime }
                    });

                    (sender as DataGrid).Columns.Add(new MaterialDesignThemes.Wpf.DataGridTextColumn
                    {
                        Header = "Tỉ lệ",
                        Binding = new Binding("DisplayRatio") { Mode = BindingMode.OneTime }
                    });

                    break;
                case ReportType.MonthsOfOneYear:
                    (sender as DataGrid).Columns.Clear();
                    (sender as DataGrid).Columns.Add(new MaterialDesignThemes.Wpf.DataGridTextColumn
                    {
                        Header = "Tháng",
                        Binding = new Binding("Month") { Mode = BindingMode.OneTime }
                    });

                    (sender as DataGrid).Columns.Add(new MaterialDesignThemes.Wpf.DataGridTextColumn
                    {
                        Header = "Số chuyến bay",
                        Binding = new Binding("FlightCount") { Mode = BindingMode.OneTime }
                    });

                    (sender as DataGrid).Columns.Add(new MaterialDesignThemes.Wpf.DataGridTextColumn
                    {
                        Header = "Doanh thu",
                        Binding = new Binding("DisplayRevenue") { Mode = BindingMode.OneTime }
                    });

                    (sender as DataGrid).Columns.Add(new MaterialDesignThemes.Wpf.DataGridTextColumn
                    {
                        Header = "Tỉ lệ",
                        Binding = new Binding("DisplayRatio") { Mode = BindingMode.OneTime }
                    });

                    break;
                case ReportType.OneMonthOfAllYears:
                case ReportType.AllYears:
                    (sender as DataGrid).Columns.Clear();
                    (sender as DataGrid).Columns.Add(new MaterialDesignThemes.Wpf.DataGridTextColumn
                    {
                        Header = "Năm",
                        Binding = new Binding("Year") { Mode = BindingMode.OneTime }
                    });

                    (sender as DataGrid).Columns.Add(new MaterialDesignThemes.Wpf.DataGridTextColumn
                    {
                        Header = "Số chuyến bay",
                        Binding = new Binding("FlightCount") { Mode = BindingMode.OneTime }
                    });

                    (sender as DataGrid).Columns.Add(new MaterialDesignThemes.Wpf.DataGridTextColumn
                    {
                        Header = "Doanh thu",
                        Binding = new Binding("DisplayRevenue") { Mode = BindingMode.OneTime }
                    });

                    (sender as DataGrid).Columns.Add(new MaterialDesignThemes.Wpf.DataGridTextColumn
                    {
                        Header = "Tỉ lệ",
                        Binding = new Binding("DisplayRatio") { Mode = BindingMode.OneTime }
                    });

                    break;
            }

        }

        /// <summary>
        /// Open the dialog host
        /// </summary>
        public async Task OpenPrintPreview()
        {
            var view = new ReportPrintPreviewWindow();

            view.UpdateColumns();

            await DialogHost.Show(view, "RootDialog");
        }
    }
}
