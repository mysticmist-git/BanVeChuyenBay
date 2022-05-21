using FlightTicketSell.Models;
using FlightTicketSell.Views;
using MaterialDesignThemes.Wpf;
using System;
using System.Windows;
using System.Windows.Input;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Core;

namespace FlightTicketSell.ViewModels
{
    /// <summary>
    /// A customer used in book view, which has some buffers, and commands
    /// </summary>
    public class CustomerBookVariant : CustomerWithIndex
    {
        #region Public Properties

        /// <summary>
        /// Indicates if this customer can be remove from this list
        /// </summary>
        public bool IsBookingCustomer { get; set; } = false;

        /// <summary>
        /// Indicates if all info is filled orn ot 
        /// </summary>
        public bool IsNoInfoFilled
        {
            get =>
                string.IsNullOrEmpty(HoTen) &&
                string.IsNullOrEmpty(CMND) &&
                string.IsNullOrEmpty(SDT) &&
                string.IsNullOrEmpty(Email);
        }

        public bool IsEssensialInfoNoFilled
        {
            get =>
                string.IsNullOrEmpty(HoTen) ||
                string.IsNullOrEmpty(CMND);
        }

        /// <summary>
        /// The display index of the customer
        /// </summary>
        public string DisplayIndex { get => "Khách hàng " + Index; }

        /// <summary>
        /// The name buffer
        /// </summary>
        public string NameBuffer { get; set; }

        /// <summary>
        /// The ID buffer
        /// </summary>
        public string IDBuffer { get; set; }

        /// <summary>
        /// The phone number buffer
        /// </summary>
        public string PhoneNumBuffer { get; set; }

        /// <summary>
        /// The email buffer
        /// </summary>
        public string EmailBuffer { get; set; }

        #region Duplicated Customer

        /// <summary>
        /// Indicates if the customer filled is already in database or not
        /// </summary>
        public bool IsCustomerNew { get; set; }


        /// <summary>
        /// The duplicated customer (if there is)
        /// </summary>
        public Customer DuplicatedCustomer { get; set; }

        #endregion

        #endregion

        #region Commands

        /// <summary>
        /// Save customer information
        /// </summary>
        public ICommand SaveCustomerCommand { get; set; }

        /// <summary>
        /// Execute when dialog load
        /// </summary>
        public ICommand LoadCommand { get; set; }

        /// <summary>
        /// Open customer info dialog
        /// </summary>
        public ICommand OpenCustomerInfoCommand { get; set; }

        /// <summary>
        /// Command to remove customer info
        /// </summary>
        public ICommand RemoveCustomerInfoCommand { get; set; }

