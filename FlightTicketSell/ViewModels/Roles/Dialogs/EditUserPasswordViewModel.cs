using FlightTicketSell.Helpers;
using FlightTicketSell.Models;
using MaterialDesignThemes.Wpf;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Linq;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FlightTicketSell.ViewModels
{
    public class EditUserPasswordViewModel : BaseViewModel
    {
        public RolesViewModel ParentViewModel { get; set; }

        public SecureString NewPassword { get; set; }

        #region Commands

        public ICommand CancelCommand { get; set; }

        public ICommand SaveCommand { get; set; }

        public bool IsSaveAble { get; set; } = true;

        public bool IsCancelable { get; set; } = true;

        #endregion

        #region Constructor

        public EditUserPasswordViewModel()
        {
            CancelCommand = new RelayCommand<object>(p => true, p =>
            {
                IsCancelable = false;
                DialogHost.GetDialogSession("RootDialog").Close();
                IsCancelable = true;
            });

            SaveCommand = new RelayCommand<object>(p => true, async p =>
            {
                IsSaveAble = false;

                if (NewPassword is null || string.IsNullOrEmpty(SecureHelper.SecureStringToString(NewPassword)))
                {
                    MessageBox.Show("Vui lòng nhập mật khẩu mới", "Thiếu thông tin", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                using (var context = new FlightTicketSellEntities())
                {
                    try
                    {
                        var user = await context.NGUOIDUNGs.Where(nd => nd.TenDangNhap == ParentViewModel.CurrentUser.Username).FirstOrDefaultAsync();

                        user.MatKhau = SecureHelper.SecureStringToString(NewPassword);
                        await context.SaveChangesAsync();
                        MessageBox.Show("Thay đổi mật khẩu thành công", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                        ParentViewModel.CurrentUser.Password = SecureHelper.SecureStringToString(NewPassword);
                        DialogHost.GetDialogSession("RootDialog").Close();
                    }
                    catch (EntityException ex)
                    {
                        NotifyHelper.ShowEntityException(ex);
                    }
                }

                IsSaveAble = true;
            });
        }

        #endregion
    }
}
