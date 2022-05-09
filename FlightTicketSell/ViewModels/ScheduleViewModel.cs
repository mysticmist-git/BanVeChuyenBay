using FlightTicketSell.Models;
using FlightTicketSell.ViewModels.Schedule;
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
        #region LayoverAirport

        #region Command
        /// <summary>
        /// Mở nhập sân bay trung gian
        /// </summary>
        public ICommand Open_Window_EnterLayoverAirport_Command { get; set; }
        /// <summary>
        /// Thêm sân bay trung gian
        /// </summary>
        public ICommand EnterLayoverAirport_LoadCommand { get; set; }
        #endregion

        #region Properties
        /// <summary>
        /// Danh sách sân bay
        /// </summary>
        public ObservableCollection<Airport> LayoverAirport_List_Airport { get; set; } = new ObservableCollection<Airport>();
        /// <summary>
        /// Thời gian dừng
        /// </summary>
        public string LayoverAirport_StopTime { get; set; }
        /// <summary>
        /// Ghi chú
        /// </summary>
        public string LayoverAirport_Note { get; set; }
        #endregion

        #endregion

        #region TicketClass
        /// <summary>
        /// Mở nhập hạng vé
        /// </summary>
        public ICommand Open_Window_EnterTicketClass_Command { get; set; }
        #endregion

        #region Main Commands
        /// <summary>
        /// Nút đổi sân bay đi & sân bay đến
        /// </summary>
        public ICommand Change_Departure_Landing_Airport_Command { get; set; }
        /// <summary>
        /// Hàm load chính
        /// </summary>
        public ICommand LoadCommand { get; set; }
       
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

        #region Main Properties
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
        public  Airport DepartureAirport { get; set; } = new Airport();

        /// <summary>
        /// Sân bay đến
        /// </summary>
        public Airport LandingAirport { get; set; } = new Airport();

        /// <summary>
        /// Danh sách sân bay được chọn
        /// </summary>
        public ObservableCollection<Airport> ChooseAirport_List { get; set; } = new ObservableCollection<Airport>();

        /// <summary>
        /// Sân bay được chọn trong Chọn sân bay
        /// </summary>
        public Airport ChooseAirport_SelectedItem { get; set; }= new Airport();
        /// <summary>
        /// Biến đánh dấu thành phần nào đã mở DialogHost Chọn sân bay
        /// </summary>
        private static string OpenChooseAirport { get; set; }
        /// <summary>
        /// Danh sách sân bay trung gian
        /// </summary>
        public ObservableCollection<LayoverAirport> List_LayoverAirport { get; set; } = new ObservableCollection<LayoverAirport> ();

        /// <summary>
        /// Danh sách hạng vé
        /// </summary>
        //public ObservableCollection<TicketClass> List_TicketClass { get; set; } = new ObservableCollection<TicketClass>();

        #endregion

        #region Main Method
        /// <summary>
        /// Loại bỏ 1 phần tử trong ObservableCollection
        /// </summary>
        public bool RemoveAirportItem(ObservableCollection<Airport> airports, Airport airport)
        {
            if (airports.Count == 0)
                return false;
            foreach (Airport airportItem in airports)
            {
                if (airport.Id == airportItem.Id)
                {
                    airports.Remove(airportItem);
                    return true;
                }
            }
            return false;
        }
       
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

                       //Danh sách sân bay trung gian
                       using (var context = new FlightTicketSellEntities())
                       {
                           try
                           {

                           }
                           catch (EntityException e)
                           {
                               MessageBox.Show("Database access failed!", string.Format($"Exception: {e.Message}"), MessageBoxButton.OK, MessageBoxImage.Error);
                           }
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
                           LayoverAirport_List_Airport = new ObservableCollection<Airport>
                              (context.SANBAYs.Select(h => new Airport
                              {
                                  Id = h.MaSanBay,
                                  Name = h.TenSanBay,
                                  Code = h.VietTat,
                                  Province = h.TinhThanh,
                                  Status= h.TrangThai

                              }).Where(k => k.Status!=false).ToList()
                              );
                           if (!string.IsNullOrEmpty(LandingAirport.Name))
                           {
                               RemoveAirportItem(LayoverAirport_List_Airport, LandingAirport);
                           }
                           if (!string.IsNullOrEmpty(DepartureAirport.Name))
                           {
                               RemoveAirportItem(LayoverAirport_List_Airport, DepartureAirport);
                           }
                           if (List_LayoverAirport.Count > 0)
                           {
                               foreach (var item in LayoverAirport_List_Airport)
                               {
                                   bool temp = false;
                                   foreach (var h in List_LayoverAirport)
                                       if (h.Id_Airport == item.Id)
                                       {
                                           temp = true;
                                           return;
                                       }
                                   if (temp)
                                       LayoverAirport_List_Airport.Remove(item);
                               }
                           }
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
                   (LandingAirport, DepartureAirport) = (DepartureAirport, LandingAirport);
               }
           );
            ChooseDepartureAirport_Command = new RelayCommand<object>((p) => { return true; },
             async (p) =>
             {
                 
                ChooseAirportView chooseAirportView = new ChooseAirportView { DataContext=this };
                 OpenChooseAirport = "Departure";
                 var result = await DialogHost.Show(chooseAirportView, "RootDialog");
             }
           );
            ChooseLandingAirport_Command = new RelayCommand<object>((p) => { return true; },
             async (p) =>
             {
               
                 ChooseAirportView chooseAirportView = new ChooseAirportView { DataContext=this};
                 OpenChooseAirport = "Landing";
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
                          }
                          if (!string.IsNullOrEmpty(LandingAirport.Name) )
                          {
                              RemoveAirportItem(ChooseAirport_List, LandingAirport);
                          }
                          if (!string.IsNullOrEmpty(DepartureAirport.Name))
                          {
                              RemoveAirportItem(ChooseAirport_List, DepartureAirport);
                          }
                          if (List_LayoverAirport.Count>0)
                          {
                              foreach(var item in ChooseAirport_List)
                              {
                                  bool temp = false;
                                  foreach (var h in List_LayoverAirport)
                                      if (h.Id_Airport == item.Id)
                                      {
                                          temp = true;
                                          return;
                                      }
                                  if (temp)
                                      ChooseAirport_List.Remove(item);
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
                    DepartureAirport.Id = ChooseAirport_SelectedItem.Id;
                    DepartureAirport.Name = ChooseAirport_SelectedItem.Name;
                    DepartureAirport.Province = ChooseAirport_SelectedItem.Province;
                    DepartureAirport.Code = ChooseAirport_SelectedItem.Code;
                    DepartureAirport.Status = ChooseAirport_SelectedItem.Status;
                }
                if (OpenChooseAirport=="Landing")
                {
                    LandingAirport.Id = ChooseAirport_SelectedItem.Id;
                    LandingAirport.Name = ChooseAirport_SelectedItem.Name;
                    LandingAirport.Province = ChooseAirport_SelectedItem.Province;
                    LandingAirport.Code = ChooseAirport_SelectedItem.Code;
                    LandingAirport.Status = ChooseAirport_SelectedItem.Status;
                }
                // Close dialog
                DialogHost.CloseDialogCommand.Execute(null, null);
            }
          );
            #endregion

            #region LayoverAirport
            Open_Window_EnterLayoverAirport_Command = new RelayCommand<object>((p) => { return true; },
               (p) =>
               {
                   EnterLayoverAirportView enterLayoverAirportView = new EnterLayoverAirportView { DataContext = this };
                   var result = DialogHost.Show(enterLayoverAirportView, "RootDialog");
               }
            );
            #endregion

            #region TicketClass
            Open_Window_EnterTicketClass_Command = new RelayCommand<object>((p) => { return true; },
               async (p) =>
               {
                   EnterTicketClassView enterTicketClassView = new EnterTicketClassView { DataContext = this };
                   var result = await DialogHost.Show(enterTicketClassView, "RootDialog");
               }
            );
            #endregion
        }
    }
}
