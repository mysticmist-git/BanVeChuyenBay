using FlightTicketSell.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;
using System.Windows;

namespace FlightTicketSell.ViewModels
{
    public class BookViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// The number of ticket booked
        /// </summary>
        public int NumberOfTicketBooked { get; set; } = 1;

        #endregion

        #region Commands

        /// <summary>
        /// The command to continue
        /// </summary>
        public ICommand ContinueCommand { get; set; }

        /// <summary>
        /// The command which execute when view loaded
        /// </summary>
        public ICommand LoadCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public BookViewModel()
        {
            // Create commands
            ContinueCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                // Initialize customer list
                if (IoC.IoC.Get<BookDetailViewModel>().Customers.Count > 0)
                {
                    var currentCustomerCount = IoC.IoC.Get<BookDetailViewModel>().Customers.Count;

                    int diff = currentCustomerCount - NumberOfTicketBooked;

                    if (diff > 0)
                    {
                        // Information lost possible notification
                        if (MessageBox.Show("Giảm số lượng vé có thể sẽ gây mất thông tin khách hàng đã nhập, bạn có chắc muốn giảm?", "Cảnh báo mất mát thông tin", MessageBoxButton.YesNo) == MessageBoxResult.No)
                            return;

                        // Remove customers that are redundant
                        for (int i = 0; i < diff; i++)
                            IoC.IoC.Get<BookDetailViewModel>().Customers.RemoveAt(IoC.IoC.Get<BookDetailViewModel>().Customers.Count - 1);
                    }
                    else
                        // Add new customers
                        for (int i = currentCustomerCount + 1; i < NumberOfTicketBooked + 1; i++)
                            IoC.IoC.Get<BookDetailViewModel>().Customers.Add(new Customer { Index = i, });
                }
                else
                    // Initialize customer first time
                    for (int i = 1; i < NumberOfTicketBooked + 1; i++)
                        IoC.IoC.Get<BookDetailViewModel>().Customers.Add(new Customer { Index = i, });
                
                // Change to next view
                IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.BookDetail;
 
            });

            LoadCommand = new RelayCommand<object>((p) => true, (p) =>
            {
            });

        }

        #endregion
    }
}
