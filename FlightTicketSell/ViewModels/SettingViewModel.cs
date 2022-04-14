using FlightTicketSell.Models;
using FlightTicketSell.Views;
using System.Windows.Input;
using System.Linq;
using System.Collections.ObjectModel;
using FlightTicketSell.ViewModels.Setting;

namespace FlightTicketSell.ViewModels
{
    public class SettingViewModel : BaseViewModel
    {
        #region Commands
        public ICommand Open_Window_MoreAirport_Command { get; set; }
        public ICommand Open_Window_MoreTicketClass_Command { get; set; }
        public ICommand Save_FlightRegulations_Command { get; set; }
        public ICommand LoadCommand { get; set; }
        #endregion

        #region Public Properties


        /// <summary>
        /// Số sân bay trung gian tối đa
        /// </summary>
        public int Max_LayoverAirport { get; set; }

        /// <summary>
        /// Thời gian bay tối thiểu
        /// </summary>
        public int Min_FlightTime{ get; set; }

        /// <summary>
        /// Thời gian đặt vé chậm nhất
        /// </summary>
        public int Latest_BookingTime { get; set; }

        /// <summary>
        /// Thời gian hủy đặt chỗ
        /// </summary>
        public int Cancel_BookingTime { get; set; }

        /// <summary>
        /// Thời gian dừng tối thiểu
        /// </summary>
        public int Min_TimeStop { get; set; }

        /// <summary>
        /// Thời gian dừng tối đa
        /// </summary>
        public int Max_TimeStop { get; set; }

        /// <summary>
        /// Danh sách sân bay
        /// </summary>
        public ObservableCollection<Airport> List_Airport { get; set; } = new ObservableCollection<Airport>();

        #endregion

        public string Title { get; } = "CÀI ĐẶT";

        public SettingViewModel()
        {
            LoadCommand = new RelayCommand<object>(
                (p) => { return true; },
                (p) =>
                {
                    using(var context = new FlightTicketSellEntities())
                    {
                        // Các quy định về chuyến bay
                        Max_LayoverAirport = (context.THAMSOes.ToList()).Where(h => h.TenThamSo == "SoSanBayTrungGianToiDa").FirstOrDefault().GiaTri;

                        if (List_Airport != null)
                        {
                            List_Airport.Clear();
                        }    
                            
                        // Danh sách sân bay
                        foreach (var item in context.SANBAYs.ToList())
                        {
                            List_Airport.Add(new Airport() { Code = item.VietTat, Name = item.TenSanBay, Province = item.TinhThanh });
                        }
                        

                    }

                }
            );

            Open_Window_MoreAirport_Command = new RelayCommand<object>(
                (p) => { return true; },
                (p) =>
                {
                    MoreAirportView moreAirportView = new MoreAirportView();
                    moreAirportView.ShowDialog();
                }
            );

            Open_Window_MoreTicketClass_Command = new RelayCommand<object>(
                (p) => { return true; },
                (p) =>
                {
                    MoreTicketClassView moreTicketClassView = new MoreTicketClassView();
                    moreTicketClassView.ShowDialog();
                }
            );

            Save_FlightRegulations_Command = new RelayCommand<object>(

               (p) => { return true; },

               (p) =>
               {
                   using (var context = new FlightTicketSellEntities())
                   {
                       context.THAMSOes.Where(h => h.TenThamSo == "SoSanBayTrungGianToiDa").FirstOrDefault().GiaTri = Max_LayoverAirport;
                       context.SaveChanges();
                      
                   }
               }
           );
        }
    }
}
