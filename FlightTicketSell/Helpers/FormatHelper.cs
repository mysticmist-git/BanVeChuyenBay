using System;
using System.Globalization;

namespace FlightTicketSell.Helpers
{
    public static class FormatHelper
    {
        /// <summary>
        /// Convert <see cref="DateTime"/> to application stanard format
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string DateFormat(DateTime date) => date.ToString("hh:mm dd:mm:yyyy", new CultureInfo("vi-VN"));

        /// <summary>
        /// Convert a decimal value to a vietnam currency format money
        /// </summary>
        /// <param name="money">The money needed to be converted</param>
        /// <returns></returns>
        public static string VietnamCurrencyFormat(decimal money) => string.Format(new CultureInfo("vi-VN"), "{0:#,##0.00}", money);

        /// <summary>
        /// Convert decimal to percent
        /// </summary>
        /// <param name="percent">The ratio</param>
        /// <returns>The percent</returns>
        public static string DecimalToPercentConverter(decimal percent) => string.Format("{0}%", Convert.ToInt32(percent * 100).ToString());
    }
}
