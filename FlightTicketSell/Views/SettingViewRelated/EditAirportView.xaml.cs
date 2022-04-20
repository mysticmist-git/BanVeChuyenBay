using FlightTicketSell.Models;
using FlightTicketSell.ViewModels.Setting;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
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

namespace FlightTicketSell.Views.SettingViewRelated
{
    /// <summary>
    /// Interaction logic for EditAirportView.xaml
    /// </summary>
    public partial class EditAirportView : Window
    {
        public EditAirportView()
        {
            InitializeComponent();
        }

        public EditAirportView(Airport sanbay)
        {
            InitializeComponent();
            CodeTextblock.Text = sanbay.Code;
            ProvinceTextblock.Text = sanbay.Province;
            NameTextblock.Text = sanbay.Name;
        }
    }
}
