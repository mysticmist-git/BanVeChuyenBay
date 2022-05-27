using FlightTicketSell.Models;
using System.Collections.ObjectModel;
using System.Data.Entity.Core;
using System.Windows;
using System.Windows.Input;
using System.Linq;
using System.Data.Entity;
using FlightTicketSell.Models.Enums;
using System.Collections.Generic;
using FlightTicketSell.ViewModels.Search;

namespace FlightTicketSell.ViewModels
{
    /// <summary>
    /// The view model for the window showing customers information of a ticket or booking
    /// </summary>
    public class CustomersDetailViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// The ticket/reservation info using for query more information
        /// </summary>
        public object Info { get; set; }
        
        /// <summary>
        /// The customer list
        /// </summary>
        public ObservableCollection<CustomerWithIndex> Customers { get; set; }

        /// <summary>
        /// An enum to identify how this window load customer (single or multiple)
        /// </summary>
        public CustomerDetailType CustomerDetailType { get; set; } 

        #endregion

        #region Commands

        /// <summary>
        /// The command to execute on view load
        /// </summary>
        public ICommand LoadCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public CustomersDetailViewModel()
        {
            // Create commands
            LoadCommand = new RelayCommand<object>((p) => true, async (p) => 
            {
                // Get flight code from flight detail view model
                var flightCode = IoC.IoC.Get<FlightDetailViewModel>().FlightInfo.MaChuyenBay;

                // Load customer
                using (var context = new FlightTicketSellEntities())
                {
                    try
                    {
                        if (CustomerDetailType == CustomerDetailType.Ticket)
                        {
                            // Get ticket info
                            var ticketInfo = (Info as Ticket);
                            
                            // Get KHACHHANG
                            var result = await context.VEs
                                .Where(ve => ve.MaChuyenBay == flightCode && ve.MaVe == ticketInfo.MaVe)
                                .Select(ve => ve.KHACHHANG)
                                .FirstOrDefaultAsync();

                            Customers = new ObservableCollection<CustomerWithIndex>(new[] { new CustomerWithIndex(result) { Index = 1 } });
                        }
                        else
                        {
                            // Get reservation info
                            var reservationInfo = (Info as BookSearchVariant);

                            // Get KHACHHANG
                            var result = await context.DATCHOes
                                .Where(dc => dc.MaChuyenBay == flightCode && dc.MaDatCho == reservationInfo.MaDatCho)
                                .Select(dc => dc.CHITIETDATCHOes
                                    .Select(ctdc => ctdc.KHACHHANG)
                                    )
                                .FirstOrDefaultAsync();

                            // Convert KHACHHANG to Customer and add it to Customers list
                            Customers = new ObservableCollection<CustomerWithIndex>();
                            for (int i = 1; i <= result.Count(); i++)
                                Customers.Add(new CustomerWithIndex(result.ElementAt(i - 1)) { Index = i });
                        }

                    }
                    catch (EntityException e)
                    {
                        MessageBox.Show("Database access failed!", string.Format($"Exception: {e.Message}"), MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                }
            });
        }

        #endregion
    }
}
