using FlightTicketSell.Helpers;
using FlightTicketSell.Interface;
using FlightTicketSell.Models;
using FlightTicketSell.Models.Enums;
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
        /// The old user group code
        /// </summary>
        public string OldUserGroupCode { get; set; }

        /// <summary>
        /// The old user group name
        /// </summary>
        public string OldUserGroupName { get; set; }

        /// <summary>
        /// The user group code
        /// </summary>
        public string UserGroupCode { get; set; }

        /// <summary>
        /// The user group name
        /// </summary>
        public string UserGroupName { get; set; }

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

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public EditUserGroupViewModel()
        {
            // Create commands
            CancelCommand = new RelayCommand<IEditUserGroupDialog>(p => true, p =>
            {
                p.Close();
            });

            SaveCommand = new RelayCommand<IEditUserGroupDialog>(p => true, async p =>
            {
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
