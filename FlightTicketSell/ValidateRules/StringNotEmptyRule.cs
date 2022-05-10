using System.Globalization;
using System.Windows.Controls;

namespace FlightTicketSell.ValidateRules
{
    public class StringNotEmptyRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (string.IsNullOrEmpty(value as string))
                return new ValidationResult(false, "Bắt buộc");

            return ValidationResult.ValidResult;
        }
    }
}
