using FlightTicketSell.Interface;
using System;
using System.Globalization;

namespace FlightTicketSell.ValueConverters
{
    /// <summary>
    /// Indicates the view interface
    /// </summary>
    public enum ViewInterface
    {
        TicketInfoFilling,
        Report,
        BookDetail,
        CustomerDetail_Search,
        Login,

        #region Role

        Role,
        AddUserGroupDialog,
        EditUserGroupDialog

        #endregion
    }

    public class ViewToInterfaceConverter : BaseValueConverter<ViewToInterfaceConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (parameter as ViewInterface?)
            {
                case ViewInterface.TicketInfoFilling:
                    return value as ITicketInfoFilling;
                case ViewInterface.Report:
                    return value as IReport;
                case ViewInterface.BookDetail:
                    return value as IBookDetail;
                case ViewInterface.CustomerDetail_Search:
                    return value as ICustomerDetail;
                case ViewInterface.Login:
                    return value as ILogin;

                // Roles
                case ViewInterface.Role:
                    return value as IRole;
                case ViewInterface.AddUserGroupDialog:
                    return value as IAddUserGroupDialog;
                case ViewInterface.EditUserGroupDialog:
                    return value as IEditUserGroupDialog;
            }

            return null;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
