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
    public class Booked : BaseViewModel
    {
        public string MaDatCho { get; set; }
        public string TenKhachDat { get; set; }
        public string CMND { get; set; }
        public string SDT { get; set; }
        public string Email { get; set; }

        public string SoCho { get; set; }
        public string TeHangVe { get; set; }
        public DateTime NgayDatVe { get; set; }
        public string GiaTien_Ve { get; set; }
        public string MaKhachDat { get; set; }

        public ICommand LoadCommand { get; set; }
        public ICommand Open_Window_DescriptionCustomer_Command { get; set; }

        public Booked()
        {
            Open_Window_DescriptionCustomer_Command = new RelayCommand<object>((p) => true, (p) =>
            {
                IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.Customer;
                IoC.IoC.Get<TickedSoldBooked_Search>().MaKhachDat = MaKhachDat;
                IoC.IoC.Get<Customer_Search>().TenKhachHang = TenKhachDat;
                IoC.IoC.Get<Customer_Search>().CMND = CMND;
                IoC.IoC.Get<Customer_Search>().SDT = SDT;
                IoC.IoC.Get<Customer_Search>().Email = Email;
            });


        }

    }
}
