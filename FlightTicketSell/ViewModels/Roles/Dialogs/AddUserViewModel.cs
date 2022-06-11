using FlightTicketSell.Helpers;
using FlightTicketSell.Models;
using FlightTicketSell.Models.Enums;
using MaterialDesignThemes.Wpf;
using System;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FlightTicketSell.ViewModels
{
    public class AddUserViewModel : BaseViewModel
    {
        #region Parent View Model

        /// <summary>
        /// The parent view model of this view model
        /// </summary>
        public RolesViewModel ParentViewModel { get; set; }

        #endregion
        
        public string UserName { get; set; }

        /// <summary>
        /// The account password
        /// </summary>
        public SecureString Password { get; set; }

        public bool IsSaveAble { get; set; } = true;

        public bool IsCancelable { get; set; } = true;

        #region Commands

        /// <summary>
        /// Save the user group down the database
        /// </summary>
        public ICommand SaveCommand { get; set; }

        /// <summary>
        /// Cancel add user group and close the dialog
        /// </summar
        public ICommand CancelCommand { get; set; }

        #endregion

        #region Constructor

        public AddUserViewModel()
        {
            // Create commands
            CancelCommand = new RelayCommand<object>(p => true, p =>
            {
                IsCancelable = false;
                DialogHost.GetDialogSession("RootDialog").Close();
                IsCancelable = true;
            });

            SaveCommand = new RelayCommand<object>(p => true, async p =>
            {
                IsSaveAble = false;

                var result = await SaveUser();

                switch (result)
                {
                    case ActionResult.Duplicate:
                        MessageBox.Show("Tên đăng nhập đã tồn tại", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    case ActionResult.Succcesful:
                        MessageBox.Show("Thêm tài khoản thành công");
                        DialogHost.GetDialogSession("RootDialog").Close();
                        break;
                    case ActionResult.Error:
                        MessageBox.Show("Đã có lỗi xảy ra", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    case ActionResult.NotEnoughInformationForAction:
                        MessageBox.Show("Xin nhập đủ thông tin", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                }

                IsSaveAble = true;
            });
        }

        /// <summary>
        /// Save user group down database
        /// </summary>
        /// <returns></returns>
        private async Task<ActionResult> SaveUser()
        {
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(SecureHelper.SecureStringToString(Password)))
            {
                return ActionResult.NotEnoughInformationForAction;
            }

            var user = this.CreateUser();
            var result = await DatabaseHelper.SaveUser(user);
            ParentViewModel.CurrentUserGroup.Users.Add(user);

            return result;
        }


        /// <summary>
        /// Create user group with <see cref="UserGroupCode"/> and <see cref="UserGroupName"/>
        /// </summary>
        /// <returns></returns>
        private User CreateUser()
        {
            if (UserName is null || Password is null)
                return null;

            return new User()
            {
                Username =this.UserName,
                Password = SecureHelper.SecureStringToString(Password),
                UserGroupID = ParentViewModel.CurrentUserGroup.Code
            };
        }

        #endregion
    }
}
