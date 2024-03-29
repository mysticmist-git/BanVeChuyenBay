﻿using FlightTicketSell.Models;
using FlightTicketSell.Models.SearchRelated;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;
using FlightTicketSell.Interface;
using System.Windows;
using System.Data.Entity.Core;
using System.Data.Entity;
using FlightTicketSell.Views;
using MaterialDesignThemes.Wpf;
using System;
using System.Windows.Controls;
using FlightTicketSell.Helpers;

namespace FlightTicketSell.ViewModels
{
    public class BookDetailViewModel : BaseViewModel
    {
        #region Interface

        /// <summary>
        /// The book detail interface
        /// </summary>
        public IBookDetail BookDetailInterface { get; set; }

        #endregion

        #region Private Members

        /// <summary>
        /// Indicates if this is the first time view model loads
        /// </summary>
        private bool _firstLoad = true;

        /// <summary>
        /// Indicates if this list included the booking person
        /// </summary>
        private bool _isBookingCustomerIncluded = true;

        #endregion

        #region Public Properties        

        /// <summary>
        /// Stores flight info
        /// </summary>
        public FlightInfo FlightInfo { get => IoC.IoC.Get<FlightDetailViewModel>().FlightInfo; }

        /// <summary>
        /// Stores ticket tiers info
        /// </summary>
        public ObservableCollection<TicketTier> TicketTiers { get => IoC.IoC.Get<FlightDetailViewModel>().TicketTier; }
        public ObservableCollection<TicketTier> ProcessedTicketTiers { get; set; }

        /// <summary>
        /// Stores current ticket tier being selected
        /// </summary>
        public TicketTier CurrentTicketTier { get; set; }

        /// <summary>
        /// Store booking info
        /// </summary>
        public Book BookInfo { get; set; } = new Book();

        /// <summary>
        /// The customer list that we'll fill information
        /// </summary>
        public ObservableCollection<CustomerBookVariant> Customers { get; set; } = new ObservableCollection<CustomerBookVariant>();

        /// <summary>
        /// Indicates if this list included the booking person
        /// </summary>
        public bool IsBookingCustomerIncluded
        {
            get => _isBookingCustomerIncluded;
            set
            {
                _isBookingCustomerIncluded = value;

                ToggleIncludeBookingCustomer();
            }
        }

        /// <summary>
        /// Indicates if the customer filled is already in database or not
        /// </summary>
        public bool IsCustomerNew { get; set; }

        /// <summary>
        /// The duplicated customer (if there is)
        /// </summary>
        public Customer DuplicatedCustomer { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// The command to return to previous view
        /// </summary>
        public ICommand ReturnCommand { get; set; }

        /// <summary>
        /// The command to continue
        /// </summary>
        public ICommand ContinueCommand { get; set; }

        /// <summary>
        /// The command which is execute when user control load
        /// </summary>
        public ICommand LoadCommand { get; set; }

        /// <summary>
        /// Add a new customer to book list
        /// </summary>
        public ICommand AddCustomerCommand { get; set; }

        /// <summary>
        /// Check the customer ID if it is already existed
        /// </summary>
        public ICommand CustomerIDCheck { get; set; }

        /// <summary>
        /// Executes when ticket tier change
        /// </summary>
        public ActionCommand<SelectionChangedEventArgs> CurrentTicketTierChangedCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Defualt constuctor
        /// </summary>
        public BookDetailViewModel()
        {
            // Create commands
            LoadCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                // Initializes
                if (_firstLoad)
                    ToggleIncludeBookingCustomer();

                ProcessedTicketTiers = new ObservableCollection<TicketTier>(TicketTiers.Where(tt => tt.GheTrong > 0).ToList());

                if (CurrentTicketTier is null)
                {
                    CurrentTicketTier = ProcessedTicketTiers.ElementAt(0);
                }

                _firstLoad = false;
            });

