using FlightTicketSell.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;

namespace FlightTicketSell.ViewModels
{
    public class BookViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// The number of ticket booked
        /// </summary>
        public int NumberOfTicketBooked { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// The command to continue
        /// </summary>
        public ICommand ContinueCommand { get; set; }

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
                for (int i = 0; i < NumberOfTicketBooked; i++)
                    IoC.IoC.Get<BookDetailViewModel>().Customers.Add
                    (
                        new Customer
                        {
                            Index = i + 1,
                            HoTen = $"Tên khách hàng " + ("00" + (i + 1).ToString()) .Substring( ("00" + (i + 1).ToString()).Length - 2, 2 )
                        }
                    );
                
                // Change to next view
                IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.BookDetail;
 
            });

        }

        #endregion
    }
}
