using FlightTicketSell.Models;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;
using FlightTicketSell.Models.Roles;
using System.Data.Entity.Core;
using FlightTicketSell.Helpers;

namespace FlightTicketSell.ViewModels
{
    /// <summary>
    /// The viewmodel for roles view
    /// </summary>
    public class RolesViewModel : BaseViewModel
    {
        #region Private Members

        #region User Group Private Members

        /// <summary>
        /// Indicates if this user group can search for flights
        /// </summary>
        private bool _canSearchFlightBuffer;

        /// <summary>
        /// Indicates if this user group can search for flights
        /// </summary>
        private bool _canEditFlightBuffer;

        /// <summary>
        /// Indicates if this user group can schedule flights
        /// </summary>
        private bool _canScheduleFlightBuffer;

        /// <summary>
        /// Indicates if this user group can view reports
        /// </summary>
        private bool _canViewReportBuffer;

        /// <summary>
        /// Indicates if this user group can change settings
        /// </summary>
        private bool _canSettingsBuffer;

        /// <summary>
        /// Indicates if this user group can manage users
        /// </summary>
        private bool _canManageUserBuffer;

        #endregion

        #endregion

        #region Public Properties

        /// <summary>
        /// The user group list
        /// </summary>
        public ObservableCollection<UserGroupModifiedWithUsers> UserGroupList { get; set; }

        /// <summary>
        /// The current user group is being selected
        /// </summary>
        public UserGroupModifiedWithUsers CurrentUserGroup { get; set; }

        /// <summary>
        /// Indicates if the permisison of the current user group is changed
        /// </summary>
        public bool IsPermissionChanged
        {
            get
            {
                if (CurrentUserGroup is null)
                    return false;

                return
                    CurrentUserGroup.CanSearchFlight != _canSearchFlightBuffer ||
                    CurrentUserGroup.CanEditFlight != _canEditFlightBuffer ||
                    CurrentUserGroup.CanScheduleFlight != _canScheduleFlightBuffer ||
                    CurrentUserGroup.CanViewReport != _canViewReportBuffer ||
                    CurrentUserGroup.CanSettings != _canSettingsBuffer ||
                    CurrentUserGroup.CanManageUser != _canManageUserBuffer;
            }
        }

        #endregion

        #region Commands

        #region Global Commands

        /// <summary>
        /// The load command
        /// </summary>
        public ICommand LoadCommand { get; set; }

        #endregion

        #region User Group Commands

        /// <summary>
        /// Executes when user choose another user group
        /// </summary>
        public ICommand UserGroupChanged { get; set; }

        #endregion

        #region Permission Commands

        /// <summary>
        /// The to save permission to this user group
        /// </summary>
        public ICommand PermissionSaveCommand { get; set; }

        /// <summary>
        /// The to reset permission of this user group
        /// </summary>
        public ICommand PermissionResetCommand { get; set; }

        /// <summary>
        /// Executes when permission the current user group is changed
        /// </summary>
        public ICommand PermissionChangedCommand { get; set; }

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public RolesViewModel()
        {
            #region Create commands

            #region Global Commands

            LoadCommand = new RelayCommand<object>(p => true, async p =>
            {
                await LoadData();
            });

            #endregion

            #region User Group Commands

            UserGroupChanged = new RelayCommand<object>(p => true, p =>
            {
                // Re-buffer permission
                BufferPermission();
            });

            #endregion

            #region Permisison Commands

            PermissionResetCommand = new RelayCommand<object>(p => true, async p =>
            {
                ReloadPermission();
            });

            PermissionSaveCommand = new RelayCommand<object>(p => true, async p =>
            {
                await SavePermission();
                BufferPermission();
            });

            PermissionChangedCommand = new RelayCommand<object>(p => true, async p =>
            {
                OnPropertyChanged(nameof(IsPermissionChanged));
            });

            #endregion

            #endregion
        }

        #endregion

        #region Methods

        /// <summary>
        /// Load the buffer permission to the current permission
        /// </summary>
        private void ReloadPermission()
        {
            if (CurrentUserGroup is null)
                return;

            CurrentUserGroup.CanSearchFlight = _canSearchFlightBuffer;
            CurrentUserGroup.CanEditFlight = _canEditFlightBuffer;
            CurrentUserGroup.CanScheduleFlight = _canScheduleFlightBuffer;
            CurrentUserGroup.CanViewReport = _canViewReportBuffer;
            CurrentUserGroup.CanSettings = _canSettingsBuffer;
            CurrentUserGroup.CanManageUser = _canManageUserBuffer;

            OnPropertyChanged(nameof(IsPermissionChanged)); 
        }

        /// <summary>
        /// Loads data of the view
        /// </summary>
        private async Task LoadData()
        {
            await LoadUserGroup();
        }

