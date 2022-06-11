using FlightTicketSell.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Core;
using FlightTicketSell.Models.Enums;

namespace FlightTicketSell.Helpers
{

    #region Enums

    /// <summary>
    /// Indicates which user group is it
    /// </summary>
    public enum UserGroupType
    {
        Normal,
        Modified,
        ModifiedWithUsers
    }

    #endregion

    /// <summary>
    /// A helper for retriving data
    /// </summary>
    public class DatabaseHelper
    {

        #region Roles

        /// <summary>
        /// Save user group down database
        /// </summary>
        /// <returns>Result of the ation</returns>
        public async static Task<ActionResult> SaveUserGroup(UserGroup userGroup)
        {
            using (var context = new FlightTicketSellEntities())
            {
                try
                {
                    if (await context.NHOMNGUOIDUNGs.Where(nnd => nnd.MaNhom == userGroup.Code).CountAsync() > 0)
                        return ActionResult.Duplicate;

                    context.NHOMNGUOIDUNGs.Add(new NHOMNGUOIDUNG()
                    {
                        MaNhom = userGroup.Code,
                        TenNhom = userGroup.Name
                    });

                    await context.SaveChangesAsync();

                    return ActionResult.Succcesful;
                }
                catch (EntityException ex)
                {
                    NotifyHelper.ShowEntityException(ex);
                    return ActionResult.Error;
                }
            }
        }

        /// <summary>
        /// Remove user group from database
        /// </summary>
        /// <returns>
        /// Result of the ation, Fail means not exists, Error means something wrong
        /// Succesful is what it is
        /// </returns>
        public async static Task<ActionResult> RemoveUserGroup(UserGroup userGroup)
        {
            using (var context = new FlightTicketSellEntities())
            {
                try
                {
                    if (await context.NHOMNGUOIDUNGs.Where(nnd => nnd.MaNhom == userGroup.Code).CountAsync() <= 0)
                        return ActionResult.Fail;

                    // Removes all users
                    context.NGUOIDUNGs.RemoveRange(await context.NGUOIDUNGs
                        .Where(nd => nd.MaNhom == userGroup.Code)
                        .ToListAsync());

                    // Removes all permission
                    var userGroupToRemove = await context.NHOMNGUOIDUNGs
                        .Where(nndung => nndung.MaNhom == userGroup.Code)
                        .FirstOrDefaultAsync();
                    userGroupToRemove.CHUCNANGs.Clear();

                    // Remove user group
                    context.NHOMNGUOIDUNGs.Remove(userGroupToRemove);

                    await context.SaveChangesAsync();

                    return ActionResult.Succcesful;
                }
                catch (EntityException ex)
                {
                    NotifyHelper.ShowEntityException(ex);
                    return ActionResult.Error;
                }
            }
        }

        /// <summary>
        /// Loads user group with a input user group code 
        /// </summary>
        /// <returns>A user group </returns>
        public static async Task<UserGroup> LoadUserGroup(string userGroupCode, UserGroupType userGroupType)
        {
            using (var context = new FlightTicketSellEntities())
            {
                try
                {
                    switch (userGroupType)
                    {

                        case UserGroupType.Normal:
                            return await context.NHOMNGUOIDUNGs
                                .Where(nnd => nnd.MaNhom == userGroupCode)
                                .Select(nnd => new UserGroup()
                                {
                                    Code = nnd.MaNhom,
                                    Name = nnd.TenNhom
                                })
                                .FirstOrDefaultAsync();

                        case UserGroupType.Modified:
                            return await context.NHOMNGUOIDUNGs
                                .Where(nnd => nnd.MaNhom == userGroupCode)
                                .Select(nnd => new UserGroupModified()
                                {
                                    Code = nnd.MaNhom,
                                    Name = nnd.TenNhom,
                                    CanSearchFlight = nnd.CHUCNANGs.Where(cn => cn.MaChucNang == "TRC").Count() > 0,
                                    CanEditFlight = nnd.CHUCNANGs.Where(cn => cn.MaChucNang == "QLCB").Count() > 0,
                                    CanScheduleFlight = nnd.CHUCNANGs.Where(cn => cn.MaChucNang == "NLCB").Count() > 0,
                                    CanViewReport = nnd.CHUCNANGs.Where(cn => cn.MaChucNang == "BCDT").Count() > 0,
                                    CanSettings = nnd.CHUCNANGs.Where(cn => cn.MaChucNang == "CD").Count() > 0,
                                    CanManageUser = nnd.CHUCNANGs.Where(cn => cn.MaChucNang == "PHQ").Count() > 0
                                })
                                .FirstOrDefaultAsync();

                        case UserGroupType.ModifiedWithUsers:
                            return await context.NHOMNGUOIDUNGs
                                .Where(nnd => nnd.MaNhom == userGroupCode)
                                .Select(nnd => new UserGroupModifiedWithUsers()
                                {
                                    Code = nnd.MaNhom,
                                    Name = nnd.TenNhom,
                                    CanSearchFlight = nnd.CHUCNANGs.Where(cn => cn.MaChucNang == "TRC").Count() > 0,
                                    CanEditFlight = nnd.CHUCNANGs.Where(cn => cn.MaChucNang == "QLCB").Count() > 0,
                                    CanScheduleFlight = nnd.CHUCNANGs.Where(cn => cn.MaChucNang == "NLCB").Count() > 0,
                                    CanViewReport = nnd.CHUCNANGs.Where(cn => cn.MaChucNang == "BCDT").Count() > 0,
                                    CanSettings = nnd.CHUCNANGs.Where(cn => cn.MaChucNang == "CD").Count() > 0,
                                    CanManageUser = nnd.CHUCNANGs.Where(cn => cn.MaChucNang == "PHQ").Count() > 0,
                                    UserCount = nnd.NGUOIDUNGs.Count(),
                                })
                                .FirstOrDefaultAsync();
                        default:
                            return null;
                    }
                }
                catch (EntityException ex)
                {
                    NotifyHelper.ShowEntityException(ex);
                    return null;
                };
            }
        }

        #endregion
    }
}

