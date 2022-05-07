using FlightTicketSell.Models;
using FlightTicketSell.ViewModels.Setting;
using FlightTicketSell.Views;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.ObjectModel;
using System.Data.Entity.Core;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FlightTicketSell.ViewModels
{
    public class ScheduleViewModel : BaseViewModel
    {
        #region Commands
        /// <summary>
        /// Mở nhập sân bay trung gian
        /// </summary>
        public ICommand Open_Window_EnterLayoverAirport_Command { get; set; }
        /// <summary>
        /// Mở nhập hạng vé
        /// </summary>
        public ICommand Open_Window_EnterTicketClass_Command { get; set; }
        /// <summary>
        /// Nút đổi sân bay đi & sân bay đến
        /// </summary>
        public ICommand Change_Departure_Landing_Airport_Command { get; set; }
        public ICommand LoadCommand { get; set; }
        /// <summary>
        /// Thêm sân bay trung gian
        /// </summary>
        public ICommand EnterLayoverAirport_LoadCommand { get; set; }
        /// <summary>
        /// DialogHost Thêm sân bay
        /// </summary>
        public ICommand ChooseAirport_Command { get; set; }
        /// <summary>
        /// Hàm load DialogHost Thêm sân bay
        /// </summary>
        public ICommand ChooseAirport_LoadCommand { get; set; }
        #endregion

        #region Public Properties
        /// <summary>
        /// Mã chuyến bay
        /// </summary>
        public string FlightCode { get; set; }

        /// <summary>
        /// Giá vé
        /// </summary>
        public string Airfares { get; set; }

        /// <summary>
        /// Thời gian bay
        /// </summary>
        public int FlightTime { get; set; }

        /// <summary>
        /// Ngày bay
        /// </summary>
        public DateTime DateFlight { get; set; }
        /// <summary>
        /// Giờ bay
        /// </summary>
        public DateTime TimeFlight { get; set; } 
        /// <summary>
        /// Sân bay đi
        /// </summary>
        public Airport DepartureAirport { get; set; } 

        /// <summary>
        /// Sân bay đến
        /// </summary>
        public Airport LandingAirport { get; set; }

        /// <summary>
        /// Danh sách sân bay được chọn
        /// </summary>
        public ObservableCollection<Airport> ChooseAirport_List { get; set; }

        /// <summary>
        /// Danh sách sân bay trung gian
        /// </summary>
        //public ObservableCollection<LayoverAirport> List_LayoverAirport { get; set; } = new ObservableCollection<LayoverAirport>();

        /// <summary>
        /// Danh sách hạng vé
        /// </summary>
        //public ObservableCollection<TicketClass> List_TicketClass { get; set; } = new ObservableCollection<TicketClass>();

        #endregion

        public ScheduleViewModel()
        {
            #region Main Command

            LoadCommand = new RelayCommand<object>((p) => { return true; },
               (p) =>
               {
                   try
                   {
                       // Gán ngày hiện tại cho datepicker lúc mở 
                       DateFlight = DateTime.Now;
                       // Gán ngày hiện tại cho datepicker lúc mở 
                       TimeFlight = DateTime.Now;

                       using (var context = new FlightTicketSellEntities())
                       {
                           

                       }
                   }
                   catch (System.Data.Entity.Core.EntityException e)
                   {
                       MessageBox.Show($"Exception: {e.Message}");
                   }
               }
           );

            EnterLayoverAirport_LoadCommand = new RelayCommand<object>((p) => { return true; },
               (p) =>
               {
                   try
                   {
                       using (var context = new FlightTicketSellEntities())
                       {
                      

                       }
                   }
                   catch (System.Data.Entity.Core.EntityException e)
                   {
                       MessageBox.Show($"Exception: {e.Message}");
                   }
               }
           );

            Change_Departure_Landing_Airport_Command = new RelayCommand<object>((p) => { return true; },
               (p) =>
               {

               }
           );

            Open_Window_EnterLayoverAirport_Command = new RelayCommand<object>((p) => { return true; },
              async (p) =>
                {
                    EnterLayoverAirportView enterLayoverAirportView = new EnterLayoverAirportView();
                    var result = await DialogHost.Show(enterLayoverAirportView, "RootDialog");
                }
            );

            Open_Window_EnterTicketClass_Command = new RelayCommand<object>((p) => { return true; },
               async (p) =>
                {
                    EnterTicketClassView enterTicketClassView = new EnterTicketClassView();
                    var result = await DialogHost.Show(enterTicketClassView, "RootDialog");
                }
            );

            ChooseAirport_Command = new RelayCommand<object>((p) => { return true; },
             async (p) =>
             {
                ChooseAirportView chooseAirportView = new ChooseAirportView();
                 var result = await DialogHost.Show(chooseAirportView, "RootDialog");
             }
           );

            ChooseAirport_LoadCommand = new RelayCommand<object>((p) => { return true; },
              (p) =>
              {
                  using (var context = new FlightTicketSellEntities())
                  {
                      try
                      {
                          var list = context.SANBAYs.ToList();
                          if (list!=null)
                          {
                              ChooseAirport_List = new ObservableCollection<Airport>();
                              foreach (var s in list)
                              {
                                  ChooseAirport_List.Add(new Airport{ Id = s.MaSanBay, Code = s.VietTat, Name = s.TenSanBay, Province = s.TinhThanh, Status = s.TrangThai });
                              }
                          }
                         
                       
                      }
                      catch (EntityException e)
                      {
                          MessageBox.Show("Database access failed!", string.Format($"Exception: {e.Message}"), MessageBoxButton.OK, MessageBoxImage.Error);
                      }
                  }
              }
          );
            #endregion


        }
    }
}