            // Create command
            ContinueCommand = new RelayCommand<object>((p) => true, async (p) =>
             {
                 // Check if all field filled
                 if (!((IBookDetail)p).IsValid)
                 {
                     ((IBookDetail)p).ForceUpdateSource();
                     MessageBox.Show("Vui lòng nhập đủ thông tin người đặt / người thụ hưởng!", "Thiếu thông tin", MessageBoxButton.OK, MessageBoxImage.Information);
                     return;
                 }

                 // Check if customer list is not empty
                 if (Customers.Count < 1)
                 {
                     MessageBox.Show("Phiếu đặt chỗ phải có ít nhất một khách hàng!", "Thiếu thông tin", MessageBoxButton.OK, MessageBoxImage.Information);
                     return;
                 }

                 /**
                  * Check if customer existed
                  * Only check the booking one, other customer will be check when its info is filled
                  */
                 using (var context = new FlightTicketSellEntities())
                 {
                     try
                     {
                         IsCustomerNew = !(await context.KHACHHANGs.Where(kh => kh.CMND == BookInfo.ThongTinNguoiDat.CMND).CountAsync() > 0);
                     }
                     catch (EntityException e)
                     {
                         NotifyHelper.ShowEntityException(e);
                     }
                 }

                 // Show dialog yêu cầu có hành động đối với CMND trùng
                 do
                 {
                     if (!IsCustomerNew)
                     {
                         // Get existed customer
                         using (var context = new FlightTicketSellEntities())
                         {
                             var customer = await context.KHACHHANGs.Where(kh => kh.CMND == BookInfo.ThongTinNguoiDat.CMND).FirstOrDefaultAsync();
                             DuplicatedCustomer = new Customer
                             {
                                 HoTen = customer.HoTen,
                                 SDT = customer.SDT,
                                 CMND = customer.CMND,
                                 Email = customer.Email
                             };
                         }

                         var view = new CustomerExistedDialog()
                         {
                             DataContext = new CustomerExistedViewModel()
                             {
                                 DuplicatedCustomer = this.DuplicatedCustomer
                             }
                         };

                         // It's ok if all fields is dupped
                         if (
                             DuplicatedCustomer.HoTen == BookInfo.ThongTinNguoiDat.HoTen &&
                             DuplicatedCustomer.SDT == BookInfo.ThongTinNguoiDat.SDT &&
                             DuplicatedCustomer.Email == BookInfo.ThongTinNguoiDat.Email
                             )
                             break;

                         if (!DialogHost.IsDialogOpen("RootDialog"))
                             await DialogHost.Show(view, "RootDialog", ClosingEventHandler);

                         return;
                     }
                 } while (false);

                 // Change view
                 IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.BookPay;

                 // Update Book Information (SoVeDat)
                 BookInfo.SoVeDat = Customers.Count;
                 BookInfo.GiaTien = CurrentTicketTier.GiaTien * Customers.Count;
             });

            ReturnCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.FlightDetail;

