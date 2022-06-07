using FlightTicketSell.Models;
using FlightTicketSell.ViewModels.Setting;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FlightTicketSell.ViewModels.Schedule
{
    public class ImportFromExcelClass : BaseViewModel
    {

        #region Private Members

        #endregion

        #region Public Properties

        public int Stt { get; set; }
        public Airport SanBayDi { set; get; }
        public Airport SanBayDen { set; get; }
        public DateTime NgayBay { set; get; }
        public DateTime GioBay { set; get; }
        public int GiaVe { set; get; }
        public int ThoiGianBay { set; get; }
        public List<TicketClassDetail> HangVe { set; get; }
        public List<LayoverAirport> SanBayTG { set; get; }

        public ImportFromExcelClass() { }
        public static bool IsSucces { get; set; }
        public ImportFromExcelClass(string stt, string masanBayDi, string masanBayDen, string ngayBay, string gioBay, string giaVe, string thoiGianBay, string hangVe, string sanBayTG)
        {
            IsSucces = true;
            Stt = Stt;
            SanBayDi = GetAirportFromCode(stt, "di", masanBayDi);
            if (SanBayDi == null)
                IsSucces = false;

            SanBayDen = GetAirportFromCode(stt, "den", masanBayDen);
            if (SanBayDen == null)
                IsSucces = false;

            NgayBay = GetDateFromString(stt, ngayBay);
            if (NgayBay.Day == -1)
                IsSucces = false;

            GioBay = GetTimeFromString(stt, gioBay);
            if (GioBay.Hour == -1)
                IsSucces = false;

            if (!int.TryParse(giaVe, out int gv))
                IsSucces = false;
            GiaVe = gv;

            if (!int.TryParse(thoiGianBay, out int tgb))
                IsSucces = false;
            ThoiGianBay = tgb;

            HangVe = GetListTicketClassFromListString(stt, GetListFromString_Ticket(stt, hangVe));
            if (HangVe == null)
                IsSucces = false;

            SanBayTG = GetListLayoverAirportFromListString(stt, GetListFromString_Airport(stt, sanBayTG));
            if (SanBayTG == null)
                IsSucces = false;

        }

        #endregion

        #region Methods
        private Airport GetAirportFromCode(string stt, string den_di, string masanBay)
        {
            using (var context = new FlightTicketSellEntities())
            {
                try
                {
                    var airport = context.SANBAYs.Where(h => h.VietTat == masanBay).FirstOrDefault();
                    if (airport == null)
                    {
                        Airport airport1 = new Airport
                        {
                            Id = airport.MaSanBay,
                            Code = airport.VietTat,
                            Name = airport.TenSanBay,
                            Province = airport.TinhThanh,
                            Status = airport.TrangThai
                        };
                        return airport1;
                    }
                    else
                    {
                        if (den_di == "den")
                        {
                            MessageBox.Show("Chuyến bay thứ " + stt + " có [Mã sân bay đến] không hợp lệ.", "Cảnh báo");
                        }
                        else
                        {
                            MessageBox.Show("Chuyến bay thứ " + stt + " có [Mã sân bay đi] không hợp lệ.", "Cảnh báo");
                        }
                        return null;
                    }

                }
                catch (EntityException e)
                {
                    MessageBox.Show("Database access failed!", string.Format($"Exception: {e.Message}"), MessageBoxButton.OK, MessageBoxImage.Error);
                }
                return null;
            }
        }
        private DateTime GetDateFromString(string stt, string ngaybay)
        {
            int pos = 1;
            string ngay = "";
            string thang = "";
            string nam = "";
            foreach (var item in ngaybay)
            {
                if (item != '/')
                {
                    if (pos == 1)
                    {
                        ngay += item;
                    }
                    else if (pos == 2)
                    {
                        thang += item;
                    }
                    else
                    {
                        nam += item;
                    }
                }
                else
                {
                    pos++;
                }
            }
            if (!int.TryParse(ngay, out _) || !int.TryParse(thang, out _) || !int.TryParse(nam, out _))
            {
                MessageBox.Show("Chuyến bay thứ " + stt + " có [Ngày bay] không hợp lệ.", "Cảnh báo");
                return new DateTime(-1, -1, -1);
            }
            return new DateTime(int.Parse(nam), int.Parse(thang), int.Parse(ngay));
        }
        private DateTime GetTimeFromString(string stt, string gioBay)
        {
            int pos = 1;
            string gio = "";
            string phut = "";
            foreach (var item in gioBay)
            {
                if (item != ':')
                {
                    if (pos == 1)
                    {
                        gio += item;
                    }
                    else
                    {
                        phut += item;
                    }
                }
                else
                {
                    pos++;
                }
            }
            if (!int.TryParse(gio, out _) || !int.TryParse(phut, out _))
            {
                MessageBox.Show("Chuyến bay thứ " + stt + " có [Giờ bay] không hợp lệ.", "Cảnh báo");
                return new DateTime(-1, -1, -1, -1, -1, -1);
            }
            return new DateTime(0, 0, 0, int.Parse(gio), int.Parse(phut), 0);
        }
        private List<List<string>> GetListFromString_Ticket(string stt, string chuoi)
        {
            List<List<string>> list = new List<List<string>>();
            List<string> Ten = new List<string>();
            List<string> So = new List<string>();
            string ten = "";
            string so = "";
            string ten_so = "ten";
            foreach (var item in chuoi)
            {
                if (item == '-')
                {
                    So.Add(so);
                    so = "";
                    ten_so = "ten";
                }
                else if (item == '_')
                {
                    Ten.Add(ten);
                    ten = "";
                    ten_so = "so";
                }
                else
                {
                    if (ten_so == "ten")
                        ten += item;
                    else
                        so += item;
                }
            }
            if (Ten.Any(h => string.IsNullOrEmpty(h)) || So.Any(h => string.IsNullOrEmpty(h)) || Ten.Count != So.Count)
            {
                MessageBox.Show("Chuyến bay thứ " + stt + " có [Hạng vé] không hợp lệ.", "Cảnh báo");
                list.Add(null);
                return list;
            }
            list.Add(Ten);
            list.Add(So);
            return list;
        }
        private List<List<string>> GetListFromString_Airport(string stt, string chuoi)
        {
            List<List<string>> list = new List<List<string>>();
            List<string> Ma = new List<string>();
            List<string> So = new List<string>();
            List<string> Ghi = new List<string>();
            string ma = "";
            string so = "";
            string ghi = "";
            string ma_so_ghi = "ten";
            foreach (var item in chuoi)
            {
                if (item == '-')
                {
                    Ghi.Add(ghi);
                    ghi = "";
                    ma_so_ghi = "ma";
                }
                else if (item == '_')
                {
                    Ma.Add(ma);
                    ma = "";
                    ma_so_ghi = "so";
                }
                else if (item == ':')
                {
                    So.Add(so);
                    so = "";
                    ma_so_ghi = "ghi";
                }
                else
                {
                    if (ma_so_ghi == "ma")
                        ma += item;
                    else if (ma_so_ghi == "so")
                        so += item;
                    else
                        ghi += item;
                }
            }
            if (Ma.Any(h => string.IsNullOrEmpty(h)) || So.Any(h => string.IsNullOrEmpty(h))
                || Ma.Count != So.Count)
            {
                MessageBox.Show("Chuyến bay thứ " + stt + " có [Sân bay trung gian] không hợp lệ.", "Cảnh báo");
                list.Add(null);
                return list;
            }
            list.Add(Ma);
            list.Add(So);
            list.Add(Ghi);
            return list;
        }
        private List<TicketClassDetail> GetListTicketClassFromListString(string stt, List<List<string>> hangve)
        {
            List<TicketClassDetail> list = new List<TicketClassDetail>();
            using (var context = new FlightTicketSellEntities())
            {
                try
                {
                    for (int i = 0; i < hangve[0].Count; i++)
                    {
                        var hangvelayduoc = context.HANGVEs.Where(h => h.TenHangVe == hangve[0][i]).FirstOrDefault();
                        if (hangvelayduoc == null)
                        {
                            MessageBox.Show("Chuyến bay thứ " + stt + " có [Hạng vé] chứa hạng vé không tồn tại.", "Cảnh báo");
                            return null;
                        }
                        if (!hangvelayduoc.TrangThai)
                        {
                            MessageBox.Show("Chuyến bay thứ " + stt + " có [Hạng vé] chứa hạng vé ngừng cung cấp.", "Cảnh báo");
                            return null;
                        }
                        if (!int.TryParse(hangve[1][i], out int soghe))
                        {
                            MessageBox.Show("Chuyến bay thứ " + stt + " có [Hạng vé] có số ghế không hợp lệ.", "Cảnh báo");
                            return null;

                        }
                        TicketClassDetail ticketClassDetail = new TicketClassDetail()
                        {
                            Id_TicketClass = hangvelayduoc.MaHangVe,
                            Seats = int.Parse(hangve[1][i]),
                            TicketClassName = hangvelayduoc.TenHangVe,
                            TicketClassCoefficient = (double)hangvelayduoc.HeSo
                        };
                        list.Add(ticketClassDetail);
                    }
                    return list;
                }
                catch (EntityException e)
                {
                    MessageBox.Show("Database access failed!", string.Format($"Exception: {e.Message}"), MessageBoxButton.OK, MessageBoxImage.Error);
                }
                return null;
            }
        }
        private List<LayoverAirport> GetListLayoverAirportFromListString(string stt, List<List<string>> sanbaytg)
        {
            List<LayoverAirport> list = new List<LayoverAirport>();
            using (var context = new FlightTicketSellEntities())
            {
                try
                {
                    for (int i = 0; i < sanbaytg[0].Count; i++)
                    {
                        var sanbaylayduoc = context.SANBAYs.Where(h => h.VietTat == sanbaytg[0][i]).FirstOrDefault();
                        if (sanbaylayduoc == null)
                        {
                            MessageBox.Show("Chuyến bay thứ " + stt + " có [Sân bay trung gian] chứa sân bay không tồn tại.", "Cảnh báo");
                            return null;
                        }
                        if (sanbaylayduoc.MaSanBay == SanBayDi.Id)
                        {
                            MessageBox.Show("Chuyến bay thứ " + stt + " có [Sân bay trung gian] trùng [Sân bay đi].", "Cảnh báo");
                            return null;
                        }
                        if (sanbaylayduoc.MaSanBay == SanBayDen.Id)
                        {
                            MessageBox.Show("Chuyến bay thứ " + stt + " có [Sân bay trung gian] trùng [Sân bay đến].", "Cảnh báo");
                            return null;
                        }
                        if (!sanbaylayduoc.TrangThai)
                        {
                            MessageBox.Show("Chuyến bay thứ " + stt + " có [Sân bay trung gian] chứa sân bay ngừng hoạtd động.", "Cảnh báo");
                            return null;
                        }
                        if (!int.TryParse(sanbaytg[1][i], out int soghe))
                        {
                            MessageBox.Show("Chuyến bay thứ " + stt + " có [Sân bay trung gian] có thời gian dừng không hợp lệ.", "Cảnh báo");
                            return null;

                        }
                        LayoverAirport layoverAirport = new LayoverAirport()
                        {
                            Id_Airport = sanbaylayduoc.MaSanBay,
                            AirportName = sanbaylayduoc.TenSanBay,
                            Order = i + 1,
                            StopTime = int.Parse(sanbaytg[1][i]),
                            Note = sanbaytg[2][i]
                        };
                        list.Add(layoverAirport);
                    }
                    return list;
                }
                catch (EntityException e)
                {
                    MessageBox.Show("Database access failed!", string.Format($"Exception: {e.Message}"), MessageBoxButton.OK, MessageBoxImage.Error);
                }
                return null;
            }
        }
        #endregion
    }
}
