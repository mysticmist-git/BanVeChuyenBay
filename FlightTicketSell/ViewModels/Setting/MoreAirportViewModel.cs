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
    public class MoreAirportViewModel : BaseViewModel
    {
        #region Commands
        public ICommand Save_Button_Command { get; set; }
        #endregion

        #region Public Properties
        /// <summary>
        /// Tên sân bay
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Viết tắt
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Tỉnh thành
        /// </summary>
        public string Province { get; set; }
        #endregion

        public MoreAirportViewModel()
        {
            Save_Button_Command = new RelayCommand<object>(
                (p) => { return true; },
                (p) =>
                {
                    using (var context = new FlightTicketSellEntities())
                    {
                        try
                        {
                            if (Name != null && Code != null && Province != null)
                            {
                                if(context.SANBAYs.Where(h => h.VietTat == Code).FirstOrDefault() != null)
                                {
                                    MessageBox.Show("Mã sân bay đã tồn tại!", "Cảnh báo");
                                    Code = "";
                                }
                                else if (context.SANBAYs.Where(h => h.TenSanBay == Name).FirstOrDefault() != null)
                                {
                                    MessageBox.Show("Tên sân bay đã tồn tại!", "Cảnh báo");
                                    Name = "";
                                }
                                else
                                {
                                    SANBAY temp = new SANBAY() { TenSanBay = Name, TrangThai = true, TinhThanh = Province, VietTat = Code };
                                    context.SANBAYs.Add(temp);
                                    context.SaveChanges();
                                    MessageBox.Show("Thêm sân bay thành công!", "Cảnh báo");
                                }    
                               
                            }
                            else
                            {
                                MessageBox.Show("Vui lòng nhập đầy đủ thông tin sân bay!", "Cảnh báo");
                            }
                            

                        }
                        catch (EntityException e)
                        {
                            MessageBox.Show("Database access failed!", string.Format($"Exception: {e.Message}"), MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }

                }
            );
        }
    }
}
