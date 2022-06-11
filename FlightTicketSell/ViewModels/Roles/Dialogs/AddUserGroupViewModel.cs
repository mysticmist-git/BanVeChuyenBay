using FlightTicketSell.Helpers;
using FlightTicketSell.Interface;
using FlightTicketSell.Models;
using FlightTicketSell.Models.Enums;
using System;
using System.Data.Entity.Core;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Data.Entity;

namespace FlightTicketSell.ViewModels
{
    public class AddUserGroupViewModel : BaseViewModel
    {
        #region Parent View Model

        /// <summary>
        /// The parent view model of this view model
        /// </summary>
        public RolesViewModel ParentViewModel { get; set; }

        #endregion

        #region Public Properties

        /// <summary>
        /// The user group code that user enter
        /// </summary>
        public string UserGroupCode { get; set; }

        /// <summary>
        /// The user group name that user enter
        /// </summary>
        public string UserGroupName { get; set; }

        public bool IsSavable { get; set; } = true;
        public bool IsCancelable { get; set; } = true;

        public bool IsInteractable { get; set; } = true;

        #endregion

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

        /// <summary>
        /// Default constructor
        /// </summary>
        public AddUserGroupViewModel()
        {
            // Create commands
            CancelCommand = new RelayCommand<IAddUserGroupDialog>(p => true, p =>
            {
                IsCancelable = false;
                IsInteractable = false;

                if (string.IsNullOrEmpty(UserGroupName) && string.IsNullOrEmpty(UserGroupCode))
                {
                    p.Close();
                    return;
                }

                var result = p.CancelConfirm();

                switch (result)
                {
                    case System.Windows.MessageBoxResult.Yes:
                        SaveCommand.Execute(p);
                        break;
                    case System.Windows.MessageBoxResult.Cancel:
                        break;
                    case System.Windows.MessageBoxResult.No:
                        p.Close();
                        break;
                }

                IsCancelable = true;
                IsInteractable = true;
            });

            SaveCommand = new RelayCommand<IAddUserGroupDialog>(p => true, async p =>
            {
                IsSavable = false;
                IsInteractable = false;

                var result = await SaveUserGroup();

                switch (result)
                {
                    case ActionResult.Duplicate:
                        p.Notify(result, await DatabaseHelper.LoadUserGroup(UserGroupCode, UserGroupType.Normal));
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

                IsSavable = true;
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
            if (string.IsNullOrEmpty(UserGroupName) || string.IsNullOrEmpty(UserGroupCode))
            {
                return ActionResult.NotEnoughInformationForAction;
            }

            var userGroup = this.CreateUserGroup();
            var result = await DatabaseHelper.SaveUserGroup(userGroup);
            ParentViewModel.UserGroupList.Add((UserGroupModifiedWithUsers)await DatabaseHelper.LoadUserGroup(userGroup.Code, UserGroupType.ModifiedWithUsers));

            return result;
        }

        /// <summary>
        /// Create user group with <see cref="UserGroupCode"/> and <see cref="UserGroupName"/>
        /// </summary>
        /// <returns></returns>
        private UserGroup CreateUserGroup()
        {
            if (UserGroupCode is null)
                return null;

            return new UserGroup()
            {
                Code = UserGroupCode,
                Name = UserGroupName
            };
        }

        #endregion
    }
}
