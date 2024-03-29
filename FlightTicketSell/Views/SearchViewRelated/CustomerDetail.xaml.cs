﻿using FlightTicketSell.Interface;
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

namespace FlightTicketSell.Views
{
    /// <summary>
    /// Interaction logic for DetailCustomers.xaml
    /// </summary>
    public partial class DetailCustomers : UserControl, ICustomerDetail
    {
        public DetailCustomers()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Close this window
        /// </summary>
        public void Close()
        {
            DialogHost.CloseDialogCommand.Execute(null, null);
        }
    }
}
