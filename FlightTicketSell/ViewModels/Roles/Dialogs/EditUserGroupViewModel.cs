using FlightTicketSell.Helpers;
using FlightTicketSell.Interface;
using FlightTicketSell.Models;
using FlightTicketSell.Models.Enums;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FlightTicketSell.ViewModels
{
    public class EditUserGroupViewModel : BaseViewModel
    {
        #region Parent View Model

        /// <summary>
        /// The parent view model of this view model
        /// </summary>
        public RolesViewModel ParentViewModel { get; set; }

        #endregion

        #region Public Properties

        /// <summary>
        /// The old user group name
        /// </summary>
        public string OldUserGroupName { get; set; }

        /// <summary>
        /// The user group name
        /// </summary>
        public string UserGroupName { get; set; }

        public bool IsInteractable { get; set; } = true;

        #endregion

        #region Commands

        /// <summary>
        /// Save the user group down the database
        /// </summary>
        public ICommand SaveCommand { get; set; }

        /// <summary>
        /// Cancel edit user group and close the dialog
        /// </summar
        public ICommand CancelCommand { get; set; }

        public ICommand LoadCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public EditUserGroupViewModel()
        {
            // Create commands

            LoadCommand = new RelayCommand<object>(p => true, p =>
            {
                IsInteractable = false;

                OldUserGroupName = ParentViewModel.CurrentUserGroup.Name;

                IsInteractable = true;
            });

            CancelCommand = new RelayCommand<IEditUserGroupDialog>(p => true, p =>
            {
                IsInteractable = false;


                p.Close();

                IsInteractable = true;
            });

            SaveCommand = new RelayCommand<IEditUserGroupDialog>(p => true, async p =>
            {
                IsInteractable = false;

                var result = await SaveUserGroup();

                switch (result)
                {
                    case ActionResult.Duplicate:
                        p.Notify(result, await DatabaseHelper.LoadUserGroup(ParentViewModel.CurrentUserGroup.Code, UserGroupType.Normal));
                        break;
                    case ActionResult.Succcesful:
                        p.Notify(result);
                        p.Close();
                        break;
                    case ActionResult.Error:
                        p.Notify(result);
                        break;
                    case ActionResult.NotEnoughInformationForAction:
                        p.Notify(result);
                        break;
                }

                IsInteractable = true;
            });
        }

        #endregion

        #region Methods

        /// <summary>
        /// Save user group down database
        /// </summary>
        /// <returns></returns>
        private async Task<ActionResult> SaveUserGroup()
        {
            if (string.IsNullOrEmpty(UserGroupName))
            {
                return ActionResult.NotEnoughInformationForAction;
            }

            // TODO: Clean this
            using (var context = new FlightTicketSellEntities())
            {
                try
                {
                    var userGroup = await context.NHOMNGUOIDUNGs.Where(nnd => nnd.MaNhom == ParentViewModel.CurrentUserGroup.Code).FirstOrDefaultAsync();

                    userGroup.TenNhom = UserGroupName;
                    await context.SaveChangesAsync();

                    ParentViewModel.UserGroupList.Where(nnd => nnd.Code == ParentViewModel.CurrentUserGroup.Code).FirstOrDefault().Name = UserGroupName;

                    return ActionResult.Succcesful;
                }
                catch (EntityException ex)
                {
                    NotifyHelper.ShowEntityException(ex);
                    return ActionResult.Fail;
                }
            }

            
        }

        #endregion
    }
}
