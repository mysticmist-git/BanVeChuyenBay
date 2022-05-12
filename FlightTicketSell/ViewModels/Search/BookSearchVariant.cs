using System;
using System.Windows.Input;
using FlightTicketSell.Views.Helper;
using System.Globalization;
using FlightTicketSell.ViewModels;
using FlightTicketSell.Models.Enums;
using System.Windows;
using FlightTicketSell.Models;
using FlightTicketSell.Helpers;
using FlightTicketSell.Views;

namespace FlightTicketSell.ViewModels.Search
{
    public class BookSearchVariant : Book
    {
        #region Commands

        /// <summary>
        /// The command to open the detail description customer widow
        /// </summary>
        public ICommand Open_Window_DescriptionCustomer_Command { get; set; }
        
        /// <summary>
        /// The command to show more information about the booking
        /// </summary>
        public ICommand ShowMoreCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public BookSearchVariant()
        {
            // Create commands 
            Open_Window_DescriptionCustomer_Command = new RelayCommand<object>((p) => true, (p) =>
            {
                var view = new DetailCustomers()
                {
                    DataContext = new CustomersDetailViewModel()
                    {
                        CustomerDetailType = Models.Enums.CustomerDetailType.Reservation,
                        Info = (this as object)
                    }
                };

                view.ShowDialog();
            });

            ShowMoreCommand = new RelayCommand<object>(p => true, p =>
            {
                // Changes view
                IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.ChangeTicket;

                IoC.IoC.Get<ChangeTicketViewModel>().BookingInfo = this;
            });
        } 

        #endregion
    }
}
