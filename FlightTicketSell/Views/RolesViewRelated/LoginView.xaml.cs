using FlightTicketSell.Interface;
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
using System.Windows.Shapes;

namespace FlightTicketSell.Views
{
    /// <summary>
    /// Interaction logic for LoginWIndow.xaml
    /// </summary>
    public partial class LoginView : Window, ILogin
    {
        public LoginView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Lock the login button
        /// </summary>
        public void LockLogin()
        {
            loginBtn.IsEnabled = false;
        }

        /// <summary>
        /// Unlock the login button
        /// </summary>
        public void UnlockLogin()
        {
            loginBtn.IsEnabled = true;
        }

        public void ShowMainWindow()
        {
            var view = new MainWindow();
            Application.Current.MainWindow = view;
            Application.Current.MainWindow.Show();
            this.Close();
        }


        private void PasswordChanged(object sender, RoutedEventArgs e)
        {
            ((dynamic)this.DataContext).Password = (sender as PasswordBox).SecurePassword;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
