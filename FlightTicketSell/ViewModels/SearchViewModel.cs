using FlightTicketSell.Views;
using FlightTicketSell.Models;
using FlightTicketSell.ViewModels.Search;
using FlightTicketSell.Views.SearchViewMore;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;
using System.Data.Entity.Core;
using System.Windows;

namespace FlightTicketSell.ViewModels
{
    public class SearchViewModel : BaseViewModel
    {
        public string Title { get; } = "TRA CỨU";

        public ICommand LoadCommand { get; set; }
        public ICommand SanBayDoi { get; set; }

        public ICommand ReturnCommand { get; set; }

        //public ICommand Open_Window_DescriptionTicketFlight_Command { get; set; }

        public ObservableCollection<Airport_Search> Airports { get; set; }
        public ObservableCollection<Datagrid_Search> Flights { get; set; }

        public Airport_Search SanBayDii { get; set; }
        public Airport_Search SanBayDenn { get; set; }


        public System.DateTime? _date;
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
        public SearchViewModel()
        {

            LoadCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                using (var context = new FlightTicketSellEntities())
                {
                    try
                    {
                        //System.DateTime? selectedDate = searchView.dp.SelectedDate.Value;
                        // Airports
                        Airports = new ObservableCollection<Airport_Search>(context.SANBAYs.Select(s => new Airport_Search { ID = s.MaSanBay, Name = s.TenSanBay }).ToList());

                      
                       
                    }
                    catch (EntityException)
                    {
                        // TODO: messagebox vo
                        return;
                    }
                }
            });

            ReturnCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.Search);

            SanBayDoi = new RelayCommand<object>((p) => true, (p) =>
            {
                using (var context = new FlightTicketSellEntities())
                {
                    try
                    {
                        //System.DateTime? selectedDate = searchView.dp.SelectedDate.Value;
                        if (SanBayDii != null && SanBayDenn != null)
                        {
                            Flights = new ObservableCollection<Datagrid_Search>(
                                                           context.GetFlightData().Where(result =>
                                                               result.SanBayDi == SanBayDii.Name && result.SanBayDen == SanBayDenn.Name)
                                                               .Select(result => new Datagrid_Search
                                                               {
                                                                   MaChuyenBay = result.MaChuyenBay.ToString(),
                                                                   SanBayDi = result.SanBayDi,
                                                                   SanBayDen = result.SanBayDen,
                                                                   KhoiHanh = result.NgayGio,
                                                                   SoDiemDung = result.SoDiemDung.ToString(),
                                                                   SoHangVe = result.SoLuongVe.ToString(),
                                                                   GiaCoBan = result.GiaVe,
                                                                   GheTrong = result.GheTrong.ToString()
                                                               }).ToList()
                                                           );
                        }
                    }
                    catch (EntityException)
                    {
                        // TODO: messagebox vo
                        return;
                    }
                }
            });

        }

    }
}
