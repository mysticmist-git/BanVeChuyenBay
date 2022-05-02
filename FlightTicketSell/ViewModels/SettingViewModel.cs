using FlightTicketSell.Models;
using FlightTicketSell.Views;
using System.Windows.Input;
using System.Linq;
using System.Collections.ObjectModel;
using FlightTicketSell.ViewModels.Setting;
using System.Data.Entity.Core;
using System.Windows;
using FlightTicketSell.Views.SettingViewRelated;
using MaterialDesignThemes.Wpf;

namespace FlightTicketSell.ViewModels
{
    public class SettingViewModel : BaseViewModel
    {
        #region Main Commands

        /// <summary>
        /// Lưu thông tin chuyến bay
        /// </summary>
        public ICommand Save_FlightRegulations_Command { get; set; }

        /// <summary>
        /// Load lớn cả giao diện
        /// </summary>
        public ICommand LoadCommand { get; set; }
        #endregion

        #region Main Public Properties


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
        public Airport Airport_selecteditem { get; set; }
        /// <summary>
        /// Danh sách hạng vé
        /// </summary>
        public ObservableCollection<TicketClass> List_TicketClass { get; set; } = new ObservableCollection<TicketClass>();
        public TicketClass TicketClass_selecteditem { get; set; }

        #endregion

        #region Airport Commands
        /// <summary>
        /// Mở thêm sân bay
        /// </summary>
        public ICommand MoreAirport_Command { get; set; }
        /// <summary>
        /// Lưu khi thêm sân bay
        /// </summary>
        public ICommand MoreAirport_Save_Button_Command { get; set; }
        /// <summary>
        /// Mở sửa sân bay
        /// </summary>
        public ICommand EditAirport_Command { get; set; }
        /// <summary>
        /// Lưu khi sửa sân bay
        /// </summary>
        public ICommand EditAirport_Save_Button_Command { get; set; }
        /// <summary>
        /// Xóa sân bay
        /// </summary>
        public ICommand DeleteAirport_Command { get; set; }
        #endregion

        #region Airport Public Properties
        /// <summary>
        /// Tên sân bay
        /// </summary>
        public string MoreAirport_Name { get; set; }

        /// <summary>
        /// Viết tắt
        /// </summary>
        public string MoreAirport_Code { get; set; }

        /// <summary>
        /// Tỉnh thành
        /// </summary>
        public string MoreAirport_Province { get; set; }
        /// <summary>
        /// Tên mới sân bay
        /// </summary>
        public string EditAirport_Name { get; set; }

        /// <summary>
        /// Tỉnh thành mới sân bay
        /// </summary>
        public string EditAirport_Province { get; set; }
        #endregion

        #region TicketClass Commands
        /// <summary>
        /// Mở thêm hạng vé
        /// </summary>
        public ICommand MoreTicketClass_Command { get; set; }
        /// <summary>
        /// Lưu khi sửa sân bay
        /// </summary>
        public ICommand MoreTicketClass_Save_Button_Command { get; set; }
        /// <summary>
        /// Sửa hạng vé
        /// </summary>
        public ICommand EditTicketClass_Command { get; set; }
        /// <summary>
        /// Lưu khi sửa sân bay
        /// </summary>
        public ICommand EditTicketClass_Save_Button_Command { get; set; }
        /// <summary>
        /// Xóa hạng vé
        /// </summary>
        public ICommand DeleteTicketClass_Command { get; set; }
      
        
        #endregion

        #region TicketClass Public Properties

        /// <summary>
        /// Tên hạng vé
        /// </summary>
        public string MoreTicketClass_Name { get; set; }

        /// <summary>
        /// Hệ số
        /// </summary>
        public double MoreTicketClass_Coefficien { get; set; }
        /// <summary>
        /// Tên hạng vé
        /// </summary>
        public string EditTicketClass_Name { get; set; }

        /// <summary>
        /// Hệ số
        /// </summary>
        public string EditTicketClass_Coefficien { get; set; }
        #endregion

