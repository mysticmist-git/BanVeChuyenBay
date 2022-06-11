using FlightTicketSell.Interface;
using FlightTicketSell.Models;
using FlightTicketSell.Models.Enums;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FlightTicketSell.Views
{
    /// <summary>
    /// Interaction logic for EditUserGroupDialog.xaml
    /// </summary>
    public partial class EditUserGroupDialog : UserControl, IEditUserGroupDialog
    {
        public EditUserGroupDialog()
        {
            InitializeComponent();
        }

        #region IEditUserGroupDialog

        public MessageBoxResult CancelConfirm()
        {
            var result = MessageBox.Show(
                "Bạn có muốn lưu nhóm người dùng?",
                "Chưa lưu trước khi thoát",
                MessageBoxButton.YesNoCancel,
                MessageBoxImage.Question);

            return result;
        }

        public void Close()
        {
            DialogHost.GetDialogSession("RootDialog").Close();
        }

        public void Notify(ActionResult result, object args = null)
        {
            {
                switch (result)
                {
                    case ActionResult.Succcesful:
                        MessageBox.Show("Chỉnh sửa nhóm người dùng thành công!", "Thành công", MessageBoxButton.OK);
                        break;
                    case ActionResult.Fail:
                        MessageBox.Show("Chỉnh sửa nhóm người dùng thất bại!", "Thất bại", MessageBoxButton.OK);
                        break;
                    case ActionResult.Duplicate:
                        var message =
                            "Nhóm người dùng đã tồn tại!" +
                            (args != null ? ("\nMã nhóm: " + (args as UserGroup).Code) : "") +
                            (args != null ? ("\nTên nhóm: " + (args as UserGroup).Name) : "");

                        MessageBox.Show(
                            message,
                            "Thất bại",
                            MessageBoxButton.OK
                            );
                        break;
                    case ActionResult.Error:
                        MessageBox.Show("Đã có lỗi xảy ra!", "Lỗi", MessageBoxButton.OK);
                        break;
                    case ActionResult.NotEnoughInformationForAction:
                        MessageBox.Show("Vui lòng đầy đủ thông tin!", "Thiếu thông tin", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                }
            }
        }


        #endregion
    }
}
