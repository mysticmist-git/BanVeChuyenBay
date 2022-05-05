using FlightTicketSell.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightTicketSell.ViewModels.Report;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Windows.Input;
using FlightTicketSell.Models.Enums;
using FlightTicketSell.Views.Helper;
using System.ComponentModel;
using System.Windows;
using System.Data.Entity.Core;

namespace FlightTicketSell.ViewModels.Search
{
    public class TicketSold : BaseViewModel
    {
        public string MaKhachHang { get; set; }
        public string TenKhachHang { get; set; }
        public string CMND { get; set; }
        public string SDT { get; set; }
        public string Email { get; set; }
        public string MaVe { get; set; }
        public string TenHangVe { get; set; }
        public DateTime NgayThanhToan { get; set; }
        public string GiaTien { get; set; }

        public ICommand Open_Window_DescriptionCustomer_Command { get; set; }

        public TicketSold()
        {
            Open_Window_DescriptionCustomer_Command = new RelayCommand<object>((p) => true, (p) =>
            {
                IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.Customer;
                IoC.IoC.Get<Customer_Search>().MaKhachHang = MaKhachHang;
                IoC.IoC.Get<Customer_Search>().TenKhachHang = TenKhachHang;
                IoC.IoC.Get<Customer_Search>().CMND = CMND;
                IoC.IoC.Get<Customer_Search>().SDT = SDT;
                IoC.IoC.Get<Customer_Search>().Email = Email;
            });


        }

    }
}
