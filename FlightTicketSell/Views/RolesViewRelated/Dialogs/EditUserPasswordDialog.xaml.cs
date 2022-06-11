using FlightTicketSell.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FlightTicketSell.Views
{
    /// <summary>
    /// Interaction logic for EditUserPasswordDialog.xaml
    /// </summary>
    public partial class EditUserPasswordDialog : UserControl
    {
        public EditUserPasswordDialog()
        {
            InitializeComponent();
        }

        private void NewPasswordChanged(object sender, RoutedEventArgs e)
        {
            ((dynamic)this.DataContext).NewPassword = (sender as PasswordBox).SecurePassword;
        }
    }
}
