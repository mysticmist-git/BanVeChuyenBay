using FlightTicketSell.IoC;
using FlightTicketSell.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FlightTicketSell
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Custom startup so we load IoC immediately before anything else
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Setup IoC
            IoC.IoC.Setup();

            // Show the main window
            Current.MainWindow = new LoginView();
            Current.MainWindow.Show();
        }
    }
}
