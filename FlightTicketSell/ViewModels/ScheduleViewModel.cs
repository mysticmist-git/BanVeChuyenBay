using FlightTicketSell.Models;
using FlightTicketSell.Views;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FlightTicketSell.ViewModels
{
    public class ScheduleViewModel : BaseViewModel
    {
        #region Commands
        /// <summary>
        /// Mở nhập sân bay trung gian
        /// </summary>
        public ICommand Open_Window_EnterLayoverAirport_Command { get; set; }
        /// <summary>
        /// Mở nhập hạng vé
        /// </summary>
        public ICommand Open_Window_EnterTicketClass_Command { get; set; }
        /// <summary>
        /// Nút đổi sân bay đi & sân bay đến
        /// </summary>
        public ICommand Change_Departure_Landing_Airport_Command { get; set; }
        /// <summary>
        /// Sân bay đi không trùng sân bay đến
        /// </summary>
        public ICommand Keep_Departure_DifferentFrom_Landing_Command { get; set; }
        public ICommand Keep_Landing_DifferentFrom_Departure_Command { get; set; }
        public ICommand LoadCommand { get; set; }
        #endregion

        #region Public Properties
        /// <summary>
        /// Mã chuyến bay
        /// </summary>
        public string FlightCode { get; set; }

        /// <summary>
        /// Giá vé
        /// </summary>
        public int Airfares { get; set; }

        /// <summary>
        /// Thời gian bay
        /// </summary>
        public int FlightTime { get; set; }

        /// <summary>
        /// Ngày bay
        /// </summary>
        public DateTime DateFlight { get; set; }
        /// <summary>
        /// Giờ bay
        /// </summary>
        public DateTime TimeFlight { get; set; } 
        /// <summary>
        /// Sân bay đi
        /// </summary>
        public ComboBox DepartureAirport { get; set; } = new ComboBox();

        /// <summary>
        /// Sân bay đến
        /// </summary>
        public ComboBox LandingAirport { get; set; } = new ComboBox();

        /// <summary>
        /// Danh sách sân bay trung gian
        /// </summary>
        //public ObservableCollection<Airport> List_Airport { get; set; } = new ObservableCollection<Airport>();

        /// <summary>
        /// Danh sách hạng vé
        /// </summary>
        //public ObservableCollection<TicketClass> List_TicketClass { get; set; } = new ObservableCollection<TicketClass>();

        #endregion


        public string Title { get; } = "NHẬN LỊCH";

        public ScheduleViewModel()
        {
            #region Main Command
           
            LoadCommand = new RelayCommand<object>((p) => { return true; },
               (p) =>
               {
                   try
                   {
                       // Gán ngày hiện tại cho datepicker lúc mở 
                       DateFlight = DateTime.Now;
                       // Gán ngày hiện tại cho datepicker lúc mở 
                       TimeFlight = DateTime.Now;

                       using (var context = new FlightTicketSellEntities())
                       {
                           //Danh sách sân bay đi
                           if (DepartureAirport.Items.Count > 0)
                           {
                               DepartureAirport.Items.Clear();
                           }
                           foreach (var item in context.SANBAYs.ToList())
                           {
                               DepartureAirport.Items.Add(item.TenSanBay);
                           }

                           //Danh sách sân bay đến
                           if (LandingAirport.Items.Count > 0)
                           {
                               LandingAirport.Items.Clear();
                           }
                           foreach (var item in context.SANBAYs.ToList())
                           {
                               LandingAirport.Items.Add(item.TenSanBay);
                           }
                       }
                   }
                   catch (System.Data.Entity.Core.EntityException e)
                   {
                       MessageBox.Show($"Exception: {e.Message}");
                   }
               }
           );

            Change_Departure_Landing_Airport_Command = new RelayCommand<object>((p) => { return true; },
               (p) =>
               {
                   var temp1 = DepartureAirport.SelectedIndex;
                   var temp2 = LandingAirport.SelectedIndex;
                   LandingAirport.SelectedIndex = -1;
                   DepartureAirport.SelectedIndex = temp2;
                   LandingAirport.SelectedIndex = temp1;

               }
           );

            Open_Window_EnterLayoverAirport_Command = new RelayCommand<object>((p) => { return true; },
                (p) =>
                {
                    EnterLayoverAirportView enterLayoverAirportView = new EnterLayoverAirportView();
                    enterLayoverAirportView.ShowDialog();
                }
            );

            Open_Window_EnterTicketClass_Command = new RelayCommand<object>((p) => { return true; },
                (p) =>
                {
                    EnterTicketClassView enterTicketClassView = new EnterTicketClassView();
                    enterTicketClassView.ShowDialog();
                }
            );

            Keep_Departure_DifferentFrom_Landing_Command = new RelayCommand<object>((p) => { return true; },
                (p) =>
                {
                    if (DepartureAirport.SelectedIndex != -1)
                    {
                        if (LandingAirport.SelectedIndex != -1)
                        {
                            if (DepartureAirport.SelectedIndex == LandingAirport.SelectedIndex)
                            {
                                _ = MessageBox.Show("Sân bay đi không trùng với sân bay đến", "Thông báo");
                                DepartureAirport.SelectedIndex = -1;
                            }
                        }
                    }
                }
            );

            Keep_Landing_DifferentFrom_Departure_Command = new RelayCommand<object>((p) => { return true; },
                (p) =>
                {
                    if (LandingAirport.SelectedIndex != -1)
                    {
                        if (DepartureAirport.SelectedIndex != -1)
                        {
                            if (DepartureAirport.SelectedIndex == LandingAirport.SelectedIndex)
                            {
                                _ = MessageBox.Show("Sân bay đến không trùng với sân bay đi", "Thông báo");
                                LandingAirport.SelectedIndex = -1;
                            }
                        }
                    }
                }
            );


            #endregion


        }
    }
}
