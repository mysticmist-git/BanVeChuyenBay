﻿using FlightTicketSell.Views;
using FlightTicketSell.Models;
using FlightTicketSell.ViewModels.Search;
using FlightTicketSell.ViewModels.Sell;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;
using System.Data.Entity.Core;
using System.Windows;
using FlightTicketSell.Models.SearchRelated;
using FlightTicketSell.Helpers;

namespace FlightTicketSell.ViewModels
{
    public class SellViewModel : BaseViewModel
    {
        public string Title { get; } = "BÁN VÉ";
        #region Commands

        /// <summary>
        /// The command to continue
        /// </summary>
        public ICommand ContinueCommand { get; set; }

        public ICommand ChangeTicketCommand { get; set; }

        public ICommand LoadCommand { get; set; }
        public ICommand SanBayDoi { get; set; }

        public ICommand ReturnCommand { get; set; }

        //public ICommand Open_Window_DescriptionTicketFlight_Command { get; set; }

        public ObservableCollection<Airport_Search> Airports { get; set; }
        public ObservableCollection<Locations_List> Locations { get; set; }
        public ObservableCollection<FlightInfo> Flights { get; set; }

        public Airport_Search SanBayDii { get; set; }
        public Airport_Search SanBayDenn { get; set; }

        public Locations_List DiemDii { get; set; }
        public Locations_List DiemDenn { get; set; }

        public int MaChuyenBay { get; set; }

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

        #endregion


        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public SellViewModel()
        {
            // Create commands
            ContinueCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.TicketInfoFilling);

            ChangeTicketCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.ChangeTicket);

            LoadCommand = new RelayCommand<object>((p) => true, (p) =>
            {
            using (var context = new FlightTicketSellEntities())
            {
                try
                {
                    //System.DateTime? selectedDate = searchView.dp.SelectedDate.Value;
                    // Airports
                    Airports = new ObservableCollection<Airport_Search>(context.SANBAYs.Select(s => new Airport_Search { ID = s.MaSanBay, Name = s.TenSanBay }).ToList());

                    Locations = new ObservableCollection<Locations_List>(context.SANBAYs.Select(s => new Locations_List { ID = s.MaSanBay, Name = s.TinhThanh }).ToList());


                    }
                    catch (EntityException e)
                    {
                        NotifyHelper.ShowEntityException(e);
                        
                    }
                }
            });

            //SanBayDoi = new RelayCommand<object>((p) => true, (p) =>
            //{
            //    using (var context = new FlightTicketSellEntities())
            //    {
            //        try
            //        {
            //            if (MaChuyenBay > 0)
            //            {
            //                Flights = new ObservableCollection<FlightInfo>(
            //                                               context.GetFlightData_updated().Where(result =>
            //                                                   result.MaChuyenBay == MaChuyenBay)
            //                                                   .Select(result => new FlightInfo
            //                                                   {
            //                                                       MaChuyenBay = result.MaChuyenBay,
            //                                                       SanBayDi = result.SanBayDi,
            //                                                       SanBayDen = result.SanBayDen,
            //                                                       NgayGio = result.NgayGio,
            //                                                       SoDiemDung = result.SoDiemDung,
            //                                                       SoHangVe = result.SoLuongVe,
            //                                                       GiaVe = result.GiaVe,
            //                                                       GheTrong = result.GheTrong
            //                                                   }).ToList()
            //                                               );
            //            }

            //            //System.DateTime? selectedDate = searchView.dp.SelectedDate.Value;  && DateOfEntry != null  && result.NgayGio == DateOfEntry.Value DiemDii.Name && result.DiemDen == DiemDenn.Name
            //            if (DiemDii != null && DiemDenn != null && SanBayDii != null && SanBayDenn != null && DateOfEntry != null)
            //            {
            //                Flights = new ObservableCollection<FlightInfo>(
            //                                               context.GetFlightData_updated().Where(result =>
            //                                                   result.DiemDi == DiemDii.Name && result.DiemDen == DiemDenn.Name && result.NgayGio == DateOfEntry.Value && result.SanBayDi == SanBayDii.Name && result.SanBayDen == SanBayDenn.Name)
            //                                                   .Select(result => new FlightInfo
            //                                                   {
            //                                                       MaChuyenBay = result.MaChuyenBay,
            //                                                       SanBayDi = result.SanBayDi,
            //                                                       SanBayDen = result.SanBayDen,
            //                                                       NgayGio = result.NgayGio,
            //                                                       SoDiemDung = result.SoDiemDung,
            //                                                       SoHangVe = result.SoLuongVe,
            //                                                       GiaVe = result.GiaVe,
            //                                                       GheTrong = result.GheTrong
            //                                                   }).ToList()
            //                                               );
            //            }
            //        }
            //        catch (EntityException)
            //        {
            //            // TODO: messagebox vo
            //            return;
            //        }
            //    }
            //});


        }

        #endregion

    }
}
