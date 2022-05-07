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
        /// Mở DialogHost Chọn sân bay đi
        /// </summary>
        public ICommand ChooseDepartureAirport_Command { get; set; }
        /// <summary>
        /// Mở DialogHost Chọn sân bay đến
        /// </summary>
        public ICommand ChooseLandingAirport_Command { get; set; }
        /// <summary>
        /// Hàm load DialogHost Chọn sân bay
        /// </summary>
        public ICommand ChooseAirport_LoadCommand { get; set; }
        /// <summary>
        /// Nút chọn trong Chọn sân bay
        /// </summary>
        public ICommand ChooseAirportButton_Command { get; set; }
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
        public  Airport DepartureAirport { get; set; } 

        /// <summary>
        /// Sân bay đến
        /// </summary>
        public Airport LandingAirport { get; set; }

        /// <summary>
        /// Danh sách sân bay được chọn
        /// </summary>
        public ObservableCollection<Airport> ChooseAirport_List { get; set; } = new ObservableCollection<Airport>();

        /// <summary>
        /// Sân bay được chọn trong Chọn sân bay
        /// </summary>
        public Airport ChooseAirport_SelectedItem { get; set; }

        /// <summary>
        /// Danh sách sân bay trung gian
        /// </summary>
        //public ObservableCollection<LayoverAirport> List_LayoverAirport { get; set; } = new ObservableCollection<LayoverAirport>();

        /// <summary>
        /// Danh sách hạng vé
        /// </summary>
        //public ObservableCollection<TicketClass> List_TicketClass { get; set; } = new ObservableCollection<TicketClass>();

        #endregion

        #region Private Properties
        /// <summary>
        /// Biến đánh dấu thành phần nào đã mở DialogHost Chọn sân bay
        /// </summary>
        private static string OpenChooseAirport { get; set; }

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

            ChooseDepartureAirport_Command = new RelayCommand<object>((p) => { return true; },
             async (p) =>
             {
                 
                ChooseAirportView chooseAirportView = new ChooseAirportView();
                 OpenChooseAirport = "Departure";
                 var result = await DialogHost.Show(chooseAirportView, "RootDialog");
             }
           );
            ChooseLandingAirport_Command = new RelayCommand<object>((p) => { return true; },
             async (p) =>
             {
               
                 ChooseAirportView chooseAirportView = new ChooseAirportView();
                 var result = await DialogHost.Show(chooseAirportView, "RootDialog");
                 OpenChooseAirport = "Landing";
             }
           );
            ChooseAirport_LoadCommand = new RelayCommand<object>((p) => { return true; },
              (p) =>
              {
                  using (var context = new FlightTicketSellEntities())
                  {
                      try
                      {
                          // Danh sách sân bay
                          if (ChooseAirport_List != null)
                          {
                              ChooseAirport_List.Clear();
                          }
                          foreach (var item in context.SANBAYs.ToList())
                          {
                              //Sân bay còn hoạt động mới thêm vào list
                              if (item.TrangThai )
                                  ChooseAirport_List.Add(new Airport() { Id = item.MaSanBay, Code = item.VietTat, Name = item.TenSanBay, Province = item.TinhThanh, Status = item.TrangThai });
                              if (LandingAirport!=null)
                              {
                                  ChooseAirport_List.Remove (LandingAirport);
                              }
                              if (DepartureAirport != null)
                              {
                                  ChooseAirport_List.Remove(DepartureAirport);
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
            ChooseAirportButton_Command = new RelayCommand<object>((p) => { return true; },
             (p) =>
            {
                if (string.IsNullOrEmpty(OpenChooseAirport) || ChooseAirport_SelectedItem==null)
                    return;
                if (OpenChooseAirport=="Departure")
                {
                    DepartureAirport = new Airport
                    {
                        Id = ChooseAirport_SelectedItem.Id,
                        Name = ChooseAirport_SelectedItem.Name,
                        Province = ChooseAirport_SelectedItem.Province,
                        Code = ChooseAirport_SelectedItem.Code,
                        Status = ChooseAirport_SelectedItem.Status
                    };
                }
                if (OpenChooseAirport=="Landing")
                {
                    LandingAirport = new Airport(ChooseAirport_SelectedItem);
                }
                // Close dialog
                DialogHost.CloseDialogCommand.Execute(null, null);
            }
          );
            #endregion


        }
    }
}
