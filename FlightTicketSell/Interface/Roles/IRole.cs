using FlightTicketSell.Models.Enums;
using System.Threading.Tasks;

namespace FlightTicketSell.Interface
{
    public interface IRole
    {
        /// <summary>
        /// Open the add user group dialog
        /// </summary>
        Task OpenAddUserGroupDialog();

        /// <summary>
        /// Open the add user group dialog
        /// </summary>
        Task OpenEditUserGroupDialog();

        /// <summary>
        /// Open a messagebox ask user if they really want to remove selected user group
        /// </summary>
        DecisionResult UserGroupRemoveConfirm();

        /// <summary>
        /// Notify when doing remove user group
        /// </summary>
        /// <param name="actionResult"></param>
        void RemoveUserGroupNotify(ActionResult decisionResult);

        /// <summary>
        /// Notify like with information for a messagebox
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name=""></param>
        /// <param name=""></param>
        DecisionResult RemoveUserGroupNotify(string message, string title, DecisionType decisionType, DecisionIcon decisionIcon);
    }
}
