using System.Linq;
using System.Windows;
using System.Windows.Input;
using FlightTicketSell.Models;
using System.Data.Entity.Core;
using FlightTicketSell.Models.SearchRelated;
using System.Collections.ObjectModel;
using FlightTicketSell.Interface;
using System.Data.Entity;
using MaterialDesignThemes.Wpf;
using System;
using FlightTicketSell.Views;

namespace FlightTicketSell.ViewModels
{
    public class TicketInfoFillingViewModel : BaseViewModel
    {
        #region Commands

        /// <summary>
        /// The command to continue
        /// </summary>
        public ICommand ContinueCommand { get; set; }

        /// <summary>
        /// Return to flight detail view
        /// </summary>
        public ICommand ReturnCommand { get; set; }

        /// <summary>
        /// The command execute on firstload
        /// </summary>
        public ICommand LoadCommand { get; set; }

        #endregion

        #region Public Properties

        /// <summary>
        /// Stores ticket tier list
        /// </summary>
        public ObservableCollection<TicketTier> TicketTiers { get => IoC.IoC.Get<FlightDetailViewModel>().TicketTier; }

        /// <summary>
        /// Stores current selected ticket tier
        /// </summary>
        public TicketTier CurrentTicketTier { get; set; }

        /// <summary>
        /// Flight infos
        /// </summary>
        public DetailFlilghtInfo FlightInfo { get => IoC.IoC.Get<FlightDetailViewModel>().FlightInfo; }

        /// <summary>
        /// Stores customer information
        /// </summary>
        public KHACHHANG Customer { get; set; } = new KHACHHANG();

        /// <summary>
        /// Indicates if the customer filled is already in database or not
        /// </summary>
        public bool IsCustomerNew { get; set; }

        /// <summary>
        /// The duplicated customer (if there is)
        /// </summary>
        public KHACHHANG DuplicatedCustomer { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public TicketInfoFillingViewModel()
        {
            // Create commands
            ContinueCommand = new RelayCommand<ITicketInfoFilling>((p) => true, async (p) =>
            {
                // Check if all field filled
                if (!p.IsValid)
                {
                    p.ForceUpdateSource();
                    return;
                }

                // Check if customer existed
                using (var context = new FlightTicketSellEntities())
                {
                    try
                    {
                        IsCustomerNew = !(await context.KHACHHANGs.Where(kh => kh.CMND == Customer.CMND).CountAsync() > 0);
                    }
                    catch (EntityException e)
                    {
                        MessageBox.Show("Database access failed!", string.Format($"Exception: {e.Message}"), MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

                // Show dialog yêu cầu có hành động đối với CMND trùng
                do
                {
                    if (!IsCustomerNew)
                    {
                        // Get existed customer
                        using (var context = new FlightTicketSellEntities())
                            DuplicatedCustomer = await context.KHACHHANGs.Where(kh => kh.CMND == Customer.CMND).FirstOrDefaultAsync();

                        var view = new CustomerExistedDialog()
                        {
                            DataContext = new CustomerExistedViewModel()
                        };

                        // It's ok if all fields is dupped
                        if (
                            DuplicatedCustomer.HoTen == Customer.HoTen &&
                            DuplicatedCustomer.SDT == Customer.SDT &&
                            DuplicatedCustomer.Email == Customer.Email
                            )
                            break;

                        await DialogHost.Show(view, "RootDialog", ClosingEventHandler);

                        return;
                    } 
                } while (false);

                // Navigate to next view
                IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.SellPay;
            });

            LoadCommand = new RelayCommand<object>(p => true, p =>
            {
                if (CurrentTicketTier is null)
                    CurrentTicketTier = TicketTiers.ElementAt(0);
            });

            ReturnCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                IoC.IoC.Rebind<TicketInfoFillingView>();
                
                IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.FlightDetail;
            });
        }

        #endregion

        #region Helpers

        /// <summary>
        /// What to do when dialog close
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void ClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if (eventArgs is null)
                return;

            switch ((int)eventArgs.Parameter)
            {
                case 0:
                    return;
                case 1:
                    Customer.HoTen = DuplicatedCustomer.HoTen;
                    Customer.CMND = DuplicatedCustomer.CMND;
                    Customer.SDT = DuplicatedCustomer.SDT;
                    Customer.Email = DuplicatedCustomer.Email;
                    return;
            }
        }

        /// <summary>
        /// Clear this view information
        /// </summary>
        public void ClearData()
        {
            Customer = new KHACHHANG();
            DuplicatedCustomer = new KHACHHANG();
            CurrentTicketTier = null;
        }

        #endregion

    }
}
