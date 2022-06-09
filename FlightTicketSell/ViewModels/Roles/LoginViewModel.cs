using FlightTicketSell.Helpers;
using FlightTicketSell.Models;
using System.Data.Entity.Core;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Linq;
using System.Data.Entity;
using FlightTicketSell.Models.Enums;
using System.Windows;
using FlightTicketSell.Interface;

namespace FlightTicketSell.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        #region Interface
        #endregion
        
        #region Public Properties

        /// <summary>
        /// The account username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The account password
        /// </summary>
        public SecureString Password { get; set; }

        #endregion

        #region Commands

        public ICommand LoginCommand { get; set; }

        #endregion

        #region Constructor

        public LoginViewModel()
        {
            // Create commands
            LoginCommand = new RelayCommand<ILogin>(p => true, async p =>
            {
                p.LockLogin();

                var loginResult = await Login();

                switch (loginResult)
                {
                    case LoginResult.Success:
                        p.ShowMainWindow();
                        return;
                    case LoginResult.Fail:
                        MessageBox.Show("Tên đăng nhập hoặc mật khẩu sai", "Đăng nhập thất bại!", MessageBoxButton.OK);
                        p.UnlockLogin();
                        return;
                    case LoginResult.Error:
                        MessageBox.Show("Đã có lỗi xảy ra, vui lòng báo cáo với người quản trị để được giúp đỡ!", "Lỗi!", MessageBoxButton.OK, MessageBoxImage.Error);
                        p.UnlockLogin();
                        return;
                }
            });
        }


        #endregion

        #region Methods

        private async Task<LoginResult> Login()
        {
            using (var context = new FlightTicketSellEntities())
            {
                try
                {
                    var password = SecureHelper.SecureStringToString(Password);
                    var loginResult = await context.NGUOIDUNGs.Where(nd => nd.TenDangNhap == Username && nd.MatKhau == password).CountAsync();

                    if (loginResult > 0)
                        return LoginResult.Success;

                    return LoginResult.Fail;
                }
                catch (EntityException e)
                {;
                    NotifyHelper.ShowEntityException(e);
                    return LoginResult.Error;
                };
            }

        }

        #endregion
    }
}
