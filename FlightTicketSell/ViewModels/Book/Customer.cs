﻿using FlightTicketSell.Models;
using FlightTicketSell.Views;
using MaterialDesignThemes.Wpf;
using System;
using System.Windows;
using System.Windows.Input;

namespace FlightTicketSell.ViewModels
{
    /// <summary>
    /// A specialize version of <see cref="KHACHHANG"/> for book view
    /// </summary>
    public class Customer : KHACHHANG
    {
        /// <summary>
        /// The index of the customer in the list
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Name buffer
        /// </summary>
        public string NameBuffer { get; set; }
        public string IDBuffer { get; set; }
        public string PhoneNumBuffer { get; set; }
        public string EmailBuffer { get; set; }

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


        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public Customer()
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

                var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);
            });

            // Command to remove customer info
            RemoveCustomerInfoCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                // Safe guarantee
                var result = MessageBox.Show("Bạn có chắc muốn xóa khách hàng này?", "Xóa khách hàng", MessageBoxButton.YesNo, MessageBoxImage.Question);

                // Return if it say no
                if (result == MessageBoxResult.No)
                    return;

                // Update index of every one after this removing customer info
                for (int i = Index; i < IoC.IoC.Get<BookDetailViewModel>().Customers.Count; i++)
                    IoC.IoC.Get<BookDetailViewModel>().Customers[i].Index--;

                // Remove customer info from list in the book detial view model
                IoC.IoC.Get<BookDetailViewModel>().Customers.RemoveAt(Index - 1);

            });

            SaveCustomerCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                // Update information
                HoTen = NameBuffer;
                CMND = IDBuffer;
                SDT = PhoneNumBuffer;
                Email = EmailBuffer;

                // Close dialog
                DialogHost.CloseDialogCommand.Execute(1, null);
            });
        }

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
                    NameBuffer==HoTen &&
                    IDBuffer ==CMND &&
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