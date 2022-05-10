using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace FlightTicketSell.ValidateRules
{
    public class MailFormatRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is null)
                return null;
            
            Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");

            if (!regex.IsMatch(value as string))
            {
                return new ValidationResult(false, "Định dạng mail không hợp lệ!");
            }

            return ValidationResult.ValidResult;
        }
    }
}
