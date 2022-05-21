using FlightTicketSell.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;
using System.Data.Entity.Core;
using System.Windows;
using System.Data.Entity;
using FlightTicketSell.Models.SearchRelated;
using FlightTicketSell.Helpers;

namespace FlightTicketSell.ViewModels
{
    public class SearchViewModel : BaseViewModel
    {
        #region Private Members

        /// <summary>
        /// The depart date of the flights
        /// </summary>
        public System.DateTime? _date;

        #endregion

        #region Public Properties

        /// <summary>
        /// Airport list
        /// </summary>
        public ObservableCollection<Airport_Search> Airports { get; set; }

        /// <summary>
        /// Indicates if we'll show departed flights
        /// </summary>
        public bool IsDisplayDeparted { get; set; } = false;

        /// <summary>
        /// Flight list
        /// </summary>
        public ObservableCollection<FlightInfo> Flights { get; set; }

        /// <summary>
        /// The processed flight list
        /// </summary>
        public ObservableCollection<FlightInfo> ProcessedFlights
        {
            get
            {
                if (Flights is null)
                    return null;
                
                ObservableCollection<FlightInfo> flightInfos = (IsDisplayDeparted ?
                                Flights :
                                new ObservableCollection<FlightInfo>(Flights.Where(f => f.DaKhoiHanh == false).ToList()));
                return flightInfos;
            }
        }

        /// <summary>
        /// Start airport param
        /// </summary>
        public Airport_Search SanBayDii { get; set; }

        /// <summary>
        /// Destinattion airport param
        /// </summary>
        public Airport_Search SanBayDenn { get; set; }

        public Airport_Search LandingAirport { get; set; } = new Airport_Search();

        public Airport_Search DepartureAirport { get; set; } = new Airport_Search();

        public string FlightCode { get; set; }

        public System.DateTime? DateOfEntry
        {
            get
            {
                return _date;
            }
            set
            {
                _date = value;
            }
        }

        public bool IsLoadFinished { get; set; } = false;

        #endregion

        #region Commands

        /// <summary>
        /// Execute this first when view load
        /// </summary>
        public ICommand LoadCommand { get; set; }

        public ICommand Change_Departure_Landing_Airport_Command { get; set; }

        /// <summary>
        /// When params changed, use it to changes flight list
        /// </summary>
        public ICommand ParamsChangedCommand { get; set; }

        /// <summary>
        /// Return the the previous view
        /// </summary>
        public ICommand ReturnCommand { get; set; }

        public ICommand Open_Window_DescriptionTicketFlight_Command { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Defaultl constructor
        /// </summary>
        public SearchViewModel()
        {
            LoadCommand = new RelayCommand<object>((p) => true, async (p) =>
            {
                // Loading
                IsLoadFinished = false;

                // Refresh flight to update departed flight
                await FlightHelper.FlightDepartedRefresh();

                // Get airports for combobox and initialize flight table
                using (var context = new FlightTicketSellEntities())
                {
                    try
                    {
                        // Load airports
                        Airports = new ObservableCollection<Airport_Search>(await context.SANBAYs.Select(s => new Airport_Search { ID = s.MaSanBay, Name = s.TenSanBay }).ToListAsync());

                        // Load flights list on first load
                        var result = await context.Database.SqlQuery<GetFlightData_Result>("EXEC GetFlightData").ToListAsync();
                        Flights = new ObservableCollection<FlightInfo>(result.Select(f => new FlightInfo(f)));
                    }
                    catch (EntityException e)
                    {
                        MessageBox.Show("Database access failed!", string.Format($"Exception: {e.Message}"), MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

                // Load finish
                IsLoadFinished = true;
            });

            Change_Departure_Landing_Airport_Command = new RelayCommand<object>((p) => { return true; },
               async (p) =>
               {
                   (SanBayDii, SanBayDenn) = (SanBayDenn, SanBayDii);
                   if (SanBayDii != null && SanBayDenn != null)
                   {
                       if (string.IsNullOrEmpty(SanBayDenn.Name) || string.IsNullOrEmpty(SanBayDii.Name))
                           return;
                       try
                       {
                           string temp = SanBayDii.Name;
                           SanBayDii.Name = SanBayDenn.Name;
                           SanBayDenn.Name = temp;
                       }
                       catch (EntityException e)
                       {
                           MessageBox.Show($"Exception: {e.Message}");
                       }
                   }

                   // Loading
                   IsLoadFinished = false;

                   // Refresh flight to update departed flight
                   await FlightHelper.FlightDepartedRefresh();

                   // Load new flight table
                   using (var context = new FlightTicketSellEntities())
                   {
                       try
                       {
                           // Get flights
                           var result = await context.Database.SqlQuery<GetFlightData_Result>("EXEC GetFlightData").ToListAsync();
                           //var result = context.GetFlightData().ToList();

                           // Apply Start airport param if there is
                           if (SanBayDii != null)
                               result = result.Where(f => f.MaSanBayDi == SanBayDii.ID).ToList();
                           // Apply Destination airport param if there is
                           if (SanBayDenn != null)
                               result = result.Where(f => f.MaSanBayDen == SanBayDenn.ID).ToList();
                           // Apply Date param if there is
                           if (DateOfEntry != null)
                               result = result.Where(f => f.NgayGio == DateOfEntry.Value).ToList();

                           Flights = new ObservableCollection<FlightInfo>(result.Select(f => new FlightInfo(f)));
                       }
                       catch (EntityException e)
                       {
                           MessageBox.Show("Database access failed!", string.Format($"Exception: {e.Message}"), MessageBoxButton.OK, MessageBoxImage.Error);
                       }
                   }

                   // Load finish
                   IsLoadFinished = true;
               }
           );

            ReturnCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.Search);

            ParamsChangedCommand = new RelayCommand<object>((p) => true, async (p) =>
            {
                // Loading
                IsLoadFinished = false;

                // Refresh flight to update departed flight
                await FlightHelper.FlightDepartedRefresh();

                // Load new flight table
                using (var context = new FlightTicketSellEntities())
                {
                    try
                    {
                        // Get flights
                        var result = await context.Database.SqlQuery<GetFlightData_Result>("EXEC GetFlightData").ToListAsync();
                        //var result = context.GetFlightData().ToList();

                        // Apply Start airport param if there is
                        if (SanBayDii != null)
                            result = result.Where(f => f.MaSanBayDi == SanBayDii.ID).ToList();
                        // Apply Destination airport param if there is
                        if (SanBayDenn != null)
                            result = result.Where(f => f.MaSanBayDen == SanBayDenn.ID).ToList();
                        // Apply Date param if there is
                        if (DateOfEntry != null)
                            result = result.Where(f => f.NgayGio == DateOfEntry.Value).ToList();

                        Flights = new ObservableCollection<FlightInfo>(result.Select(f => new FlightInfo(f)));
                    }
                    catch (EntityException e)
                    {
                        MessageBox.Show("Database access failed!", string.Format($"Exception: {e.Message}"), MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

                // Load finish
                IsLoadFinished = true;
            });

        }

        #endregion

    }
}