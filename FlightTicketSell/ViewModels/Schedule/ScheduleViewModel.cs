using FlightTicketSell.Models;
using FlightTicketSell.ViewModels.Schedule;
using FlightTicketSell.ViewModels.Setting;
using FlightTicketSell.Views;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.ObjectModel;
using System.Data.Entity.Core;
using System.Linq;
using System.Text.RegularExpressions;
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
        /// <summary>
        /// Xóa sân bay trung gian
        /// </summary>
        public ICommand Delete_LayoverAirport_Command { get; set; }
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

        #region Command
        /// <summary>
        /// Hàm Load mở nhập hạng vé
        /// </summary>
        public ICommand EnterTicketClass_LoadCommand { get; set; }
        /// <summary>
        /// Mở nhập hạng vé
        /// </summary>
        public ICommand Open_Window_EnterTicketClass_Command { get; set; }
        /// <summary>
        /// Nút lưu khi thêm hạng vé
        /// </summary>
        public ICommand EnterTicketClass_Save_Command { get; set; }
        /// <summary>
        /// Nút hủy khi thêm hạng vé
        /// </summary>
        public ICommand EnterTicketClass_Cancel_Command { get; set; }
        /// <summary>
        /// Sửa hạng vé
        /// </summary>
        public ICommand Open_Window_EditTicketClass_Command { get; set; }
        /// <summary>
        /// Nút lưu trong sửa hạng vé
        /// </summary>
        public ICommand EditTicketClass_Save_Command { get; set; }
        /// <summary>
        /// Nút hủy trong sửa hạng vé
        /// </summary>
        public ICommand EditTicketClass_Cancel_Command { get; set; }
        /// <summary>
        /// Nút xóa hạng vé
        /// </summary>
        public ICommand Delete_EnteredTicketClass_Command { get; set; }
        /// <summary>
        /// Format lại chuỗi tiền vé
        /// </summary>
        public ICommand FormatStringToMoney { get; set; }
        /// <summary>
        /// Format ngược chuỗi tiền vé
        /// </summary>
        public ICommand FormatMoneyToString { get; set; }
        /// <summary>
        /// Nút nhận lịch bay
        /// </summary>
        public ICommand ScheduleAFlight_Command { get; set; }
        /// <summary>
        /// Hủy nhận lịch bay
        /// </summary>
        public ICommand RefreshScheduleAFlight_Command { get; set; }
        #endregion

        #region Properties
        /// <summary>
        /// Danh sách hạng vé
        /// </summary>
        public ObservableCollection<TicketClassDetail> List_TicketClass { get; set; } = new ObservableCollection<TicketClassDetail>();
        /// <summary>
        /// Hạng vé được chọn trong danh sách hạng vé
        /// </summary>
        public TicketClassDetail List_TicketClass_SelectedItem { get; set; }
        /// <summary>
        /// Danh sách hạng vé trong Thêm Hạng vé
        /// </summary>
        public ObservableCollection<TicketClass> EnterTicketClass_List_TicketClass { get; set; }
        /// <summary>
        /// Hạng vé được chọn trong danh sách hạng vé trong Thêm Hạng Vé
        /// </summary>
        public TicketClass EnterTicketClass_TicketClass_SelectedItem { get; set; }
        /// <summary>
        /// Số ghế trống trong thêm hạng vé
        /// </summary>
        public string EnterTicketClass_Seats { get; set; }
        /// <summary>
        /// Số ghế trống mới trong Sửa hạng vé
        /// </summary>
        public string EditTicketClass_Seats { get; set; }
        #endregion

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
        /// <summary>
        /// Nút Hủy trong Chọn sân bay
        /// </summary>
        public ICommand Cancel_ChooseAirportButton_Command { get; set; }
        /// <summary>
        /// Thay đổi ngày bay
        /// </summary>
        public ICommand SelectedFlightDateChanged_Command { get; set; }
        /// <summary>
        /// Thay đổi thời gian bay
        /// </summary>
        public ICommand SelectedFlightTimeChanged_Command { get; set; }
        /// <summary>
        /// Kiểm tra nhập thời gian bay
        /// </summary>
        public ICommand CheckFlightTime { get; set; }
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
        public string FlightTime { get; set; }
        /// <summary>
        /// Thời gian bay tối thiểu
        /// </summary>
        public string MinFlightTime { get; set; }
        /// <summary>
        /// Ngày bay
        /// </summary>
        public DateTime DateFlight { get; set; }
        /// <summary>
        /// Ngày hiển thị đầu tiên trong chọn ngày
        /// </summary>
        public DateTime DisplayDateStart { get; set; }
        /// <summary>
        /// Giờ bay
        /// </summary>
        public DateTime TimeFlight { get; set; }
        /// <summary>
        /// Sân bay đi
        /// </summary>
        public Airport DepartureAirport { get; set; } = new Airport();

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
        public Airport ChooseAirport_SelectedItem { get; set; } = new Airport();
        /// <summary>
        /// Biến đánh dấu thành phần nào đã mở DialogHost Chọn sân bay
        /// </summary>
        private static string OpenChooseAirport { get; set; }
        /// <summary>
        /// Danh sách sân bay trung gian
        /// </summary>
        public ObservableCollection<LayoverAirport> List_LayoverAirport { get; set; } = new ObservableCollection<LayoverAirport>();
        /// <summary>
        /// Sân bay trung gian được chọn để edit
        /// </summary>
        public LayoverAirport List_LayoverAirport_SelectedItem { get; set; }

        private bool FirstLoad { get; set; } = true;
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

                   if (FirstLoad)
                   {
                       // TEST: Cho mục đích test
                       DateFlight = DateTime.Now;
                       DisplayDateStart = DateTime.Now;
                       // Thực tế
                       //DateFlight = DateTime.Now.AddDays(2);
                       //DisplayDateStart = DateTime.Now.AddDays(2);

                       TimeFlight = new DateTime(DateFlight.Year, DateFlight.Month, DateFlight.Day, 0, 0, 0);
                       FirstLoad = false;
                       FlightCode = null;
                   }

                   using (var context = new FlightTicketSellEntities())
                   {
                       try
                       {
                           var LayoverAirport_MinStopTime = context.THAMSOes.Where(h => h.TenThamSo == "ThoiGianDungToiThieu").ToList().FirstOrDefault().GiaTri;
                           var LayoverAirport_MaxStopTime = context.THAMSOes.Where(h => h.TenThamSo == "ThoiGianDungToiDa").ToList().FirstOrDefault().GiaTri;
                           var minFlightTime = context.THAMSOes.Where(h => h.TenThamSo == "ThoiGianBayToiThieu").ToList().FirstOrDefault().GiaTri;
                           MinFlightTime = "Thời gian bay tối thiểu là " + minFlightTime.ToString() + " phút.";
                           LayoverAirport_MinMaxStopTime = "Từ " + LayoverAirport_MinStopTime.ToString() + " phút đến " + LayoverAirport_MaxStopTime.ToString() + " phút";
                       }
                       catch (EntityException e)
                       {
                           MessageBox.Show("Database access failed!", string.Format($"Exception: {e.Message}"), MessageBoxButton.OK, MessageBoxImage.Error);
                       }
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
                                  Status = h.TrangThai

                              }).Where(k => k.Status != false).ToList()
                              );
                           if (LandingAirport != null && !string.IsNullOrEmpty(LandingAirport.Name))
                           {
                               RemoveAirportItem(LayoverAirport_List_Airport, LandingAirport);
                           }
                           if (DepartureAirport != null && !string.IsNullOrEmpty(DepartureAirport.Name))
                           {
                               RemoveAirportItem(LayoverAirport_List_Airport, DepartureAirport);
                           }
                           if (List_LayoverAirport == null)
                               List_LayoverAirport = new ObservableCollection<LayoverAirport>();
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
                   catch (EntityException e)
                   {
                       MessageBox.Show($"Exception: {e.Message}");
                   }
               }
           );

            Change_Departure_Landing_Airport_Command = new RelayCommand<object>((p) => { return true; },
               (p) =>
               {
                   (LandingAirport, DepartureAirport) = (DepartureAirport, LandingAirport);
                   if (DepartureAirport != null && LandingAirport != null)
                   {
                       if (string.IsNullOrEmpty(DepartureAirport.Name) || string.IsNullOrEmpty(LandingAirport.Name))
                           return;
                       try
                       {
                           using (var context = new FlightTicketSellEntities())
                           {
                               var temp = context.CHUYENBAYs.ToList().Count + 1;
                               FlightCode = DepartureAirport.Code + LandingAirport.Code + "-" + temp.ToString();
                           }
                       }
                       catch (EntityException e)
                       {
                           MessageBox.Show($"Exception: {e.Message}");
                       }
                   }
               }
           );
            ChooseDepartureAirport_Command = new RelayCommand<object>((p) => { return true; },
             async (p) =>
             {

                 ChooseAirportView chooseAirportView = new ChooseAirportView { DataContext = this };
                 OpenChooseAirport = "Departure";
                 var result = await DialogHost.Show(chooseAirportView, "RootDialog");

                 if (DepartureAirport != null && LandingAirport != null)
                 {
                     if (string.IsNullOrEmpty(DepartureAirport.Name) || string.IsNullOrEmpty(LandingAirport.Name))
                     {
                         FlightCode = null;
                         return;
                     }
                     try
                     {
                         using (var context = new FlightTicketSellEntities())
                         {
                             var temp = context.CHUYENBAYs.ToList().Count + 1;
                             FlightCode = DepartureAirport.Code + LandingAirport.Code + "-" + temp.ToString();
                         }
                     }
                     catch (EntityException e)
                     {
                         MessageBox.Show($"Exception: {e.Message}");
                     }
                 }

             }
           );
            ChooseLandingAirport_Command = new RelayCommand<object>((p) => { return true; },
             async (p) =>
             {

                 ChooseAirportView chooseAirportView = new ChooseAirportView { DataContext = this };
                 OpenChooseAirport = "Landing";
                 var result = await DialogHost.Show(chooseAirportView, "RootDialog");

                 if (DepartureAirport != null && LandingAirport != null)
                 {
                     if (string.IsNullOrEmpty(DepartureAirport.Name) || string.IsNullOrEmpty(LandingAirport.Name))
                     {
                         FlightCode = null;
                         return;
                     }
                     try
                     {
                         using (var context = new FlightTicketSellEntities())
                         {
                             var temp = context.CHUYENBAYs.ToList().Count + 1;
                             FlightCode = DepartureAirport.Code + LandingAirport.Code + "-" + temp.ToString();
                         }
                     }
                     catch (EntityException e)
                     {
                         MessageBox.Show($"Exception: {e.Message}");
                     }
                 }

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
                              if (item.TrangThai)
                                  ChooseAirport_List.Add(new Airport() { Id = item.MaSanBay, Code = item.VietTat, Name = item.TenSanBay, Province = item.TinhThanh, Status = item.TrangThai });
                          }
                          if (LandingAirport != null && !string.IsNullOrEmpty(LandingAirport.Name))
                          {
                              RemoveAirportItem(ChooseAirport_List, LandingAirport);
                          }
                          if (DepartureAirport != null && !string.IsNullOrEmpty(DepartureAirport.Name))
                          {
                              RemoveAirportItem(ChooseAirport_List, DepartureAirport);
                          }
                          if (List_LayoverAirport != null && List_LayoverAirport.Count > 0)
                          {
                              foreach (var h in List_LayoverAirport)
                              {
                                  var temp = ChooseAirport_List.Where(k => k.Id == h.Id_Airport).ToList().FirstOrDefault();
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
                if (string.IsNullOrEmpty(OpenChooseAirport) || ChooseAirport_SelectedItem == null)
                {
                    MessageBox.Show("Hãy chọn sân bay!", "Cảnh báo");
                    return;
                }

                if (OpenChooseAirport == "Departure")
                {
                    DepartureAirport = new Airport()
                    {
                        Id = ChooseAirport_SelectedItem.Id,
                        Name = ChooseAirport_SelectedItem.Name,
                        Province = ChooseAirport_SelectedItem.Province,
                        Code = ChooseAirport_SelectedItem.Code,
                        Status = ChooseAirport_SelectedItem.Status
                    };
                }
                if (OpenChooseAirport == "Landing")
                {
                    LandingAirport = new Airport()
                    {
                        Id = ChooseAirport_SelectedItem.Id,
                        Name = ChooseAirport_SelectedItem.Name,
                        Province = ChooseAirport_SelectedItem.Province,
                        Code = ChooseAirport_SelectedItem.Code,
                        Status = ChooseAirport_SelectedItem.Status
                    };
                }
                // Close dialog
                DialogHost.CloseDialogCommand.Execute(null, null);
            }
          );
            Cancel_ChooseAirportButton_Command = new RelayCommand<object>((p) => { return true; },
             (p) =>
             {
                 // Close dialog
                 DialogHost.CloseDialogCommand.Execute(null, null);
                 if (OpenChooseAirport == "Departure")
                 {
                     DepartureAirport = new Airport();
                 }
                 if (OpenChooseAirport == "Landing")
                 {
                     LandingAirport = new Airport();
                 }
             });
            FormatStringToMoney = new RelayCommand<object>((p) => { return true; },
             (p) =>
             {
                 if (double.TryParse(Airfares, out double value) && value != 0)
                     Airfares = string.Format("{0:0,0}", value);
                 else
                     Airfares = string.Empty;
             });
            FormatMoneyToString = new RelayCommand<object>((p) => { return true; },
             (p) =>
             {
                 if (string.IsNullOrEmpty(Airfares))
                     return;
                 Airfares = Regex.Replace(Airfares, @"[^a-zA-Z0-9]", string.Empty);
             });
            ScheduleAFlight_Command = new RelayCommand<object>((p) => { return true; },
             (p) =>
             {
                 if (DepartureAirport == null || LandingAirport == null || string.IsNullOrEmpty(DepartureAirport.Name) || string.IsNullOrEmpty(LandingAirport.Name))
                 {
                     MessageBox.Show("Vui lòng chọn đủ sân bay đi và sân bay đến!", "Cảnh báo");
                     return;
                 }
                 if (string.IsNullOrEmpty(Airfares))
                 {
                     MessageBox.Show("Vui lòng nhập giá vé!", "Cảnh báo");
                     return;
                 }
                 if (string.IsNullOrEmpty(FlightTime))
                 {
                     MessageBox.Show("Vui lòng nhập thời gian bay!", "Cảnh báo");
                     return;
                 }
                 if (DateFlight == null)
                 {
                     MessageBox.Show("Vui lòng chọn ngày bay!", "Cảnh báo");
                     return;
                 }
                 if (TimeFlight == null)
                 {
                     MessageBox.Show("Vui lòng chọn giờ bay!", "Cảnh báo");
                     return;
                 }
                 if (List_TicketClass == null || List_TicketClass.Count < 1)
                 {
                     MessageBox.Show("Vui lòng chọn ít nhất một hạng vé!", "Cảnh báo");
                     return;
                 }

                 try
                 {
                     using (var context = new FlightTicketSellEntities())
                     {
                         //Thêm đường bay nếu chưa có sẵn
                         int thoigianbay = int.Parse(FlightTime);
                         DUONGBAY dUONGBAY = context.DUONGBAYs.Where
                         (
                             h => h.MaSanBayDi == DepartureAirport.Id
                             && h.MaSanBayDen == LandingAirport.Id
                             && h.ThoiGianBay == thoigianbay
                         ).ToList().FirstOrDefault();
                         if (dUONGBAY == null)
                         {
                             dUONGBAY = new DUONGBAY()
                             {
                                 MaSanBayDi = DepartureAirport.Id,
                                 MaSanBayDen = LandingAirport.Id,
                                 ThoiGianBay = int.Parse(FlightTime)
                             };
                             context.DUONGBAYs.Add(dUONGBAY);
                             context.SaveChanges();
                             dUONGBAY = context.DUONGBAYs.ToList().LastOrDefault();
                         }

                         //Thêm các sân bay trung gian nếu có
                         if (List_LayoverAirport != null && List_LayoverAirport.Count > 0)
                         {
                             foreach (var item in List_LayoverAirport)
                             {
                                 SANBAYTG sANBAYTG = new SANBAYTG()
                                 {
                                     MaDuongBay = dUONGBAY.MaDuongBay,
                                     MaSanBay = item.Id_Airport,
                                     ThuTu = item.Order,
                                     ThoiGianDung = item.StopTime,
                                     GhiChu = item.Note
                                 };
                                 context.SANBAYTGs.Add(sANBAYTG);
                             }
                             context.SaveChanges();
                         }

                         //Thêm chuyến bay
                         CHUYENBAY cHUYENBAY = new CHUYENBAY()
                         {
                             MaDuongBay = dUONGBAY.MaDuongBay,
                             GiaVe = int.Parse(Regex.Replace(Airfares, @"[^a-zA-Z0-9]", string.Empty)),
                             NgayGio = new DateTime(DateFlight.Year, DateFlight.Month, DateFlight.Day, TimeFlight.Hour, TimeFlight.Minute, TimeFlight.Second),
                             DaKhoiHanh = false
                         };
                         context.CHUYENBAYs.Add(cHUYENBAY);
                         context.SaveChanges();
                         cHUYENBAY = context.CHUYENBAYs.ToList().LastOrDefault();

                         //Thêm hạng vé cho chuyến bay
                         foreach (var item in List_TicketClass)
                         {
                             CHITIETHANGVE cHITIETHANGVE = new CHITIETHANGVE()
                             {
                                 MaHangVe = item.Id_TicketClass,
                                 MaChuyenBay = cHUYENBAY.MaChuyenBay,
                                 SoGhe = item.Seats
                             };
                             context.CHITIETHANGVEs.Add(cHITIETHANGVE);
                         }

                         context.SaveChanges();
                         MessageBox.Show("Thêm chuyến bay thành công!", "Cảnh báo");

                         //Load lại
                         //IoC.IoC.Rebind<ScheduleViewModel>();
                         RefreshScheduleAFlight_Command.Execute(null);
                     }
                 }
                 catch (EntityException e)
                 {
                     MessageBox.Show($"Exception: {e.Message}");
                 }

             });

            RefreshScheduleAFlight_Command = new RelayCommand<object>((p) => { return true; },
                (p) =>
                {
                    // TEST: Cho mục đích test
                    DateFlight = DateTime.Now;
                    // Thực tế
                    // DateFlight = DateTime.Now.AddDays(2);

                    TimeFlight = new DateTime(DateFlight.Year, DateFlight.Month, DateFlight.Day, 0, 0, 0);
                    DepartureAirport = null;
                    LandingAirport = null;
                    Airfares = null;
                    FlightTime = null;
                    List_TicketClass = null;
                    List_LayoverAirport = null;
                    FlightCode = null;
                });

            SelectedFlightDateChanged_Command = new RelayCommand<object>((p) => { return true; }, (p) =>
             {
                 DateTime a = new DateTime(DateFlight.Year, DateFlight.Month, DateFlight.Day);
                 DateTime b = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                 if (a < b)
                 {
                     MessageBox.Show("Ngày bay không hợp lệ!", "Cảnh báo");
                     DateFlight = DateTime.Now;
                 }
             });

            SelectedFlightTimeChanged_Command = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                // TEST: Cho mục đích test
                // Rỗng

                // Thực tế
                /*DateTime a = new DateTime(DateFlight.Year, DateFlight.Month, DateFlight.Day);
                DateTime b = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                if (a != b)
                    return;
                DateTime c = new DateTime(DateFlight.Year, DateFlight.Month, DateFlight.Day, TimeFlight.Hour, TimeFlight.Minute, TimeFlight.Second);
                DateTime d = new DateTime(DateFlight.Year, DateFlight.Month, DateFlight.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                if (c < d)
                {
                    MessageBox.Show("Thời gian không hợp lệ!", "Cảnh báo");
                    TimeFlight = DateTime.Now;
                }*/
            });
            CheckFlightTime = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (string.IsNullOrEmpty(FlightTime))
                    return;
                using (var context = new FlightTicketSellEntities())
                {
                    try
                    {
                        var minFlightTime = context.THAMSOes.Where(h => h.TenThamSo == "ThoiGianBayToiThieu").ToList().FirstOrDefault().GiaTri;
                        if (int.Parse(FlightTime) < minFlightTime)
                        {
                            MessageBox.Show("Thời gian bay tối thiểu là " + minFlightTime.ToString() + " phút!", "Cảnh báo");
                            FlightTime = string.Empty;
                            return;
                        }
                    }
                    catch (EntityException e)
                    {
                        MessageBox.Show("Database access failed!", string.Format($"Exception: {e.Message}"), MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            });
            #endregion

            #region LayoverAirport
            Open_Window_EnterLayoverAirport_Command = new RelayCommand<object>((p) => { return true; },
             async (p) =>
               {
                   LayoverAirport_StopTime = null;
                   LayoverAirport_Note = null;
                   try
                   {
                       using (var context = new FlightTicketSellEntities())
                       {
                           var temp = context.THAMSOes.Where(h => h.TenThamSo == "SoSanBayTrungGianToiDa").FirstOrDefault().GiaTri;
                           if (List_LayoverAirport.Count() == temp)
                           {
                               MessageBox.Show("Số sân bay trung gian đạt tối đa!", "Cảnh báo");
                               return;
                           }
                       }
                   }
                   catch (System.Data.Entity.Core.EntityException e)
                   {
                       MessageBox.Show($"Exception: {e.Message}");
                   }
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
                              MessageBox.Show("Thời gian dừng không hợp lệ!\nThời gian dừng từ " + LayoverAirport_MinStopTime.ToString() + " phút đến " + LayoverAirport_MaxStopTime.ToString() + " phút.", "Cảnh báo");
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
                  if (List_LayoverAirport_SelectedItem == null)
                  {
                      MessageBox.Show("Hãy chọn sân bay muốn chỉnh sửa!", "Cảnh báo");
                      return;
                  }

                  EditLayoverAirportView editLayoverAirportView = new EditLayoverAirportView { DataContext = this };
                  var result = await DialogHost.Show(editLayoverAirportView, "RootDialog");
              }
           );
            EditLayoverAirport_Save_Command = new RelayCommand<object>((p) => { return true; },
              (p) =>
              {
                  using (var context = new FlightTicketSellEntities())
                  {
                      try
                      {
                          if (!string.IsNullOrEmpty(EditLayoverAirport_StopTime) || !string.IsNullOrEmpty(EditLayoverAirport_Note))
                          {
                              if (!string.IsNullOrEmpty(EditLayoverAirport_StopTime))
                              {
                                  var LayoverAirport_MinStopTime = context.THAMSOes.Where(h => h.TenThamSo == "ThoiGianDungToiThieu").ToList().FirstOrDefault().GiaTri;
                                  var LayoverAirport_MaxStopTime = context.THAMSOes.Where(h => h.TenThamSo == "ThoiGianDungToiDa").ToList().FirstOrDefault().GiaTri;
                                  var stoptime = int.Parse(EditLayoverAirport_StopTime);
                                  if (stoptime < LayoverAirport_MinStopTime || stoptime > LayoverAirport_MaxStopTime)
                                  {
                                      MessageBox.Show("Thời gian dừng mới không hợp lệ!\nThời gian dừng từ " + LayoverAirport_MinStopTime.ToString() + " phút đến " + LayoverAirport_MaxStopTime.ToString() + " phút.", "Cảnh báo");
                                      EditLayoverAirport_StopTime = null;
                                      return;
                                  }
                                  List_LayoverAirport.Where(h => h.Id_Airport == List_LayoverAirport_SelectedItem.Id_Airport).ToList().FirstOrDefault().StopTime = int.Parse(EditLayoverAirport_StopTime);
                              }

                              if (!string.IsNullOrEmpty(EditLayoverAirport_Note))
                                  List_LayoverAirport.Where(h => h.Id_Airport == List_LayoverAirport_SelectedItem.Id_Airport).ToList().FirstOrDefault().Note = EditLayoverAirport_Note;

                              // Close dialog
                              DialogHost.CloseDialogCommand.Execute(null, null);
                          }
                          else
                          {
                              MessageBox.Show("Vui lòng nhập ít nhất một thông tin!", "Cảnh báo");
                          }
                      }
                      catch (EntityException e)
                      {
                          MessageBox.Show("Database access failed!", string.Format($"Exception: {e.Message}"), MessageBoxButton.OK, MessageBoxImage.Error);
                      }
                  }
              }
           );
            EditLayoverAirport_Cancel_Command = new RelayCommand<object>((p) => { return true; },
              (p) =>
              {
                  // Close dialog
                  DialogHost.CloseDialogCommand.Execute(null, null);
              }
           );
            Delete_LayoverAirport_Command = new RelayCommand<object>((p) => { return true; },
              (p) =>
              {
                  if (List_LayoverAirport.Count < 1)
                  {
                      MessageBox.Show("Hãy thêm sân bay trung gian trước khi xóa!", "Cảnh báo");
                      return;
                  }
                  if (List_LayoverAirport_SelectedItem == null)
                  {
                      MessageBox.Show("Hãy chọn sân bay muốn xóa!", "Cảnh báo");
                      return;
                  }
                  var temp = List_LayoverAirport.Where(h => h.Id_Airport == List_LayoverAirport_SelectedItem.Id_Airport).ToList().FirstOrDefault();
                  List_LayoverAirport.Remove(temp);
                  for (int i = 0; i < List_LayoverAirport.Count; i++)
                  {
                      List_LayoverAirport[i].Order = i + 1;
                  }
                  MessageBox.Show("Xóa sân bay trung gian thành công!", "Cảnh báo");
              }
           );


            #endregion

            #region TicketClass
            Open_Window_EnterTicketClass_Command = new RelayCommand<object>((p) => { return true; },
               async (p) =>
               {
                   EnterTicketClass_Seats = null;
                   using (var context = new FlightTicketSellEntities())
                   {
                       try
                       {
                           if (List_TicketClass.Count == context.HANGVEs.ToList().Count())
                           {
                               MessageBox.Show("Bạn đã thêm tất cả hạng vé!", "Cảnh báo");
                               return;
                           }
                       }
                       catch (EntityException e)
                       {

                           MessageBox.Show($"Exception: {e.Message}");
                       }

                   }
                   EnterTicketClassView enterTicketClassView = new EnterTicketClassView { DataContext = this };
                   var result = await DialogHost.Show(enterTicketClassView, "RootDialog");
               }
            );
            EnterTicketClass_LoadCommand = new RelayCommand<object>((p) => { return true; },
                (p) =>
               {
                   try
                   {
                       using (var context = new FlightTicketSellEntities())
                       {
                           EnterTicketClass_List_TicketClass = new ObservableCollection<TicketClass>
                           (
                               context.HANGVEs.Select(h => new TicketClass
                               {
                                   Id = h.MaHangVe,
                                   Name = h.TenHangVe,
                                   Status = h.TrangThai,
                                   Coefficient = (double)h.HeSo

                               }).Where(k => k.Status != false).ToList()
                           );
                           if (List_TicketClass == null)
                               List_TicketClass = new ObservableCollection<TicketClassDetail>();
                           if (List_TicketClass.Count > 0)
                           {
                               foreach (var item in List_TicketClass)
                               {
                                   var temp = EnterTicketClass_List_TicketClass.Where(h => h.Id == item.Id_TicketClass).ToList().FirstOrDefault();
                                   EnterTicketClass_List_TicketClass.Remove(temp);
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
            EnterTicketClass_Save_Command = new RelayCommand<object>((p) => { return true; },
                (p) =>
                {
                    if (EnterTicketClass_TicketClass_SelectedItem == null)
                    {
                        MessageBox.Show("Bạn chưa chọn hạng vé!", "Cảnh báo");
                        return;
                    }
                    if (string.IsNullOrEmpty(EnterTicketClass_Seats))
                    {
                        MessageBox.Show("Bạn chưa nhập số ghế!", "Cảnh báo");
                        return;
                    }

                    List_TicketClass.Add
                    (
                        new TicketClassDetail
                        {
                            Id_TicketClass = EnterTicketClass_TicketClass_SelectedItem.Id,
                            TicketClassName = EnterTicketClass_TicketClass_SelectedItem.Name,
                            Seats = int.Parse(EnterTicketClass_Seats),
                            TicketClassCoefficient = EnterTicketClass_TicketClass_SelectedItem.Coefficient,

                        }
                    );
                    // Close dialog
                    DialogHost.CloseDialogCommand.Execute(null, null);
                }
            );
            EnterTicketClass_Cancel_Command = new RelayCommand<object>((p) => { return true; },
               (p) =>
               {
                   // Close dialog
                   DialogHost.CloseDialogCommand.Execute(null, null);
               }
           );
            Open_Window_EditTicketClass_Command = new RelayCommand<object>((p) => { return true; },
               async (p) =>
               {
                   if (List_TicketClass_SelectedItem == null)
                   {
                       MessageBox.Show("Hãy chọn hạng vé muốn chỉnh sửa!", "Cảnh báo");
                       return;
                   }
                   EditTicketClass_Seats = null;
                   EditEnteredTicketClassView editEnteredTicketClassView = new EditEnteredTicketClassView { DataContext = this };
                   var result = await DialogHost.Show(editEnteredTicketClassView, "RootDialog");
               }
           );
            EditTicketClass_Save_Command = new RelayCommand<object>((p) => { return true; },
                (p) =>
               {
                   if (string.IsNullOrEmpty(EditTicketClass_Seats))
                   {
                       MessageBox.Show("Hãy nhập số ghế mới!", "Cảnh báo");
                       return;
                   }
                   List_TicketClass.Where(h => h.Id_TicketClass == List_TicketClass_SelectedItem.Id_TicketClass).ToList().FirstOrDefault().Seats = int.Parse(EditTicketClass_Seats);
                   // Close dialog
                   DialogHost.CloseDialogCommand.Execute(null, null);
               }
           );
            EditTicketClass_Cancel_Command = new RelayCommand<object>((p) => { return true; },
                (p) =>
               {
                   // Close dialog
                   DialogHost.CloseDialogCommand.Execute(null, null);
               }
           );
            Delete_EnteredTicketClass_Command = new RelayCommand<object>((p) => { return true; },
                (p) =>
                {
                    if (List_TicketClass.Count < 1)
                    {
                        MessageBox.Show("Hãy thêm hạng vé trước khi xóa!", "Cảnh báo");
                        return;
                    }
                    if (List_TicketClass_SelectedItem == null)
                    {
                        MessageBox.Show("Hãy chọn hạng vé muốn xóa!", "Cảnh báo");
                        return;
                    }
                    var temp = List_TicketClass.Where(h => h.Id_TicketClass == List_TicketClass_SelectedItem.Id_TicketClass).ToList().FirstOrDefault();
                    List_TicketClass.Remove(temp);
                    MessageBox.Show("Xóa hạng vé thành công!", "Cảnh báo");
                }
           );
            #endregion
        }
    }
}
