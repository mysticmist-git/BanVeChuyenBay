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
        #region Public Properties

        /// <summary>
        /// The user group list
        /// </summary>
        public ObservableCollection<UserGroupModifiedWithUsers> UserGroupList { get; set; }

        /// <summary>
        /// The current user group is being selected
        /// </summary>
        public UserGroupModified CurrentUserGroup { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// The load command
        /// </summary>
        public ICommand LoadCommand { get; set; }

        /// <summary>
        /// The to save permission to this user group
        /// </summary>
        public ICommand PermissionSaveCommand { get; set; }

        /// <summary>
        /// The to reset permission of this user group
        /// </summary>
        public ICommand PermissionResetCommand { get; set; }

        #endregion

        #region Constructor

        public RolesViewModel()
        {
            // Create commands
            LoadCommand = new RelayCommand<object>(p => true, async p =>
            {
                await LoadData();
            });

            PermissionResetCommand = new RelayCommand<object>(p => true, async p =>
            {
                await LoadData();
            });

            PermissionSaveCommand = new RelayCommand<object>(p => true, async p =>
            {
                await SavePermission();
            });
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
                        CanSearchFlight = nng.CHUCNANGs.Where(cn => cn.MaChucNang=="TRC").Count() > 0,
                        CanEditFlight = nng.CHUCNANGs.Where(cn => cn.MaChucNang=="QLCB").Count() > 0,
                        CanScheduleFlight = nng.CHUCNANGs.Where(cn => cn.MaChucNang=="NLCB").Count() > 0,
                        CanViewReport = nng.CHUCNANGs.Where(cn => cn.MaChucNang=="BCDT").Count() > 0,
                        CanSettings = nng.CHUCNANGs.Where(cn => cn.MaChucNang=="CD").Count() > 0,
                        CanManageUser = nng.CHUCNANGs.Where(cn => cn.MaChucNang=="PHQ").Count() > 0,
                        UserCount = nng.NGUOIDUNGs.Count(),
                        IsPermissionChanged = false
                    })
                    .ToListAsync();

                UserGroupList = new ObservableCollection<UserGroupModifiedWithUsers>(userGroups);
            }
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

                CurrentUserGroup.IsPermissionChanged = false;
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

        #region Helper

        #region Permission


        #endregion

        #endregion
    }
}
