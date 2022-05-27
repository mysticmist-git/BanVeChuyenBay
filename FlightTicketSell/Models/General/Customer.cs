using FlightTicketSell.Models;
using FlightTicketSell.ViewModels;
using FlightTicketSell.Views;
using MaterialDesignThemes.Wpf;
using System;
using System.Windows;
using System.Windows.Input;

namespace FlightTicketSell.Models
{
    /// <summary>
    /// A specialized version of <see cref="KHACHHANG"/> for book view
    /// </summary>
    public class Customer : BaseViewModel
    {
        #region Public Properties

        public int MaKhachHang { get; set; }

        public string HoTen { get; set; }

        public string CMND { get; set; }

        public string SDT { get; set; }

        public string Email { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Copy constructor
        /// </summary>
        public Customer(KHACHHANG khachHang) : this()
        {
            if (khachHang is null)
                return;

            // Copy information from a base instance
            MaKhachHang = khachHang.MaKhachHang;
            HoTen = khachHang.HoTen;
            CMND = khachHang.CMND;
            SDT = khachHang.SDT;
            Email = khachHang.Email;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Customer()
        {
            
        }

        #endregion
    }
}
