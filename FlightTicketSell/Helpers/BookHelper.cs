using FlightTicketSell.Models.Enums;

namespace FlightTicketSell.Helpers
{
    public static class BookHelper
    {
        /// <summary>
        /// Convert a state frorm string to an enum and back
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static string BookingStateToString(BookingState state)
        {
            switch (state)
            {
                case BookingState.NotChanged:
                    return "ChuaDoi";
                case BookingState.Changed:
                    return "DaDoi";
                case BookingState.Cancel:
                    return "DaHuy";
                default:
                    return null;
            }
        }

        private static readonly string[] VietnameseSigns = new string[]
        {

            "aAeEoOuUiIdDyY",

            "áàạảãâấầậẩẫăắằặẳẵ",

            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",

            "éèẹẻẽêếềệểễ",

            "ÉÈẸẺẼÊẾỀỆỂỄ",

            "óòọỏõôốồộổỗơớờợởỡ",

            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",

            "úùụủũưứừựửữ",

            "ÚÙỤỦŨƯỨỪỰỬỮ",

            "íìịỉĩ",

            "ÍÌỊỈĨ",

            "đ",

            "Đ",

            "ýỳỵỷỹ",

            "ÝỲỴỶỸ"
        };

        public static string convertText(string str)
        {
            for (int i = 1; i < VietnameseSigns.Length; i++)
            {
                for (int j = 0; j < VietnameseSigns[i].Length; j++)
                    str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
            }
            return str;
        }
    }
}
