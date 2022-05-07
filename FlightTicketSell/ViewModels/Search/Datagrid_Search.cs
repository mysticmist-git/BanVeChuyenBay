using System;
using System.Windows.Input;
using FlightTicketSell.ViewModels.Search;

namespace FlightTicketSell.ViewModels.Search
{
    public class Datagrid_Search : BaseViewModel
    {
        public ICommand Open_Window_DescriptionTicketFlight_Command { get; set; }


        public Datagrid_Search()
        {
            Open_Window_DescriptionTicketFlight_Command = new RelayCommand<object>((p) => true, (p) =>
            {
                IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.FlightTicket;
                IoC.IoC.Get<FlightTicket_Search>().MaChuyenBay = MaChuyenBay;
               IoC.IoC.Get<TickedSoldBooked_Search>().MaChuyenBay = MaChuyenBay;
                IoC.IoC.Get<FlightTicket_Search>().SanBayDi = SanBayDi;
                IoC.IoC.Get<FlightTicket_Search>().SanBayDen = SanBayDen;
                IoC.IoC.Get<FlightTicket_Search>().NgayGioBay = KhoiHanh.ToString();
                IoC.IoC.Get<FlightTicket_Search>().GiaCoBan = GiaCoBan;
            });
        }

        public string MaChuyenBay { get; set; }

        public string SanBayDi { get; set; }

        public string SanBayDen { get; set; }

        public DateTime KhoiHanh { get; set; }

        public string SoDiemDung { get; set; }

        public string SoHangVe { get; set; }

        public decimal GiaCoBan { get; set; }

        public string GheTrong { get; set; }
    }
}
