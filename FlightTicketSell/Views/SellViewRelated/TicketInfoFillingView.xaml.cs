using FlightTicketSell.Interface;
using FlightTicketSell.ValidateRules;
using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FlightTicketSell.Views
{
    /// <summary>
    /// Interaction logic for TicketInfoView.xaml
    /// </summary>
    public partial class TicketInfoFillingView : UserControl, ITicketInfoFilling
    {
        public TicketInfoFillingView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Indicates if all field filled
        /// </summary>
        public bool IsValid
        {
            get
            {
                if (
                   // Essential fields is null or empty
                   string.IsNullOrEmpty(tbCustomerName.Text) ||
                   string.IsNullOrEmpty(tbCustomerID.Text) ||

                   // Email in incorrect format
                   !(string.IsNullOrEmpty(tbCustomerEmail.Text) || new MailFormatRule().Validate(tbCustomerEmail.Text, null).IsValid) ||

                   // Ticket tier is not selected
                   dgTicketTier.SelectedItem is null
               )
                    return false;

                return true;
            }
        }

        /// <summary>
        /// Force field to update source
        /// </summary>
        public void ForceUpdateSource()
        {
            tbCustomerName.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            tbCustomerID.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            dgTicketTier.GetBindingExpression(DataGrid.SelectedItemProperty).UpdateSource();
        }

        /// <summary>
        /// Update source on lost focus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            (sender as TextBox).GetBindingExpression(TextBox.TextProperty).UpdateSource();
        }

        /// <summary>
        /// Make sure it's number entered
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private new void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllow(e.Text);
        }

        /// <summary>
        /// Indicate if text is all number
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private bool IsTextAllow(string text)
        {
            Regex _regex = new Regex("[^0-9]");
            
            return !_regex.IsMatch(text);
        }
    }
}
