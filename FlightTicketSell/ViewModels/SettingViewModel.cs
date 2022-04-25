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
        #region Commands
        public ICommand Open_Window_MoreAirport_Command { get; set; }
        public ICommand Open_Window_MoreTicketClass_Command { get; set; }
        public ICommand Save_FlightRegulations_Command { get; set; }
        public ICommand Airport_Delete_Button_Command { get; set; }
        public ICommand Airport_Edit_Button_Command { get; set; }
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
        public Airport Airport_selecteditem { get; set; }
        /// <summary>
        /// Danh sách hạng vé
        /// </summary>
        public ObservableCollection<TicketClass> List_TicketClass { get; set; } = new ObservableCollection<TicketClass>();

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
                                    List_Airport.Add(new Airport() { Code = item.VietTat, Name = item.TenSanBay, Province = item.TinhThanh });
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
                                    List_TicketClass.Add(new TicketClass() { Name = item.TenHangVe, Coefficient = (double)item.HeSo });
                            }

                        }
                        catch (EntityException e)
                        {
                            MessageBox.Show("Database access failed!", string.Format($"Exception: {e.Message}"), MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }

                }
            );

            Open_Window_MoreAirport_Command = new RelayCommand<object>((p) => true, async (p) =>
               
                {
                    MoreAirportView moreAirportView = new MoreAirportView();
                    var result = await DialogHost.Show(moreAirportView, "RootDialog", ClosingEventHandler);

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
                                    List_Airport.Add(new Airport() { Code = item.VietTat, Name = item.TenSanBay, Province = item.TinhThanh });
                            }
                        }
                        catch (EntityException e)
                        {
                            MessageBox.Show("Database access failed!", string.Format($"Exception: {e.Message}"), MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
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
                       try
                       {
                           context.THAMSOes.Where(h => h.TenThamSo == "SoSanBayTrungGianToiDa").FirstOrDefault().GiaTri = Max_LayoverAirport;
                           context.SaveChanges();
                       }
                       catch (EntityException e)
                       {
                           MessageBox.Show("Database access failed!", string.Format($"Exception: {e.Message}"), MessageBoxButton.OK, MessageBoxImage.Error);
                       }
                   }
               }
           );

            Airport_Delete_Button_Command = new RelayCommand<object>(

              (p) => { return true; },

              (p) =>
              {
                  using (var context = new FlightTicketSellEntities())
                  {
                      try
                      {
                          if (Airport_selecteditem !=null)
                          {
                              SANBAY temp = context.SANBAYs.Where(h => h.VietTat == Airport_selecteditem.Code).FirstOrDefault();
                              if (temp != null)
                              {
                                  context.SANBAYs.Where(h => h.VietTat == Airport_selecteditem.Code).FirstOrDefault().TrangThai = false;
                                  context.SaveChanges();
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

            Airport_Edit_Button_Command = new RelayCommand<object>(

             (p) => { return true; },

             (p) =>
             {
                 if (Airport_selecteditem !=null)
                 {
                     EditAirportView editAirportView = new EditAirportView(Airport_selecteditem);
                     editAirportView.ShowDialog();
                 }
                 else
                 {
                     MessageBox.Show("Vui lòng chọn sân bay muốn chỉnh sửa!", "Cảnh báo");
                 }
             }
         );
        }

        /// <summary>
        /// What to do when dialog closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void ClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            //if (
            //    Convert.ToInt32(eventArgs.Parameter) == 1 ||
            //    (
            //        NameBuffer == HoTen &&
            //        IDBuffer == CMND &&
            //        PhoneNumBuffer == SDT &&
            //        EmailBuffer == Email
            //    )
            //)
            //    return;

            //var result = MessageBox.Show("Bạn có muốn lưu thay đổi", "Lưu thay đổi", MessageBoxButton.YesNoCancel);
            //switch (result)
            //{
            //    case MessageBoxResult.Yes:
            //        SaveCustomerCommand.Execute(null);
            //        break;
            //    case MessageBoxResult.No:
            //        break;
            //    case MessageBoxResult.Cancel:
            //        eventArgs.Cancel();
            //        break;

            //}
        }
    }
}