                IoC.IoC.Rebind<BookDetailViewModel>();
            });

            AddCustomerCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                // Check if there's enought seat left
                //if (FlightInfo.GheTrong == Customers.Count)
                if (CurrentTicketTier.GheTrong == Customers.Count)
                {
                    MessageBox.Show("Đã hết ghế trống!", "Hết ghế trống", MessageBoxButton.OK);
                    return;
                }

                // Add a new customer
                Customers.Add(new CustomerBookVariant { Index = Customers.Count + 1 });
            });

            CustomerIDCheck = new RelayCommand<object>((p) => true, async (p) =>
            {
                // Check if customer existed
                using (var context = new FlightTicketSellEntities())
                {
                    try
                    {
                        IsCustomerNew = !(await context.KHACHHANGs.Where(kh => kh.CMND == BookInfo.ThongTinNguoiDat.CMND).CountAsync() > 0);
                    }
                    catch (EntityException e)
                    {
                        NotifyHelper.ShowEntityException(e);;
                    }
                }

                // Show dialog yêu cầu có hành động đối với CMND trùng
                do
                {
                    if (!IsCustomerNew)
                    {
                        // Get existed customer
                        using (var context = new FlightTicketSellEntities())
                        {
                            var customer = await context.KHACHHANGs.Where(kh => kh.CMND == BookInfo.ThongTinNguoiDat.CMND).FirstOrDefaultAsync();
                            DuplicatedCustomer = new Customer
                            {
                                HoTen = customer.HoTen,
                                SDT = customer.SDT,
                                CMND = customer.CMND,
                                Email = customer.Email
                            };

                        }

                        var view = new CustomerExistedDialog()
                        {
                            DataContext = new CustomerExistedViewModel()
                            {
                                DuplicatedCustomer = this.DuplicatedCustomer
                            }
                        };

                        // It's ok if all fields is dupped
                        if (
                            DuplicatedCustomer.HoTen == BookInfo.ThongTinNguoiDat.HoTen &&
                            DuplicatedCustomer.SDT == BookInfo.ThongTinNguoiDat.SDT &&
                            DuplicatedCustomer.Email == BookInfo.ThongTinNguoiDat.Email
                            )
                            break;

                        if (!DialogHost.IsDialogOpen("RootDialog"))
                            await DialogHost.Show(view, "RootDialog", ClosingEventHandler);

                        return;
                    }
                } while (false);
            });

            CurrentTicketTierChangedCommand = new ActionCommand<SelectionChangedEventArgs>(args =>
            {
                if (CurrentTicketTier is null) return;

                if (CurrentTicketTier.GheTrong < Customers.Count)
                {
                    MessageBox.Show(String.Format(
                        $"Hạng vé này chỉ có {CurrentTicketTier.GheTrong} ghế trống, trong khi có {Customers.Count} khách thụ hưởng.\n" +
                        "Xin điều chỉnh danh sách khách hàng."),
                        "Quá số ghế trống hiện có",
                        MessageBoxButton.OK
                        );
                    // TODO: Really gross, change if you can
                    var temp = args.RemovedItems.Count > 0 ? args.RemovedItems[0] as TicketTier : TicketTiers[0];
                    (args.Source as DataGrid).ItemsSource = null;
                    (args.Source as DataGrid).ItemsSource = TicketTiers;
                    CurrentTicketTier = temp;
                }
            });
        }

        #endregion

        #region Methods

        /// <summary>
        /// Includes, or excluds the booking customer in the customer list
        /// </summary>
        private void ToggleIncludeBookingCustomer()
        {
            if (IsBookingCustomerIncluded)
            {
                // Check if there's enought seat left
                if (FlightInfo.GheTrong == Customers.Count)
                {
                    MessageBox.Show("Đã hết ghế trống!", "Hết ghế trống", MessageBoxButton.OK);
                    IsBookingCustomerIncluded = false;
                    return;
                }

                var bookingCustomer = new CustomerBookVariant()
                {
                    Index = 1,
                    HoTen = BookInfo.ThongTinNguoiDat.HoTen,
                    CMND = BookInfo.ThongTinNguoiDat.CMND,
                    SDT = BookInfo.ThongTinNguoiDat.SDT,
                    Email = BookInfo.ThongTinNguoiDat.Email,
                    IsBookingCustomer = true
                };

                Customers.Insert(0, bookingCustomer);

                // Re-Indexing
                for (int i = 1; i <= Customers.Count; i++)
                    Customers[i - 1].Index = i;
            }
            else
            {
                if (Customers[0].IsBookingCustomer == false)
                    return;

                Customers.RemoveAt(0);

                // Re-Indexing
                for (int i = 1; i <= Customers.Count; i++)
                    Customers[i - 1].Index = i;
            }
        }

        /// <summary>
        /// Clear this view information
        /// </summary>
        public void ClearData()
        {
            BookInfo.ThongTinNguoiDat = new Customer();
            CurrentTicketTier = null;
            Customers = new ObservableCollection<CustomerBookVariant>();
        }

        /// <summary>
        /// Check if the cmnd passed in already existed in the customer list
        /// </summary>
        /// <param name="iDBuffer"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public CustomerBookVariant IsCustomerExisted(string iDBuffer)
        {
            foreach (var customer in Customers)
                if (customer.CMND == iDBuffer)
                    return customer;

            return null;
        }

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
                    BookInfo.ThongTinNguoiDat.HoTen = DuplicatedCustomer.HoTen;
                    BookInfo.ThongTinNguoiDat.CMND = DuplicatedCustomer.CMND;
                    BookInfo.ThongTinNguoiDat.SDT = DuplicatedCustomer.SDT;
                    BookInfo.ThongTinNguoiDat.Email = DuplicatedCustomer.Email;
                    return;
            }
        }

        #endregion

    }
}