        #region Method
        /// <summary>
        /// Load lại danh sách sân bay
        /// </summary>
        private void ReLoadList_Airport()
        {
            using (var context = new FlightTicketSellEntities())
            {
                try
                {
                    // Danh sách sân bay
                    if (List_Airport != null)
                    {
                        List_Airport.Clear();
                    }
                    foreach (var item in context.SANBAYs.ToList())
                    {
                        //Sân bay còn hoạt động mới thêm vào list
                        if (item.TrangThai)
                            List_Airport.Add(new Airport() { Id = item.MaSanBay, Code = item.VietTat, Name = item.TenSanBay, Province = item.TinhThanh, Status = item.TrangThai });
                    }
                }
                catch (EntityException e)
                {
                    MessageBox.Show("Database access failed!", string.Format($"Exception: {e.Message}"), MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Load lại danh sách hạng vé
        /// </summary>
        private void ReLoadList_TicketClass()
        {
            using (var context = new FlightTicketSellEntities())
            {
                try
                {
                    // Danh sách sân bay
                    if (List_TicketClass != null)
                    {
                        List_TicketClass.Clear();
                    }
                    foreach (var item in context.HANGVEs.ToList())
                    {
                        //Sân bay còn hoạt động mới thêm vào list
                        if (item.TrangThai)
                            List_TicketClass.Add(new TicketClass() { Id = item.MaHangVe, Name = item.TenHangVe, Coefficient = (double)item.HeSo, Status = item.TrangThai });
                    }
                }
                catch (EntityException e)
                {
                    MessageBox.Show("Database access failed!", string.Format($"Exception: {e.Message}"), MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        #endregion

        public SettingViewModel()
        {
            #region Main Commands
            
            LoadCommand = new RelayCommand<object>(
                (p) => { return true; },
                (p) =>
                {
                    using(var context = new FlightTicketSellEntities())
                    {
                        try
                        {
                            // Các quy định về chuyến bay
                            Max_LayoverAirport = (context.THAMSOes.ToList()).Where(h => h.TenThamSo == "SoSanBayTrungGianToiDa").FirstOrDefault().GiaTri;
                            Min_FlightTime = (context.THAMSOes.ToList()).Where(h => h.TenThamSo == "ThoiGianBayToiThieu").FirstOrDefault().GiaTri;
                            Latest_BookingTime = (context.THAMSOes.ToList()).Where(h => h.TenThamSo == "ThoiGianDatVeChamNhat").FirstOrDefault().GiaTri;
                            Cancel_BookingTime = (context.THAMSOes.ToList()).Where(h => h.TenThamSo == "ThoiGianHuyDatVe").FirstOrDefault().GiaTri;
                            Min_TimeStop = (context.THAMSOes.ToList()).Where(h => h.TenThamSo == "ThoiGianDungToiThieu").FirstOrDefault().GiaTri;
                            Max_TimeStop = (context.THAMSOes.ToList()).Where(h => h.TenThamSo == "ThoiGianDungToiDa").FirstOrDefault().GiaTri;

                            // Danh sách sân bay
                            if (List_Airport != null)
                            {
                                List_Airport.Clear();
                            }
                            foreach (var item in context.SANBAYs.ToList())
                            {
                                //Sân bay còn hoạt động mới thêm vào list
                                if (item.TrangThai)
                                    List_Airport.Add(new Airport() { Id = item.MaSanBay, Code = item.VietTat, Name = item.TenSanBay, Province = item.TinhThanh, Status = item.TrangThai });
                            }

                            // Danh sách hạng vé
                            if (List_TicketClass != null)
                            {
                                List_TicketClass.Clear();
                            }
                            foreach (var item in context.HANGVEs.ToList())
                            {
                                //Hạng vé còn áp dụng mới thêm vào list
                                if (item.TrangThai)
                                    List_TicketClass.Add(new TicketClass() { Id = item.MaHangVe, Name = item.TenHangVe, Coefficient = (double)item.HeSo, Status=item.TrangThai });
                            }

                        }
                        catch (EntityException e)
                        {
                            MessageBox.Show("Database access failed!", string.Format($"Exception: {e.Message}"), MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }

                }
            );

           
          

            Save_FlightRegulations_Command = new RelayCommand<object>(
               (p) => { return true; },
               (p) =>
               {
                   using (var context = new FlightTicketSellEntities())
                   {
                       try
                       {
                           context.THAMSOes.Where(h => h.TenThamSo == "SoSanBayTrungGianToiDa").FirstOrDefault().GiaTri = Max_LayoverAirport;
                           context.THAMSOes.Where(h => h.TenThamSo == "ThoiGianBayToiThieu").FirstOrDefault().GiaTri = Min_FlightTime;
                           context.THAMSOes.Where(h => h.TenThamSo == "ThoiGianDungToiThieu").FirstOrDefault().GiaTri = Min_TimeStop;
                           context.THAMSOes.Where(h => h.TenThamSo == "ThoiGianDungToiDa").FirstOrDefault().GiaTri = Max_TimeStop;
                           context.THAMSOes.Where(h => h.TenThamSo == "ThoiGianDatVeChamNhat").FirstOrDefault().GiaTri = Latest_BookingTime;
                           context.THAMSOes.Where(h => h.TenThamSo == "ThoiGianHuyDatVe").FirstOrDefault().GiaTri = Cancel_BookingTime;
                           context.SaveChanges();
                           MessageBox.Show("Lưu thành công!");
                       }
                       catch (EntityException e)
                       {
                           MessageBox.Show("Database access failed!", string.Format($"Exception: {e.Message}"), MessageBoxButton.OK, MessageBoxImage.Error);
                       }
                   }
               }
           );

          

           
            #endregion

            #region Aiport Commands

            MoreAirport_Command = new RelayCommand<object>(
               (p) => true,
               async (p) =>
               {
                   MoreAirportView moreAirportView = new MoreAirportView { DataContext = this };
                   var result = await DialogHost.Show(moreAirportView, "RootDialog");

                    //Load lại danh sách sân bay
                    ReLoadList_Airport();
               }
           );

            MoreAirport_Save_Button_Command = new RelayCommand<object>(
               (p) => { return true; },
               (p) =>
               {
                   bool IsRun = false;
                   using (var context = new FlightTicketSellEntities())
                   {
                       try
                       {
                           var list_airport = context.SANBAYs.ToList();
                           if (!(MoreAirport_Name != null && MoreAirport_Code != null && MoreAirport_Province != null))
                           {
                               MessageBox.Show("Vui lòng nhập đầy đủ thông tin sân bay!", "Cảnh báo");
                               return;
                           }
                           if (list_airport.Where(h => h.VietTat == MoreAirport_Code).FirstOrDefault() != null)
                           {
                               MessageBox.Show("Mã sân bay đã tồn tại!", "Cảnh báo");
                               MoreAirport_Code = "";
                               return;
                           }
                           if (list_airport.Where(h => h.TenSanBay == MoreAirport_Name).FirstOrDefault() != null)
                           {
                               MessageBox.Show("Tên sân bay đã tồn tại!", "Cảnh báo");
                               MoreAirport_Name = "";
                               return;
                           }

                           SANBAY temp = new SANBAY() { TenSanBay = MoreAirport_Name, TrangThai = true, TinhThanh = MoreAirport_Province, VietTat = MoreAirport_Code };
                           context.SANBAYs.Add(temp);
                           context.SaveChanges();
                           MessageBox.Show("Thêm sân bay thành công!", "Cảnh báo");
                           IsRun = true;

                       }
                       catch (EntityException e)
                       {
                           MessageBox.Show("Database access failed!", string.Format($"Exception: {e.Message}"), MessageBoxButton.OK, MessageBoxImage.Error);
                       }
                   }

                   if (IsRun)
                   {
                        // Close dialog
                        DialogHost.CloseDialogCommand.Execute(null, null);
                   }

               }
           );

            EditAirport_Command = new RelayCommand<object>(
            (p) => { return true; },
           async (p) =>
           {
               if (Airport_selecteditem != null)
               {
                   EditAirportView editAirportView = new EditAirportView()
                   { DataContext = this };
                   var result = await DialogHost.Show(editAirportView, "RootDialog");

                   //Load lại danh sách sân bay
                   ReLoadList_Airport();
               }
               else
               {
                   MessageBox.Show("Vui lòng chọn sân bay muốn chỉnh sửa!", "Cảnh báo");
               }
           }
        );

            EditAirport_Save_Button_Command = new RelayCommand<object>(
              (p) => { return true; },
              (p) =>
              {
                  using (var context = new FlightTicketSellEntities())
                  {
                      try
                      {
                          if (EditAirport_Name != null || EditAirport_Province != null)
                          {

                              if (EditAirport_Name != null)
                                  context.SANBAYs.ToList().Where(s => s.MaSanBay == Airport_selecteditem.Id).FirstOrDefault().TenSanBay = EditAirport_Name;
                              if (EditAirport_Province != null)
                                  context.SANBAYs.ToList().Where(s => s.MaSanBay == Airport_selecteditem.Id).FirstOrDefault().TinhThanh = EditAirport_Province;

                              context.SaveChanges();
                              MessageBox.Show("Lưu thay đổi sân bay thành công!", "Cảnh báo");

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

            DeleteAirport_Command = new RelayCommand<object>(
            (p) => { return true; },
            (p) =>
            {
                using (var context = new FlightTicketSellEntities())
                {
                    try
                    {
                        if (Airport_selecteditem != null)
                        {
                              //Kiểm tra chắc chắn muốn xóa
                              MessageBoxResult messageBoxResult = MessageBox.Show("Bạn chắn chắn muốn xóa sân bay?", "Cảnh báo", MessageBoxButton.YesNo);
                            if (messageBoxResult == MessageBoxResult.No)
                                return;

                            var list = context.SANBAYs.ToList();
                            SANBAY temp = list.Where(h => h.MaSanBay == Airport_selecteditem.Id).FirstOrDefault();
                            if (temp != null)
                            {
                                temp.TrangThai = false;
                                context.SaveChanges();
                                MessageBox.Show($"Xóa sân bay {Airport_selecteditem.Code} - {Airport_selecteditem.Name} - {Airport_selecteditem.Province}\nthành công!");

                                  //Load lại danh sách sân bay
                                  ReLoadList_Airport();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Vui lòng chọn 1 sân bay!", "Cảnh báo");
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

            #region TicketClass Commands

            MoreTicketClass_Command = new RelayCommand<object>(
              (p) => { return true; },
              async (p) =>
              {
                  MoreTicketClassView moreTicketClassView = new MoreTicketClassView { DataContext = this };
                  var result = await DialogHost.Show(moreTicketClassView, "RootDialog");

                    // Load lại danh sách hạng vé
                    ReLoadList_TicketClass();
              }
          );

            MoreTicketClass_Save_Button_Command = new RelayCommand<object>((p) => { return true; },
               (p) =>
               {
                   using (var context = new FlightTicketSellEntities())
                   {
                       try
                       {
                           var list_ticketclass = context.HANGVEs.ToList();
                           if (string.IsNullOrEmpty(MoreTicketClass_Name) && MoreTicketClass_Coefficien <= 0)
                           {
                               MessageBox.Show("Vui lòng nhập đầy đủ thông tin hạng vé!", "Cảnh báo");
                               return;
                           }
                           if (list_ticketclass.Where(h => h.TenHangVe == MoreTicketClass_Name).FirstOrDefault() != null)
                           {
                               MessageBox.Show("Tên hạng vé đã tồn tại!", "Cảnh báo");
                               MoreTicketClass_Name = "";
                               return;
                           }
                           HANGVE temp = new HANGVE() { TenHangVe = MoreTicketClass_Name, TrangThai = true, HeSo = (decimal)MoreTicketClass_Coefficien };
                           context.HANGVEs.Add(temp);
                           context.SaveChanges();
                           MessageBox.Show("Thêm hạng vé thành công!", "Cảnh báo");

                       }
                       catch (EntityException e)
                       {
                           MessageBox.Show("Database access failed!", string.Format($"Exception: {e.Message}"), MessageBoxButton.OK, MessageBoxImage.Error);
                       }
                   }
               });

            EditTicketClass_Command = new RelayCommand<object>(
             (p) => { return true; },
             async (p) =>
             {
                 if (TicketClass_selecteditem != null)
                 {
                     EditTicketClassView editTicketClassView = new EditTicketClassView { DataContext = this };
                     var result = await DialogHost.Show(editTicketClassView, "RootDialog");

                     // Load lại danh sách hạng vé
                     ReLoadList_TicketClass();
                 }
                 else
                 {
                     MessageBox.Show("Vui lòng chọn sân bay muốn chỉnh sửa!", "Cảnh báo");
                 }
             }
         );

            EditTicketClass_Save_Button_Command = new RelayCommand<object>((p) => { return true; },
               (p) =>
               {
                   using (var context = new FlightTicketSellEntities())
                   {
                       try
                       {
                           if ( !string.IsNullOrEmpty(EditTicketClass_Name) || !string.IsNullOrEmpty(EditTicketClass_Coefficien))
                           {

                               if (!string.IsNullOrEmpty(EditTicketClass_Name))
                                   context.HANGVEs.ToList().Where(s => s.MaHangVe == TicketClass_selecteditem.Id).FirstOrDefault().TenHangVe = EditTicketClass_Name;
                               if (!string.IsNullOrEmpty(EditTicketClass_Coefficien))
                                   context.HANGVEs.ToList().Where(s => s.MaHangVe == TicketClass_selecteditem.Id).FirstOrDefault().HeSo = (decimal)(double.Parse(EditTicketClass_Coefficien));

                               context.SaveChanges();
                               MessageBox.Show("Lưu thay đổi hạng vé thành công!", "Cảnh báo");
                              
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
               });


            DeleteTicketClass_Command = new RelayCommand<object>(
             (p) => { return true; },
             (p) =>
             {
                 using (var context = new FlightTicketSellEntities())
                 {
                     try
                     {
                         if (TicketClass_selecteditem != null)
                         {
                             //Kiểm tra chắc chắn muốn xóa
                             MessageBoxResult messageBoxResult = MessageBox.Show("Bạn chắn chắn muốn xóa hạng vé?", "Cảnh báo", MessageBoxButton.YesNo);
                             if (messageBoxResult == MessageBoxResult.No)
                                 return;

                             var list = context.HANGVEs.ToList();
                             HANGVE temp = list.Where(h => h.MaHangVe == TicketClass_selecteditem.Id).FirstOrDefault();
                             if (temp != null)
                             {
                                 temp.TrangThai = false;
                                 context.SaveChanges();
                                 MessageBox.Show($"Xóa hạng vé {TicketClass_selecteditem.Name} - {TicketClass_selecteditem.Coefficient}\nthành công!");

                                 //Load lại danh sách sân bay
                                 ReLoadList_TicketClass();
                             }
                         }
                         else
                         {
                             MessageBox.Show("Vui lòng chọn 1 hạng vé!", "Cảnh báo");
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
