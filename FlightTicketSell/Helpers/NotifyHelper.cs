using System;
using System.Data.Entity.Core;
using System.Windows;

namespace FlightTicketSell.Helpers
{
    public class NotifyHelper
    {
        public static void ShowEntityException(EntityException e)
        {
            MessageBox.Show("Database access failed!", string.Format($"Exception: {e.Message}"), MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}