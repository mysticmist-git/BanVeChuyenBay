﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for EditEnteredTicketClassView.xaml
    /// </summary>
    public partial class EditEnteredTicketClassView : UserControl
    {
        public EditEnteredTicketClassView()
        {
            InitializeComponent();
        }
        //Không cho nhập chữ
        private new void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
