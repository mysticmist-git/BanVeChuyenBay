using FlightTicketSell.Interface;
using FlightTicketSell.ValidateRules;
using FlightTicketSell.ViewModels;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FlightTicketSell.Views
{
    /// <summary>
    /// Interaction logic for BookDetailView.xaml
    /// </summary>
    public partial class BookDetailView : UserControl, IBookDetail
    {
        public BookDetailView()
        {
            InitializeComponent();
        }

        // <summary>
        /// Indicates if all field filled
        /// </summary>
        public bool IsValid
        {
            get
            {
                if (
                   // ====== Check book person infomation

                   // Essensial fields
                   string.IsNullOrEmpty(tbCustomerName.Text) ||
                   string.IsNullOrEmpty(tbCustomerID.Text) ||
                   // Email in incorrect format
                   !(string.IsNullOrEmpty(tbCustomerEmail.Text) || new MailFormatRule().Validate(tbCustomerEmail.Text, null).IsValid)
                )
                    return false;

                // ====== Check customer list information
                foreach (var customer in (DataContext as BookDetailViewModel).Customers)
                {
                    if (customer.IsEssensialInfoNoFilled)
                        return false;
                }

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

        #region Booking customer binding 

        // TODO: Weird, really gross

        /// <summary>
        /// Update the name of the booking customer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BookingCustomerInfo_Changed(object sender, TextChangedEventArgs e)
        {
            if ((DataContext as BookDetailViewModel).IsBookingCustomerIncluded)
            {
                (DataContext as BookDetailViewModel).Customers[0].HoTen = tbCustomerName?.Text;
                (DataContext as BookDetailViewModel).Customers[0].CMND = tbCustomerID?.Text;
                (DataContext as BookDetailViewModel).Customers[0].SDT = tbCustomerPhoneNumber?.Text;
                (DataContext as BookDetailViewModel).Customers[0].Email = tbCustomerEmail?.Text;

                // TODO: Rất tà đạo
                (DataContext as BookDetailViewModel).Customers[0].OnPropertyChanged("IsEssensialInfoNoFilled");
            }
        } 

        #endregion
    }
}