        /// <summary>
        /// Check the customer ID if it is already existed
        /// </summary>
        public ICommand CustomerIDCheck { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Copy constructor
        /// </summary>
        public CustomerBookVariant(KHACHHANG kh) : base(kh) => InitializeCommands();

        /// <summary>
        /// Default constructor
        /// </summary>
        public CustomerBookVariant() => InitializeCommands();

        #endregion

        #region Private Methods

        /// <summary>
        /// The method to initialize commands of this view model
        /// </summary>
        private void InitializeCommands()
        {
            // Create commands
            OpenCustomerInfoCommand = new RelayCommand<object>((p) => true, async (p) =>
            {
                var view = new CustomerInfoDialog
                {
                    DataContext = this
                };

                // Load current information to buffers
                NameBuffer = HoTen;
                IDBuffer = CMND;
                PhoneNumBuffer = SDT;
                EmailBuffer = Email;

                await DialogHost.Show(view, "RootDialog", ClosingEventHandler);
            });

            // Command to remove customer info
            RemoveCustomerInfoCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                if (!IsNoInfoFilled)
                {
                    // Safe guarantee
                    var result = MessageBox.Show("Bạn có chắc muốn xóa khách hàng này?", "Xóa khách hàng", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    // Return if it say no
                    if (result == MessageBoxResult.No)
                        return;
                }

                // Update index of every one after this removing customer info
                for (int i = Index; i < IoC.IoC.Get<BookDetailViewModel>().Customers.Count; i++)
                    IoC.IoC.Get<BookDetailViewModel>().Customers[i].Index--;

                // Remove customer info from list in the book detial view model
                IoC.IoC.Get<BookDetailViewModel>().Customers.RemoveAt(Index - 1);

            });

            SaveCustomerCommand = new RelayCommand<object>((p) => true, async (p) =>
            {
                /**
                 * Check customer info if it is existed in the either the customer list or in the database (its CMND / ID)
                 */

                // Check if it already existed in the client customer list
                DuplicatedCustomer = IoC.IoC.Get<BookDetailViewModel>().IsCustomerExisted(IDBuffer);

                if (DuplicatedCustomer != null)
                {
                    // Show dialog yêu cầu có hành động đối với CMND trùng
                    do
                    {
                        // It's ok if all fields is dupped
                        if (
                            DuplicatedCustomer.HoTen == NameBuffer &&
                            DuplicatedCustomer.SDT == PhoneNumBuffer &&
                            DuplicatedCustomer.Email == EmailBuffer
                            )
                            break;

                        MessageBox.Show(
                            "CMND bạn vừa nhập đã trùng với khách hàng này trong danh sách đặt chỗ, xin nhập lại!\n" +
                            $"Họ và tên: " + DuplicatedCustomer.HoTen + "\n" +
                            $"CMND: " + DuplicatedCustomer.CMND + "\n" +
                            $"Số điện thoại: " + DuplicatedCustomer.SDT + "\n" +
                            $"Email: " + DuplicatedCustomer.HoTen + "\n"
                            ,

                            "Trùng khách hàng", MessageBoxButton.OK, MessageBoxImage.Question);

                        return;

                    } while (false);
                }

                // Check if customer existed in database
                using (var context = new FlightTicketSellEntities())
                {
                    try
                    {
                        IsCustomerNew = !(await context.KHACHHANGs.Where(kh => kh.CMND == IDBuffer).CountAsync() > 0);
                    }
                    catch (EntityException e)
                    {
                        MessageBox.Show("Database access failed!", string.Format($"Exception: {e.Message}"), MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }


                do
                {
                    if (!IsCustomerNew)
                    {
                        // Get existed customer
                        using (var context = new FlightTicketSellEntities())
                        {
                            var customer = await context.KHACHHANGs.Where(kh => kh.CMND == IDBuffer).FirstOrDefaultAsync();
                            DuplicatedCustomer = new Customer
                            {
                                HoTen = customer.HoTen,
                                SDT = customer.SDT,
                                CMND = customer.CMND,
                                Email = customer.Email
                            };
                        }

                        // It's ok if all fields is dupped
                        if (
                            DuplicatedCustomer.HoTen == NameBuffer &&
                            DuplicatedCustomer.SDT == PhoneNumBuffer &&
                            DuplicatedCustomer.Email == EmailBuffer
                            )
                            break;

                        var result = MessageBox.Show(
                            "CMND bạn vừa nhập đã trùng với khách hàng này trong hệ thống, bạn có muốn dùng lại thông tin của khách hàng này?\n" +
                            $"Họ và tên: " + DuplicatedCustomer.HoTen + "\n" +
                            $"CMND: " + DuplicatedCustomer.CMND + "\n" +
                            $"Số điện thoại: " + DuplicatedCustomer.SDT + "\n" +
                            $"Email: " + DuplicatedCustomer.HoTen + "\n"
                            ,

                            "Trùng khách hàng", MessageBoxButton.YesNo, MessageBoxImage.Question);

                        if (result == MessageBoxResult.Yes)
                        {
                            NameBuffer = DuplicatedCustomer.HoTen;
                            IDBuffer = DuplicatedCustomer.CMND;
                            PhoneNumBuffer = DuplicatedCustomer.SDT;
                            EmailBuffer = DuplicatedCustomer.Email;
                        }
                        else
                            return;
                    }
                } while (false);

                // Update information
                HoTen = NameBuffer;
                CMND = IDBuffer;
                SDT = PhoneNumBuffer;
                Email = EmailBuffer;

                // Close dialog
                DialogHost.CloseDialogCommand.Execute(1, null);

                OnPropertyChanged(nameof(IsEssensialInfoNoFilled));
            });

            CustomerIDCheck = new RelayCommand<object>((p) => true, async (p) =>
            {
                /**
                 * Check customer info if it is existed in the either the customer list or in the database (its CMND / ID)
                 */

                // Check if it already existed in the client customer list
                DuplicatedCustomer = IoC.IoC.Get<BookDetailViewModel>().IsCustomerExisted(IDBuffer);

                if (DuplicatedCustomer != null)
                {
                    // Show dialog yêu cầu có hành động đối với CMND trùng
                    do
                    {
                        // It's ok if all fields is dupped
                        if (
                            DuplicatedCustomer.HoTen == NameBuffer &&
                            DuplicatedCustomer.SDT == PhoneNumBuffer &&
                            DuplicatedCustomer.Email == EmailBuffer
                            )
                            break;

                        MessageBox.Show(
                            "CMND bạn vừa nhập đã trùng với khách hàng này trong danh sách đặt chỗ, xin nhập lại!\n" +
                            $"Họ và tên: " + DuplicatedCustomer.HoTen + "\n" +
                            $"CMND: " + DuplicatedCustomer.CMND + "\n" +
                            $"Số điện thoại: " + DuplicatedCustomer.SDT + "\n" +
                            $"Email: " + DuplicatedCustomer.HoTen + "\n"
                            ,

                            "Trùng khách hàng", MessageBoxButton.OK, MessageBoxImage.Question);

                        return;

                    } while (false);
                }

                // Check if customer existed in database
                using (var context = new FlightTicketSellEntities())
                {
                    try
                    {
                        IsCustomerNew = !(await context.KHACHHANGs.Where(kh => kh.CMND == IDBuffer).CountAsync() > 0);
                    }
                    catch (EntityException e)
                    {
                        MessageBox.Show("Database access failed!", string.Format($"Exception: {e.Message}"), MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }


                do
                {
                    if (!IsCustomerNew)
                    {
                        // Get existed customer
                        using (var context = new FlightTicketSellEntities())
                        {
                            var customer = await context.KHACHHANGs.Where(kh => kh.CMND == IDBuffer).FirstOrDefaultAsync();
                            DuplicatedCustomer = new Customer
                            {
                                HoTen = customer.HoTen,
                                SDT = customer.SDT,
                                CMND = customer.CMND,
                                Email = customer.Email
                            };
                        }

                        // It's ok if all fields is dupped
                        if (
                            DuplicatedCustomer.HoTen == NameBuffer &&
                            DuplicatedCustomer.SDT == PhoneNumBuffer &&
                            DuplicatedCustomer.Email == EmailBuffer
                            )
                            break;

                        var result = MessageBox.Show(
                            "CMND bạn vừa nhập đã trùng với khách hàng này trong hệ thống, bạn có muốn dùng lại thông tin của khách hàng này?\n" +
                            $"Họ và tên: " + DuplicatedCustomer.HoTen + "\n" +
                            $"CMND: " + DuplicatedCustomer.CMND + "\n" +
                            $"Số điện thoại: " + DuplicatedCustomer.SDT + "\n" +
                            $"Email: " + DuplicatedCustomer.HoTen + "\n"
                            ,

                            "Trùng khách hàng", MessageBoxButton.YesNo, MessageBoxImage.Question);

                        if (result == MessageBoxResult.Yes)
                        {
                            NameBuffer = DuplicatedCustomer.HoTen;
                            IDBuffer = DuplicatedCustomer.CMND;
                            PhoneNumBuffer = DuplicatedCustomer.SDT;
                            EmailBuffer = DuplicatedCustomer.Email;
                        }
                        else
                            return;
                    }
                } while (false);
            });
        }


        #endregion

        #region Handlers

        /// <summary>
        /// What to do when dialog closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void ClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if (
                Convert.ToInt32(eventArgs.Parameter) == 1 ||
                (
                    NameBuffer == HoTen &&
                    IDBuffer == CMND &&
                    PhoneNumBuffer == SDT &&
                    EmailBuffer == Email
                )
            )
                return;

            var result = MessageBox.Show("Bạn có muốn lưu thay đổi", "Lưu thay đổi", MessageBoxButton.YesNoCancel);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    SaveCustomerCommand.Execute(null);
                    break;
                case MessageBoxResult.No:
                    break;
                case MessageBoxResult.Cancel:
                    eventArgs.Cancel();
                    break;
            }
        }

        #endregion
    }
}
