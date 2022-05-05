using FlightTicketSell.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FlightTicketSell.ViewModels.Search
{
    public class Customer_Search : BaseViewModel
    {
        public string MaKhachHang { get; set; }
        public string TenKhachHang { get; set; }
        public string MaKhachDat { get; set; }
        public string CMND { get; set; }
        public string SDT { get; set; }
        public string Email { get; set; }

        public ICommand LoadCommand { get; set; }

        public ICommand ReturnCommand { get; set; }

        public Customer_Search()
        {

            ReturnCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.TickedSoldBooked);
            LoadCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                using (var context = new FlightTicketSellEntities())
                {
                    try
                    {
                        if (MaKhachDat == "")
                        {
                            TenKhachHang = context.KHACHHANGs.Where(result => result.MaKhachHang.ToString() == MaKhachHang).FirstOrDefault().HoTen;
                            CMND = context.KHACHHANGs.Where(result => result.MaKhachHang.ToString() == MaKhachHang).FirstOrDefault().CMND;
                            SDT = context.KHACHHANGs.Where(result => result.MaKhachHang.ToString() == MaKhachHang).FirstOrDefault().SDT;
                            Email = context.KHACHHANGs.Where(result => result.MaKhachHang.ToString() == MaKhachHang).FirstOrDefault().Email;
                        }
                        else if (MaKhachHang == "")
                        {
                            TenKhachHang = context.KHACHHANGs.Where(result => result.MaKhachHang.ToString() == MaKhachDat).FirstOrDefault().HoTen;
                            CMND = context.KHACHHANGs.Where(result => result.MaKhachHang.ToString() == MaKhachDat).FirstOrDefault().CMND;
                            SDT = context.KHACHHANGs.Where(result => result.MaKhachHang.ToString() == MaKhachDat).FirstOrDefault().SDT;
                            Email = context.KHACHHANGs.Where(result => result.MaKhachHang.ToString() == MaKhachDat).FirstOrDefault().Email;
                        }

                    }
                    catch
                    {
                    }
                }
            });

            
        }
    }
}
