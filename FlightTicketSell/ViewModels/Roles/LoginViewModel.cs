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
                        await LoadUser();
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

        /// <summary>
        /// Load the user that logged into the application
        /// </summary>
        private async Task LoadUser()
        {
            using (var context = new FlightTicketSellEntities())
            {
                try
                {
                    var user = await context.NGUOIDUNGs
                                .Where(ng => ng.TenDangNhap == Username)
                                .FirstOrDefaultAsync();

                    IoC.IoC.Get<ApplicationViewModel>().CurrentUser = new User()
                    {
                        Username = user.TenDangNhap,
                        Password = user.MatKhau,
                        UserGroupID = user.MaNhom
                    };

                    IoC.IoC.Get<ApplicationViewModel>().CurrentUserGroup = await LoadUserGroup(user.MaNhom);
                }
                catch (EntityException e)
                {;
                    NotifyHelper.ShowEntityException(e);
                };
            }
        }

        /// <summary>
        /// Loads user group with a input user group code 
        /// </summary>
        /// <returns>A user group </returns>
        private async Task<UserGroupModified> LoadUserGroup(string userGroupCode)
        {
            using (var context = new FlightTicketSellEntities())
            {
                var userGroup = await context.NHOMNGUOIDUNGs
                    .Where(nnd => nnd.MaNhom == userGroupCode)
                    .Select(nnd => new UserGroupModifiedWithUsers()
                    {
                        Code = nnd.MaNhom,
                        Name = nnd.TenNhom,
                        CanSearchFlight = nnd.CHUCNANGs.Where(cn => cn.MaChucNang=="TRC").Count() > 0,
                        CanEditFlight = nnd.CHUCNANGs.Where(cn => cn.MaChucNang=="QLCB").Count() > 0,
                        CanScheduleFlight = nnd.CHUCNANGs.Where(cn => cn.MaChucNang=="NLCB").Count() > 0,
                        CanViewReport = nnd.CHUCNANGs.Where(cn => cn.MaChucNang=="BCDT").Count() > 0,
                        CanSettings = nnd.CHUCNANGs.Where(cn => cn.MaChucNang=="CD").Count() > 0,
                        CanManageUser = nnd.CHUCNANGs.Where(cn => cn.MaChucNang=="PHQ").Count() > 0,
                        UserCount = nnd.NGUOIDUNGs.Count(),
                    })
                    .FirstOrDefaultAsync();

                return userGroup;
            }
        }

        #endregion
    }
}
