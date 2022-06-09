using FlightTicketSell.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;
using System.Data.Entity.Core;
using System.Windows;
using System.Data.Entity;
using FlightTicketSell.Models.SearchRelated;
using FlightTicketSell.Helpers;
using MaterialDesignThemes.Wpf;
using FlightTicketSell.Views;
using System.Threading.Tasks;

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

        #region ChooseAirport
        /// <summary>
        /// Nút đổi sân bay
        /// </summary>
        public ICommand Change_Departure_Landing_Airport_Command { get; set; }
        /// <summary>
        /// Hàm load khi chọn sân bay
        /// </summary>
        public ICommand ChooseAirport_LoadCommand { get; set; }
        /// <summary>
        /// Nút chọn trong Chọn sân bay
        /// </summary>
        public ICommand ChooseAirportButton_Command { get; set; }
        /// <summary>
        /// Nút hủy trong Chọn sân bay
        /// </summary>
        public ICommand Cancel_ChooseAirportButton_Command { get; set; }
        /// <summary>
        /// Chọn sân bay đi
        /// </summary>
        public ICommand ChooseDepartureAirport_Command { get; set; }
        /// <summary>
        /// Chọn sân bay đến
        /// </summary>
        public ICommand ChooseLandingAirport_Command { get; set; }
        #endregion

        #region ChooseDate
        public ICommand SelectedDateChangedCommand { get; set; }
        #endregion

        #region Public Properties

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
        /// <summary>
        /// Danh sách sân bay khi load Chọn sân bay
        /// </summary>
        public ObservableCollection<Airport_Search> ChooseAirport_List { get; set; } = new ObservableCollection<Airport_Search>();
        /// <summary>
        /// Sân bay được chọn của list trong Chọn sân bay
        /// </summary>
        public Airport_Search ChooseAirport_SelectedItem { get; set; } = new Airport_Search();
        /// <summary>
        /// Biến đánh dấu thành phần nào đã mở DialogHost Chọn sân bay
        /// </summary>
        private static string OpenChooseAirport { get; set; }
        /// <summary>
        /// Biến check lần đầu load
        /// </summary>
        private bool FirstLoad { get; set; } = true;
        #endregion

        #region Commands
        /// <summary>
        /// Cập nhật Table khi các thành phần search đổi
        /// </summary>
        public ICommand ParamsChangedCommand { get; set; }
        /// <summary>
        /// Execute this first when view load
        /// </summary>
        public ICommand LoadCommand { get; set; }
        /// <summary>
        /// Return the the previous view
        /// </summary>
        public ICommand ReturnCommand { get; set; }

        public ICommand Open_Window_DescriptionTicketFlight_Command { get; set; }


        #endregion

        #region Method
        /// <summary>
        /// Loại bỏ 1 phần tử trong ObservableCollection
        /// </summary>
        public bool RemoveAirportItem(ObservableCollection<Airport_Search> airports, Airport_Search airport)
        {
            if (airports.Count == 0)
                return false;
            foreach (var airportItem in airports)
            {
                if (airport.ID == airportItem.ID)
                {
                    airports.Remove(airportItem);
                    return true;
                }
            }
            return false;
        }
        #endregion


        #region Constructor

        /// <summary>
        /// Defaultl constructor
        /// </summary>
        public SearchViewModel()
        {
            LoadCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                ParamsChangedCommand.Execute(null);
            });

            ParamsChangedCommand = new RelayCommand<object>((p) => true, async (p) =>
               {

                   //if (SanBayDii == null && SanBayDenn == null)
                   //{
                   //    _ = FristLoadFlightTable();
                   //    return;
                   //}
                   // Loading
                   if (SanBayDii != null && string.IsNullOrEmpty(SanBayDii.Name))
                   {
                       SanBayDii = null;
                   }
                   if (SanBayDenn != null && string.IsNullOrEmpty(SanBayDenn.Name))
                   {
                       SanBayDenn = null;
                   }
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
                           if (SanBayDii != null && !string.IsNullOrEmpty(SanBayDii.Name))
                               result = result.Where(f => f.MaSanBayDi == SanBayDii.ID).ToList();
                           // Apply Destination airport param if there is
                           if (SanBayDenn != null && !string.IsNullOrEmpty(SanBayDenn.Name))
                               result = result.Where(f => f.MaSanBayDen == SanBayDenn.ID).ToList();
                           // Apply Date param if there is
                           if (DateOfEntry != null && !string.IsNullOrEmpty(DateOfEntry.Value.ToString()))
                               result = result.Where(f => f.NgayGio.ToString("MM-dd-yyyy") == DateOfEntry.Value.ToString("MM-dd-yyyy")).ToList();
                           Flights = new ObservableCollection<FlightInfo>(result.Select(f => new FlightInfo(f)));
                       }
                       catch (EntityException e)
                       {
                           NotifyHelper.ShowEntityException(e);;
                       }
                   }

                   // Load finish
                   IsLoadFinished = true;
               });

            SelectedDateChangedCommand = new RelayCommand<object>((p) => true, async (p) =>
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
                        NotifyHelper.ShowEntityException(e);;
                    }
                }

                // Load finish
                IsLoadFinished = true;
            });

            Change_Departure_Landing_Airport_Command = new RelayCommand<object>((p) => { return true; },
               (p) =>
               {
                   (SanBayDii, SanBayDenn) = (SanBayDenn, SanBayDii);
                   ParamsChangedCommand.Execute(null);
               }
           );

            ReturnCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.Search);


            ChooseDepartureAirport_Command = new RelayCommand<object>((p) => { return true; },
          async (p) =>
          {

              ChooseAirportViewInSearchView chooseAirportViewInSearchView = new ChooseAirportViewInSearchView { DataContext = this };
              OpenChooseAirport = "Departure";
              var result = await DialogHost.Show(chooseAirportViewInSearchView, "RootDialog");
          }
        );
            ChooseLandingAirport_Command = new RelayCommand<object>((p) => { return true; },
             async (p) =>
             {

                 ChooseAirportViewInSearchView chooseAirportViewInSearchView = new ChooseAirportViewInSearchView { DataContext = this };
                 OpenChooseAirport = "Landing";
                 var result = await DialogHost.Show(chooseAirportViewInSearchView, "RootDialog");

             }
           );

            ChooseAirport_LoadCommand = new RelayCommand<object>((p) => { return true; },
             (p) =>
             {
                 using (var context = new FlightTicketSellEntities())
                 {
                     try
                     {
                         // Danh sách sân bay
                         if (ChooseAirport_List != null)
                         {
                             ChooseAirport_List.Clear();
                         }
                         foreach (var item in context.SANBAYs.ToList())
                         {
                             //Sân bay còn hoạt động mới thêm vào list
                             if (item.TrangThai)
                                 ChooseAirport_List.Add(new Airport_Search() { ID = item.MaSanBay, Code = item.VietTat, Name = item.TenSanBay, Province = item.TinhThanh, Status = item.TrangThai });
                         }
                         if (SanBayDenn != null && !string.IsNullOrEmpty(SanBayDenn.Name))
                         {
                             RemoveAirportItem(ChooseAirport_List, SanBayDenn);
                         }
                         if (SanBayDii != null && !string.IsNullOrEmpty(SanBayDii.Name))
                         {
                             RemoveAirportItem(ChooseAirport_List, SanBayDii);
                         }


                     }
                     catch (EntityException e)
                     {
                         NotifyHelper.ShowEntityException(e);;
                     }
                 }
             }
         );

            ChooseAirportButton_Command = new RelayCommand<object>((p) => { return true; },
           (p) =>
           {
               //if (string.IsNullOrEmpty(OpenChooseAirport) || ChooseAirport_SelectedItem == null)
               //{
               //    MessageBox.Show("Hãy chọn sân bay!", "Cảnh báo");
               //    return;
               //}

               if (ChooseAirport_SelectedItem == null)
               {
                   return;
               }

               if (OpenChooseAirport == "Departure")
               {
                   SanBayDii = new Airport_Search()
                   {
                       ID = ChooseAirport_SelectedItem.ID,
                       Name = ChooseAirport_SelectedItem.Name,
                       Province = ChooseAirport_SelectedItem.Province,
                       Code = ChooseAirport_SelectedItem.Code,
                       Status = ChooseAirport_SelectedItem.Status
                   };
               }
               if (OpenChooseAirport == "Landing")
               {
                   SanBayDenn = new Airport_Search()
                   {
                       ID = ChooseAirport_SelectedItem.ID,
                       Name = ChooseAirport_SelectedItem.Name,
                       Province = ChooseAirport_SelectedItem.Province,
                       Code = ChooseAirport_SelectedItem.Code,
                       Status = ChooseAirport_SelectedItem.Status
                   };
               }
               // Close dialog
               DialogHost.CloseDialogCommand.Execute(null, null);
           }
        );
            Cancel_ChooseAirportButton_Command = new RelayCommand<object>((p) => { return true; },
             (p) =>
             {
                 // Close dialog
                 DialogHost.CloseDialogCommand.Execute(null, null);
                 if (OpenChooseAirport == "Departure")
                 {
                     SanBayDii = null;
                 }
                 if (OpenChooseAirport == "Landing")
                 {
                     SanBayDenn = null;
                 }
             });

        }

        #endregion

    }
}