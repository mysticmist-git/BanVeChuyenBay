using FlightTicketSell.Interface;
using FlightTicketSell.Models.Enums;
using FlightTicketSell.ViewModels;
using MaterialDesignThemes.Wpf;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FlightTicketSell.Views
{
    /// <summary>
    /// Interaction logic for RolesView.xaml
    /// </summary>
    public partial class RolesView : UserControl, IRole
    {
        public RolesView()
        {
            InitializeComponent();
        }

        #region IRole

        public async Task OpenAddUserGroupDialog()
        {
            AddUserGroupDialog view = new AddUserGroupDialog();
            (view.DataContext as AddUserGroupViewModel).ParentViewModel = this.DataContext as RolesViewModel;

            await DialogHost.Show(view, "RootDialog");
        }

        public async Task OpenEditUserGroupDialog()
        {
            EditUserGroupDialog view = new EditUserGroupDialog();
            (view.DataContext as EditUserGroupViewModel).ParentViewModel = this.DataContext as RolesViewModel;

            await DialogHost.Show(view, "RootDialog");
        }

        public void RemoveUserGroupNotify(ActionResult result)
        {
            switch (result)
            {
                case ActionResult.Fail:
                    MessageBox.Show("Xin chọn nhóm người dùng", "Chưa chọn nhóm người dùng", MessageBoxButton.OK);
                    break;
                case ActionResult.Succcesful:
                    MessageBox.Show("Xóa thành công!", "Thành công", MessageBoxButton.OK);
                    break;
                case ActionResult.Error:
                    MessageBox.Show("Vui lòng chọn nhóm người dùng", "Không thể xóa", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }
        }

        public DecisionResult RemoveUserGroupNotify(string message, string title, DecisionType decisionType, DecisionIcon decisionIcon)
        {
            MessageBoxButton button;
            MessageBoxImage image;

            switch (decisionType)
            {
                case DecisionType.YesNo:
                    button = MessageBoxButton.YesNo;
                    break;
                case DecisionType.YesNoCancel:
                    button = MessageBoxButton.YesNoCancel;
                    break;
                case DecisionType.OK:
                    button = MessageBoxButton.OK;
                    break;
                case DecisionType.OKCancel:
                    button = MessageBoxButton.OKCancel;
                    break;
                default:
                    button = MessageBoxButton.OK;
                    break;
            }

            switch (decisionIcon)
            {
                case DecisionIcon.Asterisk:
                    image = MessageBoxImage.Asterisk;
                    break;
                case DecisionIcon.Question:
                    image = MessageBoxImage.Question;
                    break;
                case DecisionIcon.Exclamation:
                    image = MessageBoxImage.Exclamation;
                    break;
                case DecisionIcon.Error:
                    image = MessageBoxImage.Error;
                    break;
                case DecisionIcon.Hand:
                    image = MessageBoxImage.Hand;
                    break;
                case DecisionIcon.Information:
                    image = MessageBoxImage.Information;
                    break;
                case DecisionIcon.Warning:
                    image = MessageBoxImage.Warning;
                    break;
                case DecisionIcon.None:
                    image = MessageBoxImage.None;
                    break;
                default:
                    image = MessageBoxImage.None;
                    break;
            }

            var result = MessageBox.Show(message, title, button, image);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    return DecisionResult.Yes;
                case MessageBoxResult.No:
                    return DecisionResult.No;
                case MessageBoxResult.Cancel:
                    return DecisionResult.Cancel;
                case MessageBoxResult.OK:
                    return DecisionResult.OK;
                default:
                    return DecisionResult.None;
                        
            }
        }

        public DecisionResult UserGroupRemoveConfirm()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
