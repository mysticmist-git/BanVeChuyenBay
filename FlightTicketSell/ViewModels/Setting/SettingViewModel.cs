using FlightTicketSell.Models;
using FlightTicketSell.Views;
using System.Windows.Input;
using System.Linq;
using System.Collections.ObjectModel;
using FlightTicketSell.ViewModels.Setting;
using System.Data.Entity.Core;
using System.Windows;
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
        /// Hàm lostfocus các textbox quy định
        /// </summary>
        public ICommand LostFocusTextBox { get; set; }
        /// <summary>
        /// Load lớn cả giao diện
        /// </summary>
        public ICommand LoadCommand { get; set; }
        #endregion

        #region Main Public Properties
        /// <summary>
        /// Số sân bay trung gian tối đa
        /// </summary>
        public string Max_LayoverAirport { get; set; }

        /// <summary>
        /// Thời gian bay tối thiểu
        /// </summary>
        public string Min_FlightTime { get; set; }

        /// <summary>
        /// Thời gian đặt vé chậm nhất
        /// </summary>
        public string Latest_BookingTime { get; set; }

        /// <summary>
        /// Thời gian hủy đặt chỗ
        /// </summary>
        public string Cancel_BookingTime { get; set; }

        /// <summary>
        /// Thời gian dừng tối thiểu
        /// </summary>
        public string Min_TimeStop { get; set; }

        /// <summary>
        /// Thời gian dừng tối đa
        /// </summary>
        public string Max_TimeStop { get; set; }

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
        /// Hủy khi thêm sân bay
        /// </summary>
        public ICommand MoreAirport_Cancel_Button_Command { get; set; }
        /// <summary>
        /// Mở sửa sân bay
        /// </summary>
        public ICommand EditAirport_Command { get; set; }
        /// <summary>
        /// Lưu khi sửa sân bay
        /// </summary>
        public ICommand EditAirport_Save_Button_Command { get; set; }
        /// <summary>
        /// Hủy khi sửa sân bay
        /// </summary>
        public ICommand EditAirport_Cancel_Button_Command { get; set; }
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
        public string IsAirportUsed
        {
            get
            {
                using (var context = new FlightTicketSellEntities())
                {
                    try
                    {
                        if (Airport_selecteditem != null)
                        {
                            var exist1 = context.SANBAYTGs.Where(h => h.MaSanBay == Airport_selecteditem.Id).FirstOrDefault();
                            var exist2 = context.DUONGBAYs.Where(h => h.MaSanBayDi == Airport_selecteditem.Id || h.MaSanBayDen == Airport_selecteditem.Id).FirstOrDefault();
                            var isUsing = context.SANBAYs.Where(h => h.MaSanBay == Airport_selecteditem.Id).FirstOrDefault().TrangThai;
                            if (exist1 != null || exist2 != null)
                            {
                                if (isUsing)
                                {
                                    return "Ngừng hoạt động";
                                }
                                else
                                {
                                    return "Tái hoạt động";
                                }
                                
                            }
                            else
                            {
                                return "Xóa";
                            }
                                
                        }
                    }
                    catch (EntityException e)
                    {
                        MessageBox.Show("Database access failed!", string.Format($"Exception: {e.Message}"), MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                return "Xóa";
            }
        }
        #endregion

        #region TicketClass Commands
        /// <summary>
        /// Mở thêm hạng vé
        /// </summary>
        public ICommand MoreTicketClass_Command { get; set; }
        /// <summary>
        /// Lưu khi thêm hạng vé
        /// </summary>
        public ICommand MoreTicketClass_Save_Button_Command { get; set; }
        /// <summary>
        /// Hủy khi thêm hạng vé
        /// </summary>
        public ICommand MoreTicketClass_Cancel_Button_Command { get; set; }
        /// <summary>
        /// Sửa hạng vé
        /// </summary>
        public ICommand EditTicketClass_Command { get; set; }
        /// <summary>
        /// Lưu khi sửa hạng vé
        /// </summary>
        public ICommand EditTicketClass_Save_Button_Command { get; set; }
        /// <summary>
        /// Lưu khi sửa hạng vé
        /// </summary>
        public ICommand EditTicketClass_Cancel_Button_Command { get; set; }
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
        public string MoreTicketClass_Coefficien { get; set; }
        /// <summary>
        /// Tên hạng vé
        /// </summary>
        public string EditTicketClass_Name { get; set; }

        /// <summary>
        /// Hệ số
        /// </summary>
        public string EditTicketClass_Coefficien { get; set; }
        /// <summary>
        /// Biến bool
        /// </summary>
        public string IsTicketUsed
        {
            get
            {
                using (var context = new FlightTicketSellEntities())
                {
                    try
                    {
                        if (TicketClass_selecteditem != null)
                        {
                            var exist = context.CHITIETHANGVEs.Where(h => h.MaHangVe == TicketClass_selecteditem.Id).FirstOrDefault();
                            var isprovide = context.HANGVEs.Where(h => h.MaHangVe == TicketClass_selecteditem.Id).FirstOrDefault().TrangThai;
                            if (exist != null)
                            {
                                if (isprovide)
                                {
                                    return "Ngừng cung cấp";
                                }
                                else
                                {
                                    return "Tái cung cấp";
                                }
                            }
                            else
                            {
                                return "Xóa";
                            }

                        }
                    }
                    catch (EntityException e)
                    {
                        MessageBox.Show("Database access failed!", string.Format($"Exception: {e.Message}"), MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                return "Xóa";
            }
        }
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
                    using (var context = new FlightTicketSellEntities())
                    {
                        try
                        {
                            // Các quy định về chuyến bay
                            Max_LayoverAirport = (context.THAMSOes.ToList()).Where(h => h.TenThamSo == "SoSanBayTrungGianToiDa").FirstOrDefault().GiaTri.ToString();
                            Min_FlightTime = (context.THAMSOes.ToList()).Where(h => h.TenThamSo == "ThoiGianBayToiThieu").FirstOrDefault().GiaTri.ToString();
                            Latest_BookingTime = (context.THAMSOes.ToList()).Where(h => h.TenThamSo == "ThoiGianDatVeChamNhat").FirstOrDefault().GiaTri.ToString();
                            Cancel_BookingTime = (context.THAMSOes.ToList()).Where(h => h.TenThamSo == "ThoiGianHuyDatVe").FirstOrDefault().GiaTri.ToString();
                            Min_TimeStop = (context.THAMSOes.ToList()).Where(h => h.TenThamSo == "ThoiGianDungToiThieu").FirstOrDefault().GiaTri.ToString();
                            Max_TimeStop = (context.THAMSOes.ToList()).Where(h => h.TenThamSo == "ThoiGianDungToiDa").FirstOrDefault().GiaTri.ToString();

                            // Danh sách sân bay
                            if (List_Airport != null)
                            {
                                List_Airport.Clear();
                            }
                            foreach (var item in context.SANBAYs.ToList())
                            {
                                List_Airport.Add(new Airport() { Id = item.MaSanBay, Code = item.VietTat, Name = item.TenSanBay, Province = item.TinhThanh, Status = item.TrangThai });
                            }

                            // Danh sách hạng vé
                            if (List_TicketClass != null)
                            {
                                List_TicketClass.Clear();
                            }
                            foreach (var item in context.HANGVEs.ToList())
                            {
                                List_TicketClass.Add(new TicketClass() { Id = item.MaHangVe, Name = item.TenHangVe, Coefficient = (double)item.HeSo, Status = item.TrangThai });
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
                           if (string.IsNullOrEmpty(Max_LayoverAirport.ToString()))
                           {
                               MessageBox.Show("Số sân bay trung gian không được trống!", "Cảnh báo");
                               Max_LayoverAirport = context.THAMSOes.Where(h => h.TenThamSo == "SoSanBayTrungGianToiDa").FirstOrDefault().GiaTri.ToString();
                               return;
                           }
                           if (string.IsNullOrEmpty(Min_FlightTime.ToString()))
                           {
                               MessageBox.Show("Thời gian bay tối thiểu không được trống!", "Cảnh báo");
                               Min_FlightTime = context.THAMSOes.Where(h => h.TenThamSo == "ThoiGianBayToiThieu").FirstOrDefault().GiaTri.ToString();
                               return;
                           }
                           if (string.IsNullOrEmpty(Min_TimeStop.ToString()))
                           {
                               MessageBox.Show("Thời gian dừng tối thiểu không được trống!", "Cảnh báo");
                               Min_TimeStop = context.THAMSOes.Where(h => h.TenThamSo == "ThoiGianDungToiThieu").FirstOrDefault().GiaTri.ToString();
                               return;
                           }
                           if (string.IsNullOrEmpty(Max_TimeStop.ToString()))
                           {
                               MessageBox.Show("Thời gian dừng tối đa không được trống!", "Cảnh báo");
                               Max_TimeStop = context.THAMSOes.Where(h => h.TenThamSo == "ThoiGianDungToiDa").FirstOrDefault().GiaTri.ToString();
                               return;
                           }
                           if (string.IsNullOrEmpty(Latest_BookingTime.ToString()))
                           {
                               MessageBox.Show("Thời gian chậm nhất đặt vé không được trống!", "Cảnh báo");
                               Latest_BookingTime = context.THAMSOes.Where(h => h.TenThamSo == "ThoiGianDatVeChamNhat").FirstOrDefault().GiaTri.ToString();
                               return;
                           }
                           if (string.IsNullOrEmpty(Cancel_BookingTime.ToString()))
                           {
                               MessageBox.Show("Thời gian hủy vé không được trống!", "Cảnh báo");
                               Cancel_BookingTime = context.THAMSOes.Where(h => h.TenThamSo == "ThoiGianHuyDatVe").FirstOrDefault().GiaTri.ToString();
                               return;
                           }
                           if (int.Parse(Max_LayoverAirport) == context.SANBAYs.ToList().Count() - 2)
                           {
                               MessageBox.Show("Số sân bay trung gian không vượt quá số sân bay trừ đi 2!", "Cảnh báo");
                               Max_LayoverAirport = context.THAMSOes.Where(h => h.TenThamSo == "SoSanBayTrungGianToiDa").FirstOrDefault().GiaTri.ToString();
                               return;
                           }
                           context.THAMSOes.Where(h => h.TenThamSo == "SoSanBayTrungGianToiDa").FirstOrDefault().GiaTri = int.Parse(Max_LayoverAirport);
                           context.THAMSOes.Where(h => h.TenThamSo == "ThoiGianBayToiThieu").FirstOrDefault().GiaTri = int.Parse(Min_FlightTime);
                           context.THAMSOes.Where(h => h.TenThamSo == "ThoiGianDungToiThieu").FirstOrDefault().GiaTri = int.Parse(Min_TimeStop);
                           context.THAMSOes.Where(h => h.TenThamSo == "ThoiGianDungToiDa").FirstOrDefault().GiaTri = int.Parse(Max_TimeStop);
                           context.THAMSOes.Where(h => h.TenThamSo == "ThoiGianDatVeChamNhat").FirstOrDefault().GiaTri = int.Parse(Latest_BookingTime);
                           context.THAMSOes.Where(h => h.TenThamSo == "ThoiGianHuyDatVe").FirstOrDefault().GiaTri = int.Parse(Cancel_BookingTime);
                           if (context.SaveChanges() > 0)
                               MessageBox.Show("Lưu thành công!", "Cảnh báo");
                           else
                               MessageBox.Show("Bạn chưa thực hiện thay đổi các quy định!", "Cảnh báo");
                       }
                       catch (EntityException e)
                       {
                           MessageBox.Show("Database access failed!", string.Format($"Exception: {e.Message}"), MessageBoxButton.OK, MessageBoxImage.Error);
                       }
                   }
               }
           );

            LostFocusTextBox = new RelayCommand<object>(
                (p) => { return true; },
                (p) =>
                {
                    using (var context = new FlightTicketSellEntities())
                    {
                        try
                        {
                            if (string.IsNullOrEmpty(Max_LayoverAirport.ToString()) || int.Parse(Max_LayoverAirport) <= 0)
                            {
                                MessageBox.Show("Thời gian bay trung gian tối đa không thể là 0.", "Cảnh báo");
                                Max_LayoverAirport = context.THAMSOes.Where(h => h.TenThamSo == "SoSanBayTrungGianToiDa").FirstOrDefault().GiaTri.ToString();
                                return;
                            }
                            if (string.IsNullOrEmpty(Min_FlightTime.ToString()) || int.Parse(Min_FlightTime) <= 0)
                            {
                                MessageBox.Show("Thời gian bay tối thiểu không thể là 0.", "Cảnh báo");
                                Min_FlightTime = context.THAMSOes.Where(h => h.TenThamSo == "ThoiGianBayToiThieu").FirstOrDefault().GiaTri.ToString();
                                return;
                            }
                            if (string.IsNullOrEmpty(Min_TimeStop.ToString()) || int.Parse(Min_TimeStop) <= 0)
                            {
                                MessageBox.Show("Thời gian dừng tối thiểu không thể là 0.", "Cảnh báo");
                                Min_TimeStop = context.THAMSOes.Where(h => h.TenThamSo == "ThoiGianDungToiThieu").FirstOrDefault().GiaTri.ToString();
                                return;
                            }
                            if (string.IsNullOrEmpty(Max_TimeStop.ToString()) || int.Parse(Max_TimeStop) <= int.Parse(Min_TimeStop))
                            {
                                MessageBox.Show("Thời gian dừng tối đa không thể nhỏ hơn thời gian dừng tối thiểu.", "Cảnh báo");
                                Max_TimeStop = context.THAMSOes.Where(h => h.TenThamSo == "ThoiGianDungToiDa").FirstOrDefault().GiaTri.ToString();
                                return;
                            }
                            if (string.IsNullOrEmpty(Latest_BookingTime.ToString()) || int.Parse(Latest_BookingTime) <= 0)
                            {
                                MessageBox.Show("Thời gian đặt vé chậm nhất không thể là 0.", "Cảnh báo");
                                Latest_BookingTime = context.THAMSOes.Where(h => h.TenThamSo == "ThoiGianDatVeChamNhat").FirstOrDefault().GiaTri.ToString();
                                return;
                            }
                            if (string.IsNullOrEmpty(Cancel_BookingTime.ToString()) || int.Parse(Cancel_BookingTime) <= 0)
                            {
                                MessageBox.Show("Thời gian hủy đặt chỗ không thể là 0.", "Cảnh báo");
                                Cancel_BookingTime = context.THAMSOes.Where(h => h.TenThamSo == "ThoiGianHuyDatVe").FirstOrDefault().GiaTri.ToString();
                                return;
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

            #region Aiport Commands

            MoreAirport_Command = new RelayCommand<object>(
               (p) => true,
               async (p) =>
               {
                   MoreAirport_Name = string.Empty;
                   MoreAirport_Code = string.Empty;
                   MoreAirport_Province = string.Empty;
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
                   using (var context = new FlightTicketSellEntities())
                   {
                       try
                       {
                           var list_airport = context.SANBAYs.ToList();
                           if (string.IsNullOrEmpty(MoreAirport_Name) || string.IsNullOrEmpty(MoreAirport_Code) || string.IsNullOrEmpty(MoreAirport_Province))
                           {
                               MessageBox.Show("Vui lòng nhập đầy đủ thông tin sân bay!", "Cảnh báo");
                               return;
                           }
                           if (list_airport.Where(h => h.VietTat == MoreAirport_Code).FirstOrDefault() != null)
                           {
                               MessageBox.Show("Mã sân bay đã tồn tại hoặc đã được sử dụng trong quá khứ!", "Cảnh báo");
                               MoreAirport_Code = "";
                               return;
                           }
                           if (list_airport.Where(h => h.TenSanBay == MoreAirport_Name).FirstOrDefault() != null)
                           {
                               MessageBox.Show("Tên sân bay đã tồn tại hoặc đã được sử dụng trong quá khứ!", "Cảnh báo");
                               MoreAirport_Name = "";
                               return;
                           }

                           SANBAY temp = new SANBAY() { TenSanBay = MoreAirport_Name, TrangThai = true, TinhThanh = MoreAirport_Province, VietTat = MoreAirport_Code };
                           context.SANBAYs.Add(temp);
                           context.SaveChanges();
                           // Close dialog
                           DialogHost.CloseDialogCommand.Execute(null, null);
                           MessageBox.Show("Thêm sân bay thành công!", "Cảnh báo");
                       }
                       catch (EntityException e)
                       {
                           MessageBox.Show("Database access failed!", string.Format($"Exception: {e.Message}"), MessageBoxButton.OK, MessageBoxImage.Error);
                       }
                   }



               }
           );

            MoreAirport_Cancel_Button_Command = new RelayCommand<object>(
               (p) => { return true; },
               (p) =>
               {
                   //Close dialog
                   DialogHost.CloseDialogCommand.Execute(null, null);
               }
           );

            EditAirport_Command = new RelayCommand<object>(
            (p) => { return true; },
           async (p) =>
           {
               if (Airport_selecteditem != null)
               {
                   EditAirport_Name = string.Empty;
                   EditAirport_Province = string.Empty;
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
                          var check = false;
                          if (!string.IsNullOrEmpty(EditAirport_Name))
                          {
                              foreach (var item in context.SANBAYs.ToList())
                              {
                                  if (item.MaSanBay != Airport_selecteditem.Id && item.TenSanBay == EditAirport_Name && item.TrangThai == true)
                                  {
                                      MessageBox.Show("Tên sân bay đã tồn tại!", "Cảnh báo");
                                      return;
                                  }
                              }
                              context.SANBAYs.ToList().Where(s => s.MaSanBay == Airport_selecteditem.Id).FirstOrDefault().TenSanBay = EditAirport_Name;
                              check = true;
                          }
                          if (!string.IsNullOrEmpty(EditAirport_Province))
                          {
                              context.SANBAYs.ToList().Where(s => s.MaSanBay == Airport_selecteditem.Id).FirstOrDefault().TinhThanh = EditAirport_Province;
                              check = true;
                          }
                          if (check)
                          {
                              context.SaveChanges();
                              // Close dialog
                              DialogHost.CloseDialogCommand.Execute(null, null);
                              MessageBox.Show("Lưu thay đổi sân bay thành công!", "Cảnh báo");
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
            EditAirport_Cancel_Button_Command = new RelayCommand<object>(
              (p) => { return true; },
              (p) =>
              {
                  //Close dialog
                  DialogHost.CloseDialogCommand.Execute(null, null);
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
                            MessageBoxResult messageBoxResult;
                            if (IsAirportUsed=="Ngừng hoạt động")
                            {
                                //Kiểm tra sân bay có là sân bay trung gian của 1 chuyến bay chưa khởi hành hay không?
                                var existSBTG = context.SANBAYTGs.Where(h => h.MaSanBay == Airport_selecteditem.Id).ToList();
                                foreach (var item in existSBTG)
                                {
                                    var check = context.CHUYENBAYs.Where(h => h.MaDuongBay == item.MaDuongBay).FirstOrDefault();
                                    if (check.DaKhoiHanh == false)
                                    {

                                        var code1 = check.DUONGBAY.SANBAY1.VietTat;
                                        var code2 = check.DUONGBAY.SANBAY.VietTat;
                                        MessageBox.Show("Không thể ngừng hoạt động sân bay.\nSân bay đang là sân bay trung gian của chuyến bay chưa cất cánh: " + code1 + code2 + "-" + check.MaChuyenBay, "Cảnh báo");
                                        return;
                                    }
                                }

                                //Kiểm tra sân bay có là sân bay đi/đến của 1 chuyến bay chưa khởi hành hay không?
                                var existDB = context.DUONGBAYs.Where(h => h.MaSanBayDi == Airport_selecteditem.Id || h.MaSanBayDen == Airport_selecteditem.Id).ToList();
                                foreach (var item in existDB)
                                {
                                    var check = context.CHUYENBAYs.Where(h => h.MaDuongBay == item.MaDuongBay).FirstOrDefault();
                                    if (check.DaKhoiHanh == false)
                                    {
                                        var code1 = check.DUONGBAY.SANBAY1.VietTat;
                                        var code2 = check.DUONGBAY.SANBAY.VietTat;
                                        MessageBox.Show("Không thể ngừng hoạt động sân bay.\nSân bay là sân bay đi/đến của chuyến bay chưa cất cánh: " + code1 + code2 + "-" + check.MaChuyenBay, "Cảnh báo");
                                        return;
                                    }
                                }

                                //Kiểm tra chắc chắn muốn ngừng hoạt động
                                messageBoxResult = MessageBox.Show("Bạn chắn chắn muốn ngừng hoạt động sân bay?", "Cảnh báo", MessageBoxButton.YesNo);
                                if (messageBoxResult == MessageBoxResult.No)
                                    return;
                                var temp = context.SANBAYs.Where(h => h.MaSanBay == Airport_selecteditem.Id).FirstOrDefault();
                                if (temp != null)
                                {
                                    temp.TrangThai = false;
                                    context.SaveChanges();
                                    MessageBox.Show($"Ngừng hoạt động sân bay thành công!");

                                    //Load lại danh sách sân bay
                                    ReLoadList_Airport();
                                }
                            }
                            else if (IsAirportUsed=="Tái hoạt động")
                            {
                                //Kiểm tra chắc chắn muốn ngừng hoạt động
                                messageBoxResult = MessageBox.Show("Bạn chắn chắn muốn tái hoạt động sân bay?", "Cảnh báo", MessageBoxButton.YesNo);
                                if (messageBoxResult == MessageBoxResult.No)
                                    return;
                                var temp = context.SANBAYs.Where(h => h.MaSanBay == Airport_selecteditem.Id).FirstOrDefault();
                                if (temp != null)
                                {
                                    temp.TrangThai = true;
                                    context.SaveChanges();
                                    MessageBox.Show($"Tái hoạt động sân bay thành công!");

                                    //Load lại danh sách sân bay
                                    ReLoadList_Airport();
                                }
                            }
                            else
                            {
                                //Kiểm tra chắc chắn muốn xóa
                                messageBoxResult = MessageBox.Show("Bạn chắn chắn muốn xóa sân bay?", "Cảnh báo", MessageBoxButton.YesNo);
                                if (messageBoxResult == MessageBoxResult.No)
                                    return;

                                var temp = context.SANBAYs.Where(h => h.MaSanBay == Airport_selecteditem.Id).FirstOrDefault();
                                if (temp != null)
                                {
                                    context.SANBAYs.Remove(temp);
                                    context.SaveChanges();
                                    MessageBox.Show($"Xóa sân bay thành công!");

                                    //Load lại danh sách sân bay
                                    ReLoadList_Airport();
                                }
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
                  MoreTicketClass_Name = string.Empty;
                  MoreTicketClass_Coefficien = string.Empty;
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
                           if (string.IsNullOrEmpty(MoreTicketClass_Name) || string.IsNullOrEmpty(MoreTicketClass_Coefficien))
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
                           HANGVE temp = new HANGVE() { TenHangVe = MoreTicketClass_Name, TrangThai = true, HeSo = (decimal)(double.Parse(MoreTicketClass_Coefficien)) };
                           context.HANGVEs.Add(temp);
                           context.SaveChanges();

                           //Close dialog
                           DialogHost.CloseDialogCommand.Execute(null, null);

                           MessageBox.Show("Thêm hạng vé thành công!", "Cảnh báo");

                       }
                       catch (EntityException e)
                       {
                           MessageBox.Show("Database access failed!", string.Format($"Exception: {e.Message}"), MessageBoxButton.OK, MessageBoxImage.Error);
                       }
                   }
               });
            MoreTicketClass_Cancel_Button_Command = new RelayCommand<object>(
              (p) => { return true; },
              (p) =>
              {
                  //Close dialog
                  DialogHost.CloseDialogCommand.Execute(null, null);
              }
          );

            EditTicketClass_Command = new RelayCommand<object>(
             (p) => { return true; },
             async (p) =>
             {
                 if (TicketClass_selecteditem != null)
                 {
                     EditTicketClass_Name = string.Empty;
                     EditTicketClass_Coefficien = string.Empty;
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
                           bool check = false;

                           if (!string.IsNullOrEmpty(EditTicketClass_Name))
                           {
                               foreach (var item in context.HANGVEs.ToList())
                               {
                                   if (item.MaHangVe != TicketClass_selecteditem.Id && item.TenHangVe == EditTicketClass_Name)
                                   {
                                       MessageBox.Show("Tên hạng vé đã tồn tại!", "Cảnh báo");
                                       EditTicketClass_Name = null;
                                       return;
                                   }
                               }
                               context.HANGVEs.ToList().Where(s => s.MaHangVe == TicketClass_selecteditem.Id).FirstOrDefault().TenHangVe = EditTicketClass_Name;
                               check = true;
                           }
                           if (!string.IsNullOrEmpty(EditTicketClass_Coefficien))
                           {
                               if (double.Parse(EditTicketClass_Coefficien) == 0)
                               {
                                   MessageBox.Show("Hệ số không thể là 0.", "Cảnh báo");
                                   EditTicketClass_Coefficien = string.Empty;
                                   return;
                               }
                               context.HANGVEs.ToList().Where(s => s.MaHangVe == TicketClass_selecteditem.Id).FirstOrDefault().HeSo = (decimal)(double.Parse(EditTicketClass_Coefficien));
                               check = true;
                           }
                           if (check)
                           {
                               context.SaveChanges();
                               // Close dialog
                               DialogHost.CloseDialogCommand.Execute(null, null);
                               MessageBox.Show("Lưu thay đổi hạng vé thành công!", "Cảnh báo");
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
            EditTicketClass_Cancel_Button_Command = new RelayCommand<object>(
              (p) => { return true; },
              (p) =>
              {
                  //Close dialog
                  DialogHost.CloseDialogCommand.Execute(null, null);
              }
          );

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
                             MessageBoxResult messageBoxResult;
                             if (IsTicketUsed == "Ngừng cung cấp")
                             {
                                 //Kiểm tra có chuyến bay nào sử dụng vé mà chưa cất cánh
                                 var list = context.CHITIETHANGVEs.Where(h => h.MaHangVe == TicketClass_selecteditem.Id).ToList();
                                 foreach (var item in list)
                                 {
                                     var exist = context.CHUYENBAYs.Where(h => h.MaChuyenBay == item.MaChuyenBay).FirstOrDefault();
                                     if (!exist.DaKhoiHanh)
                                     {
                                         var code1 = exist.DUONGBAY.SANBAY1.VietTat;
                                         var code2 = exist.DUONGBAY.SANBAY.VietTat;
                                         MessageBox.Show("Bạn không thể ngừng cung cấp hạng vé.\nHạng vé đang được cung cấp trên chuyến bay chưa khởi hành: " + code1 + code2 + "-" + exist.MaChuyenBay, "Cảnh báo");
                                         return;
                                     }
                                 }

                                 //Kiểm tra chắc chắn muốn ngừng cung cấp
                                 messageBoxResult = MessageBox.Show("Bạn chắn chắn muốn ngừng cung cấp hạng vé?", "Cảnh báo", MessageBoxButton.YesNo);
                                 if (messageBoxResult == MessageBoxResult.No)
                                     return;
                                 var disable = context.HANGVEs.Where(h => h.MaHangVe == TicketClass_selecteditem.Id).FirstOrDefault();
                                 if (disable != null)
                                 {
                                     disable.TrangThai = false;
                                     context.SaveChanges();
                                     MessageBox.Show($"Ngừng cung cấp hạng vé thành công!");

                                     //Load lại danh sách sân bay
                                     ReLoadList_TicketClass();
                                 }
                             }
                             else if (IsTicketUsed == "Tái cung cấp")
                             {
                                 //Kiểm tra chắc chắn muốn tái cung cấp
                                 messageBoxResult = MessageBox.Show("Bạn chắn chắn muốn tái cung cấp hạng vé?", "Cảnh báo", MessageBoxButton.YesNo);
                                 if (messageBoxResult == MessageBoxResult.No)
                                     return;
                                 var disable = context.HANGVEs.Where(h => h.MaHangVe == TicketClass_selecteditem.Id).FirstOrDefault();
                                 if (disable != null)
                                 {
                                     disable.TrangThai = true;
                                     context.SaveChanges();
                                     MessageBox.Show($"Tái cung cấp hạng vé thành công!");

                                     //Load lại danh sách sân bay
                                     ReLoadList_TicketClass();
                                 }
                             }
                             else
                             {
                                 //Kiểm tra chắc chắn muốn xóa
                                 messageBoxResult = MessageBox.Show("Bạn chắn chắn muốn xóa hạng vé?", "Cảnh báo", MessageBoxButton.YesNo);
                                 if (messageBoxResult == MessageBoxResult.No)
                                     return;
                                 var temp = context.HANGVEs.Where(h => h.MaHangVe == TicketClass_selecteditem.Id).FirstOrDefault();
                                 if (temp != null)
                                 {
                                     context.HANGVEs.Remove(temp);
                                     context.SaveChanges();
                                     MessageBox.Show($"Xóa hạng vé thành công!");

                                     //Load lại danh sách sân bay
                                     ReLoadList_TicketClass();
                                 }
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
