using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace BanVeChuyenBay.ValueConverter
{
    public class IsIntEqualEnumConverter : BaseValueConverter<IsIntEqualEnumConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null || value == null) return false;

            if (parameter.GetType().IsEnum && value is int)
            {
                return (int)parameter == (int)value;
            }
            return false;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