        /// <summary>
        /// Loads all User groups
        /// </summary>
        /// <returns></returns>
        private async Task LoadUserGroup()
        {
            using (var context = new FlightTicketSellEntities())
            {
                var userGroups = await context.NHOMNGUOIDUNGs
                    .Select(nng => new UserGroupModifiedWithUsers()
                    {
                        Code = nng.MaNhom,
                        Name = nng.TenNhom,
                        CanSearchFlight = nng.CHUCNANGs.Where(cn => cn.MaChucNang == "TRC").Count() > 0,
                        CanEditFlight = nng.CHUCNANGs.Where(cn => cn.MaChucNang == "QLCB").Count() > 0,
                        CanScheduleFlight = nng.CHUCNANGs.Where(cn => cn.MaChucNang == "NLCB").Count() > 0,
                        CanViewReport = nng.CHUCNANGs.Where(cn => cn.MaChucNang == "BCDT").Count() > 0,
                        CanSettings = nng.CHUCNANGs.Where(cn => cn.MaChucNang == "CD").Count() > 0,
                        CanManageUser = nng.CHUCNANGs.Where(cn => cn.MaChucNang == "PHQ").Count() > 0,
                        UserCount = nng.NGUOIDUNGs.Count(),
                    })
                    .ToListAsync();

                UserGroupList = new ObservableCollection<UserGroupModifiedWithUsers>(userGroups);

                if (UserGroupList.Count > 0)
                {
                    CurrentUserGroup = UserGroupList[0];
                    BufferPermission();
                }
            }
        }

        /// <summary>
        /// Buffer the current user group permission, so we can check if changed later
        /// </summary>
        private void BufferPermission()
        {
            _canSearchFlightBuffer = CurrentUserGroup.CanSearchFlight;
            _canEditFlightBuffer = CurrentUserGroup.CanEditFlight;
            _canScheduleFlightBuffer = CurrentUserGroup.CanScheduleFlight;
            _canViewReportBuffer = CurrentUserGroup.CanViewReport;
            _canSettingsBuffer = CurrentUserGroup.CanSettings;
            _canManageUserBuffer = CurrentUserGroup.CanManageUser;

            OnPropertyChanged(nameof(IsPermissionChanged));
        }

        /// <summary>
        /// Savef permissions of the current user group
        /// </summary>
        /// <returns></returns>
        private async Task SavePermission()
        {
            using (var context = new FlightTicketSellEntities())
            {
                try
                {
                    if (CurrentUserGroup.CanSearchFlight)
                        await GrantPermission(CurrentUserGroup.Code, "TRC");
                    else
                        await RemovePermission(CurrentUserGroup.Code, "TRC");

                    if (CurrentUserGroup.CanEditFlight)
                        await GrantPermission(CurrentUserGroup.Code, "QLCB");
                    else
                        await RemovePermission(CurrentUserGroup.Code, "QLCB");

                    if (CurrentUserGroup.CanScheduleFlight)
                        await GrantPermission(CurrentUserGroup.Code, "NLCB");
                    else
                        await RemovePermission(CurrentUserGroup.Code, "NLCB");

                    if (CurrentUserGroup.CanViewReport)
                        await GrantPermission(CurrentUserGroup.Code, "BCDT");
                    else
                        await RemovePermission(CurrentUserGroup.Code, "BCDT");

                    if (CurrentUserGroup.CanSettings)
                        await GrantPermission(CurrentUserGroup.Code, "CD");
                    else
                        await RemovePermission(CurrentUserGroup.Code, "CD");

                    if (CurrentUserGroup.CanManageUser)
                        await GrantPermission(CurrentUserGroup.Code, "PHQ");
                    else
                        await RemovePermission(CurrentUserGroup.Code, "PHQ");
                }
                catch (EntityException ex)
                {
                    NotifyHelper.ShowEntityException(ex);
                }
            }
        }

        /// <summary>
        /// Grant a permission to the user group
        /// </summary>
        /// <param name="userGroupCode">The user group code being granted</param>
        /// <param name="permissionCode">The code of the permission to be granted </param>
        private async Task GrantPermission(string userGroupCode, string permissionCode)
        {
            using (var context = new FlightTicketSellEntities())
            {
                try
                {
                    var userGroup = await context.NHOMNGUOIDUNGs
                                        .Where(nnd => nnd.MaNhom == userGroupCode)
                                        .FirstOrDefaultAsync();

                    // Check if permisison already granted
                    if (userGroup.CHUCNANGs.Where(cn => cn.MaChucNang == permissionCode).Count() > 0)
                        return;

                    // Grant permission
                    var permission = await context.CHUCNANGs.Where(cn => cn.MaChucNang == permissionCode).FirstOrDefaultAsync();
                    userGroup.CHUCNANGs.Add(permission);
                    await context.SaveChangesAsync();
                }
                catch (EntityException ex)
                {
                    NotifyHelper.ShowEntityException(ex);
                }
            }
        }

        /// <summary>
        /// Removes a permission to the user group
        /// </summary>
        /// <param name="userGroupCode">The user group code has permission being removed</param>
        /// <param name="permissionCode">The code of the permission to be removed </param>
        private async Task RemovePermission(string userGroupCode, string permissionCode)
        {
            using (var context = new FlightTicketSellEntities())
            {
                try
                {
                    var userGroup = await context.NHOMNGUOIDUNGs
                                        .Where(nnd => nnd.MaNhom == userGroupCode)
                                        .FirstOrDefaultAsync();

                    // Check if permisison is indeed granted before
                    if (userGroup.CHUCNANGs.Where(cn => cn.MaChucNang == permissionCode).Count() <= 0)
                        return;

                    // Remove permission
                    var permission = await context.CHUCNANGs.Where(cn => cn.MaChucNang == permissionCode).FirstOrDefaultAsync();
                    userGroup.CHUCNANGs.Remove(permission);
                    await context.SaveChangesAsync();
                }
                catch (EntityException ex)
                {
                    NotifyHelper.ShowEntityException(ex);
                }
            }
        }

        #endregion
    }
}
