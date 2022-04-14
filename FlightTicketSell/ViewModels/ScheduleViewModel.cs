using FlightTicketSell.Views;
using System.Windows.Input;

namespace FlightTicketSell.ViewModels
{
    public class ScheduleViewModel : BaseViewModel
    {
        #region Commands
        public ICommand Open_Window_EnterLayoverAirport_Command { get; set; }
        public ICommand Open_Window_EnterTicketClass_Command { get; set; }
        #endregion
        public string Title { get; } = "NHẬN LỊCH";

        public ScheduleViewModel()
        {
            Open_Window_EnterLayoverAirport_Command = new RelayCommand<object>(

                (p) => { return true; },

                (p) =>
                {
                    EnterLayoverAirportView enterLayoverAirportView = new EnterLayoverAirportView();
                    enterLayoverAirportView.ShowDialog();
                }
            );

            Open_Window_EnterTicketClass_Command = new RelayCommand<object>(

                (p) => { return true; },

                (p) =>
                {
                    EnterTicketClassView enterTicketClassView = new EnterTicketClassView();
                    enterTicketClassView.ShowDialog();
                }
            );
        }
    }
}
