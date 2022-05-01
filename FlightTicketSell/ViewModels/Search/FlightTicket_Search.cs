using FlightTicketSell.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightTicketSell.ViewModels.Report;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Windows.Input;
using FlightTicketSell.Models.Enums;
using FlightTicketSell.Views.Helper;
using System.ComponentModel;
using System.Windows;
using System.Data.Entity.Core;

namespace FlightTicketSell.ViewModels.Search
{
    public class FlightTicket_Search : BaseViewModel
    {
        public string MaChuyenBay { get; set; }

        public string SanBayDi { get; set; }

        public string SanBayDen { get; set; }

        public string NgayGioBay { get; set; }

        public string ThoiGian { get; set; }

        //San bay trung gian
        public ObservableCollection<MidAirport_Search> MidAirport { get; set; }
        public ObservableCollection<TicketClass_Search> TicketClass { get; set; }

        public string ThoiGianBay { get; set; }

        //Command
        public ICommand LoadCommand { get; set; }
        public ICommand ReturnCommand { get; set; }
        public ICommand Open_Window_TickedSoldBooked_Command { get; set; }


        public string MaDuongBay { get; set; }

        public string MaHangVe { get; set; }

        public decimal GiaCoBan { get; set; }
        public FlightTicket_Search()
        {
            ReturnCommand = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.Search);
            Open_Window_TickedSoldBooked_Command = new RelayCommand<object>((p) => true, (p) => IoC.IoC.Get<ApplicationViewModel>().CurrentView = Models.AppView.TickedSoldBooked);

            LoadCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                using (var context = new FlightTicketSellEntities())
                {
                    try
                    {
                        ThoiGianBay = context.CHUYENBAYs.Where(result => result.MaChuyenBay.ToString() == MaChuyenBay).FirstOrDefault().DUONGBAY.ThoiGianBay.ToString();

                        MaDuongBay = context.CHUYENBAYs.Where(result => result.MaChuyenBay.ToString() == MaChuyenBay).FirstOrDefault().MaDuongBay.ToString();

                        //context.CHITIETHANGVEs.Where(result => result.MaChuyenBay.ToString() == MaChuyenBay).FirstOrDefault().MaHangVe.ToString();

                        MidAirport = new ObservableCollection<MidAirport_Search>(
                                                                context.SANBAYTGs.Where(result =>
                                                                result.MaDuongBay.ToString() == MaDuongBay)
                                                                .Select(result => new MidAirport_Search
                                                                {
                                                                    ThuTu = result.ThuTu.ToString(),
                                                                    TenSanBay = context.SANBAYs.Where(x => x.MaSanBay == result.MaSanBay).FirstOrDefault().TenSanBay,
                                                                    GhiChu = result.GhiChu.ToString(),
                                                                    ThoiGianDung = result.ThoiGianDung.ToString()
                                                                }).ToList()
                                                           );
                        TicketClass = new ObservableCollection<TicketClass_Search>(
                                                                context.CHUYENBAYs.Where(result =>
                                                                result.MaChuyenBay.ToString() == MaChuyenBay).FirstOrDefault()
                                                                .CHITIETHANGVEs.Select(cthv => new TicketClass_Search
                                                                {
                                                                    TenHangVe = cthv.HANGVE.TenHangVe,
                                                                    GheTrong = (cthv.SoGhe -
                                                                    context.VEs.Where(ve => ve.MaChuyenBay.ToString() == MaChuyenBay && ve.HANGVE.MaHangVe == cthv.HANGVE.MaHangVe).Count() -
                                                                    (context.DATCHOes.Where(dc =>
                                                                    dc.MaChuyenBay.ToString() == MaChuyenBay &&
                                                                    dc.HANGVE.MaHangVe == cthv.HANGVE.MaHangVe).Sum(dcc => (int?)dcc.SoVeDat) ?? 0)).ToString(),
                                                                    GiaTien = ((cthv.HANGVE.HeSo) * GiaCoBan).ToString()
                                                                }).ToList()
                            );


                    }
                    catch
                    {

                    }
                }
            });

        }
    }
}
