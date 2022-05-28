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
using MaterialDesignThemes.Wpf;

namespace FlightTicketSell.ViewModels
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
            Open_Window_DescriptionCustomer_Command = new RelayCommand<object>((p) => true, async (p) =>
            {
                var view = new DetailCustomers()
                {
                    DataContext = new CustomersDetailViewModel()
                    {
                        CustomerDetailType = Models.Enums.CustomerDetailType.Reservation,
                        Info = (this as object)
                    }
                };
                var result = await DialogHost.Show(view, "RootDialog");

            });

            ShowMoreCommand = new RelayCommand<object>(p => true, p =>
            {
                IoC.IoC.Get<ChangeTicketViewModel>().BookingInfo = this;

                // Changes view
                IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.ChangeTicket;
            });
        } 

        #endregion
    }
}
