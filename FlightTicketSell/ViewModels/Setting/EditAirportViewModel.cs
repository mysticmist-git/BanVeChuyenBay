using FlightTicketSell.Models;
using FlightTicketSell.Views;
using System.Windows.Input;
using System.Linq;
using System.Collections.ObjectModel;
using FlightTicketSell.ViewModels.Setting;
using System.Data.Entity.Core;
using System.Windows;

namespace FlightTicketSell.ViewModels
{
    public class EditAirportViewModel : BaseViewModel
    {
        #region Commands
        public ICommand Save_Button_Command { get; set; }
        #endregion

        #region Public Properties
        /// <summary>
        /// Tên sân bay
        /// </summary>
        public string Name { get; set; }

        #endregion

        public EditAirportViewModel()
        {
            
            Save_Button_Command = new RelayCommand<object>(
                 (p) => { return true; },
                 (p) =>
                 {
                     //using (var context = new FlightTicketSellEntities())
                     //{
                     //    try
                     //    {
                     //        if (Name != null)
                     //        {
                     //            SANBAY temp = new SANBAY() { TenSanBay = Name, TrangThai = true, TinhThanh = Province, VietTat = Code };
                     //            context.SANBAYs.Add(temp);
                     //            context.SaveChanges();
                     //            MessageBox.Show("Thay đổi tên sân bay thành công!", "Cảnh báo");
                     //        }
                     //        else
                     //        {
                     //            MessageBox.Show("Vui lòng nhập tên mới!", "Cảnh báo");
                     //        }
                     //    }
                     //    catch (EntityException e)
                     //    {
                     //        MessageBox.Show("Database access failed!", string.Format($"Exception: {e.Message}"), MessageBoxButton.OK, MessageBoxImage.Error);
                     //    }
                     //}

                 }
             );
        }
    }
}
