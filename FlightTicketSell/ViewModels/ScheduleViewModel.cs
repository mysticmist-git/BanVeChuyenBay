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
        /// <summary>
        /// Nút lưu thêm sân bay trung gian
        /// </summary>
        public ICommand EnterLayoverAirport_Save_Command { get; set; }
        /// <summary>
        /// Nút hủy thêm sân bay trung gian
        /// </summary>
        public ICommand EnterLayoverAirport_Cancel_Command { get; set; }
        /// <summary>
        /// thay cho event textchanged của stoptimetextbox
        /// </summary>
        //public ICommand LayoverAirport_StopTimeCheck_Command { get; set; }

        /// <summary>
        /// Mở chỉnh sửa sân bay trung gian
        /// </summary>
        public ICommand Open_Window_EditLayoverAirport_Command { get; set; }
        /// <summary>
        /// Lưu chỉnh sửa sân bay trung gian
        /// </summary>
        public ICommand EditLayoverAirport_Save_Command { get; set; }
        /// <summary>
        /// Hủy sửa sân bay trung gian
        /// </summary>
        public ICommand EditLayoverAirport_Cancel_Command { get; set; }
        #endregion

        #region Properties
        /// <summary>
        /// Danh sách sân bay
        /// </summary>
        public ObservableCollection<Airport> LayoverAirport_List_Airport { get; set; } = new ObservableCollection<Airport>();
        /// <summary>
        /// Sân bay được chọn
        /// </summary>
        public Airport LayoverAirport_Airport_SelectedItem { get; set; }
        /// <summary>
        /// Thời gian dừng
        /// </summary>
        public string LayoverAirport_StopTime { get; set; }
        /// <summary>
        /// Chuỗi helper thời gian dừng tối đa, tối thiểu
        /// </summary>
        public string LayoverAirport_MinMaxStopTime { get; set; }
        /// <summary>
        /// Ghi chú
        /// </summary>
        public string LayoverAirport_Note { get; set; }
        /// <summary>
        /// Sửa ghi chú
        /// </summary>
        public string EditLayoverAirport_Note { get; set; }
        /// <summary>
        /// Sửa thời gian dừng
        /// </summary>
        public string EditLayoverAirport_StopTime { get; set; }
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
        /// Sân bay trung gian được chọn để edit
        /// </summary>
        public LayoverAirport List_LayoverAirport_SelectedItem { get; set; }
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
                           var LayoverAirport_MinStopTime = context.THAMSOes.Where(h => h.TenThamSo == "ThoiGianDungToiThieu").ToList().FirstOrDefault().GiaTri;
                           var LayoverAirport_MaxStopTime = context.THAMSOes.Where(h => h.TenThamSo == "ThoiGianDungToiDa").ToList().FirstOrDefault().GiaTri;
                           LayoverAirport_MinMaxStopTime = "Thời gian dừng từ " + LayoverAirport_MinStopTime.ToString() + " phút đến " + LayoverAirport_MaxStopTime.ToString() + " phút";
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
                                foreach (var h in List_LayoverAirport)
                                {
                                    var temp = LayoverAirport_List_Airport.Where(k => k.Id == h.Id_Airport).ToList().FirstOrDefault();
                                    LayoverAirport_List_Airport.Remove(temp);
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
                            foreach (var h in List_LayoverAirport)
                            {
                                var temp = ChooseAirport_List.Where(k => k.Id ==h.Id_Airport).ToList().FirstOrDefault();
                                ChooseAirport_List.Remove(temp);
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
             async (p) =>
               {
                   LayoverAirport_StopTime = null;
                   LayoverAirport_Note=null;
                   EnterLayoverAirportView enterLayoverAirportView = new EnterLayoverAirportView { DataContext = this };
                   var result = await DialogHost.Show(enterLayoverAirportView, "RootDialog");
               }
            );

            EnterLayoverAirport_Save_Command = new RelayCommand<object>((p) => { return true; },
              (p) =>
              {
                  if (string.IsNullOrEmpty(LayoverAirport_StopTime))
                  {
                      MessageBox.Show("Bạn chưa nhập thời gian dừng!", "Cảnh báo");
                      return;
                  }
                     
                  if (string.IsNullOrEmpty(LayoverAirport_Airport_SelectedItem.Name))
                  {
                      MessageBox.Show("Bạn chưa chọn sân bay!", "Cảnh báo");
                      return;
                  }

                  try
                  {
                      using (var context = new FlightTicketSellEntities())
                      {
                         
                          var LayoverAirport_MinStopTime = context.THAMSOes.Where(h => h.TenThamSo == "ThoiGianDungToiThieu").ToList().FirstOrDefault().GiaTri;
                          var LayoverAirport_MaxStopTime = context.THAMSOes.Where(h => h.TenThamSo == "ThoiGianDungToiDa").ToList().FirstOrDefault().GiaTri;
                          var stoptime = int.Parse(LayoverAirport_StopTime);
                          if (stoptime < LayoverAirport_MinStopTime || stoptime > LayoverAirport_MaxStopTime)
                          {
                              MessageBox.Show("Thời gian dừng không hợp lệ!", "Cảnh báo");
                              LayoverAirport_StopTime = null;
                              return;
                          }
                      }
                  }
                  catch (System.Data.Entity.Core.EntityException e)
                  {
                      MessageBox.Show($"Exception: {e.Message}");
                  }
                  List_LayoverAirport.Add(new LayoverAirport 
                  {
                      Id_Airport = LayoverAirport_Airport_SelectedItem.Id,
                      AirportName = LayoverAirport_Airport_SelectedItem.Name,
                      StopTime = int.Parse(LayoverAirport_StopTime),
                      Note = LayoverAirport_Note?.ToString(),
                      Order = List_LayoverAirport.Count + 1
                  });
                  // Close dialog
                  DialogHost.CloseDialogCommand.Execute(null, null);
              }
           );
            EnterLayoverAirport_Cancel_Command = new RelayCommand<object>((p) => { return true; },
              (p) =>
              {
                  // Close dialog
                  DialogHost.CloseDialogCommand.Execute(null, null);
              }
           );
            Open_Window_EditLayoverAirport_Command = new RelayCommand<object>((p) => { return true; },
             async (p) =>
              {
                  EditLayoverAirportView editLayoverAirportView = new EditLayoverAirportView { DataContext = this };
                  var result = await DialogHost.Show(editLayoverAirportView, "RootDialog");
              }
           );
            EditLayoverAirport_Save_Command = new RelayCommand<object>((p) => { return true; },
              (p) =>
              {
                  //using (var context = new FlightTicketSellEntities())
                  //{
                  //    try
                  //    {
                  //        if (EditLayoverAirport_StopTime != null || EditLayoverAirport_StopTime != null)
                  //        {

                  //            if (EditAirport_Name != null)
                  //                context.SANBAYs.ToList().Where(s => s.MaSanBay == Airport_selecteditem.Id).FirstOrDefault().TenSanBay = EditAirport_Name;
                  //            if (EditAirport_Province != null)
                  //                context.SANBAYs.ToList().Where(s => s.MaSanBay == Airport_selecteditem.Id).FirstOrDefault().TinhThanh = EditAirport_Province;

                  //            context.SaveChanges();
                  //            MessageBox.Show("Lưu thay đổi sân bay thành công!", "Cảnh báo");

                  //            // Close dialog
                  //            DialogHost.CloseDialogCommand.Execute(null, null);
                  //        }
                  //        else
                  //        {
                  //            MessageBox.Show("Vui lòng nhập ít nhất một thông tin!", "Cảnh báo");
                  //        }
                  //    }
                  //    catch (EntityException e)
                  //    {
                  //        MessageBox.Show("Database access failed!", string.Format($"Exception: {e.Message}"), MessageBoxButton.OK, MessageBoxImage.Error);
                  //    }
                  //}
              }
           );
            EditLayoverAirport_Cancel_Command = new RelayCommand<object>((p) => { return true; },
              (p) =>
              {
                
              }
           );
            //LayoverAirport_StopTimeCheck_Command = new RelayCommand<object>((p) => { return true; },
            //  (p) =>
            // {
            //     try
            //     {
            //         using (var context = new FlightTicketSellEntities())
            //         {
            //             if (string.IsNullOrEmpty(LayoverAirport_StopTime))
            //                 return;
            //             var LayoverAirport_MinStopTime = context.THAMSOes.Where(h => h.TenThamSo == "ThoiGianDungToiThieu").ToList().FirstOrDefault().GiaTri;
            //             var LayoverAirport_MaxStopTime = context.THAMSOes.Where(h => h.TenThamSo == "ThoiGianDungToiDa").ToList().FirstOrDefault().GiaTri;
            //             var stoptime = int.Parse(LayoverAirport_StopTime);
            //             if (stoptime < LayoverAirport_MinStopTime || stoptime > LayoverAirport_MaxStopTime)
            //             {
            //                 LayoverAirport_StopTime.Remove(LayoverAirport_StopTime.Length - 1);
            //             }
            //         }
            //     }
            //     catch (System.Data.Entity.Core.EntityException e)
            //     {
            //         MessageBox.Show($"Exception: {e.Message}");
            //     }
            // }
            //);

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
