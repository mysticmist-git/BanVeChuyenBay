using FlightTicketSell.Views;
using System.Windows.Input;

namespace FlightTicketSell.ViewModels
{
    public class SettingViewModel : BaseViewModel
    {
        #region Commands
        public ICommand Open_Window_MoreAirport_Command { get; set; }
        public ICommand Open_Window_MoreTicketClass_Command { get; set; }
        #endregion

        public string Title { get; } = "CÀI ĐẶT";

        public SettingViewModel()
        {
            Open_Window_MoreAirport_Command = new RelayCommand<object>(

                (p) => { return true; },

                (p) =>
                {
                    MoreAirportView moreAirportView = new MoreAirportView();
                    moreAirportView.ShowDialog();
                }
            );

            Open_Window_MoreTicketClass_Command = new RelayCommand<object>(

                (p) => { return true; },

                (p) =>
                {
                    MoreTicketClassView moreTicketClassView = new MoreTicketClassView();
                    moreTicketClassView.ShowDialog();
                }
            );
        }
    }
}
