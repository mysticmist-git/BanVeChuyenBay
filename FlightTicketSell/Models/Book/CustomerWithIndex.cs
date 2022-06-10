using FlightTicketSell.Helpers;
using FlightTicketSell.Models.Enums;
using FlightTicketSell.ViewModels;
using System.Data.Entity.Core;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace FlightTicketSell.Models
{
    public class CustomerWithIndex : Customer
    {
        #region Public Properties

        public string HoTen_Display { get; set; }

        public string CMND_Display { get; set; }

        public string SDT_Display { get; set; }

        public string Email_Display { get; set; }

        /// <summary>
        /// The index of this customer in some list
        /// </summary>
        public int Index { get; set; }
        public string DisplayIndex { get => "Khách hàng " + Index; }
        private bool IsChanged = false;
        public static CustomerDetailType CustomerDetailType { get; set; }
        public bool IsRead { get; set; }
        #endregion

        #region Command
        public ICommand EditCustomerInfor_Command { get; set; }
        public ICommand SaveCustomerInfor_Command { get; set; }
        #endregion

        #region Constructor

        /// <summary>
        /// Copy constructor
        /// </summary>
        public CustomerWithIndex(KHACHHANG kh) : base(kh)
        {
            HoTen_Display = HoTen;
            CMND_Display = CMND;
            SDT_Display = SDT;
            Email_Display = Email;
            IsRead = true;
            EditCustomerInfor_Command = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (CustomerDetailType == CustomerDetailType.Reservation)
                    IsRead = false;
                   
            });
            SaveCustomerInfor_Command = new RelayCommand<object>((p) => { return true; }, (p) =>
            {

                IsChanged = false;
                if (HoTen_Display != HoTen || CMND_Display != CMND || SDT_Display != SDT || Email_Display != Email)
                    IsChanged = true;

                if (IsChanged)
                {
                    if (string.IsNullOrEmpty(HoTen_Display))
                    {
                        MessageBox.Show("Họ tên không được để trống!", "Cảnh báo");
                        HoTen_Display = HoTen;
                        return;
                    }

                    if (string.IsNullOrEmpty(CMND_Display))
                    {
                        MessageBox.Show("CMND không được để trống!", "Cảnh báo");
                        CMND_Display =CMND;
                        return;
                    }

                    using (var context = new FlightTicketSellEntities())
                    {
                        try
                        {

                            if (CMND_Display != CMND && context.KHACHHANGs.Where(h => h.MaKhachHang != MaKhachHang && h.CMND == CMND_Display).FirstOrDefault() != null)
                            {
                                MessageBox.Show("Số CMND đã tồn tại.", "Cảnh báo");
                                CMND_Display = CMND;
                                return;
                            }
                            var khachhang = context.KHACHHANGs.Where(h => h.MaKhachHang == MaKhachHang).FirstOrDefault();
                            if (khachhang != null)
                            {
                                khachhang.HoTen = HoTen = HoTen_Display;
                                khachhang.CMND = CMND = CMND_Display;
                                khachhang.SDT = SDT = SDT_Display;
                                khachhang.Email = Email = Email_Display;
                                context.SaveChanges();
                                MessageBox.Show("Thay đổi thông tin thành công!", "Cảnh báo");
                            }
                            else
                            {
                                MessageBox.Show("Thay đổi thông tin không thành công!", "Cảnh báo");
                            }
                        }
                        catch (EntityException e)
                        {
                            NotifyHelper.ShowEntityException(e);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Bạn chưa thực hiện thay đổi thông tin!", "Cảnh báo");
                }
                IsRead = true;
            });
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public CustomerWithIndex()
        {

        }

        #endregion
    }
}