using BanVeChuyenBay.ViewModels;
using System;
using System.Globalization;
using BanVeChuyenBay.IoC;
using System.Diagnostics;

namespace BanVeChuyenBay.ValueConverter
{
    public class IoCConverter : BaseValueConverter<IoCConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Find approriate value
            switch ((string)value)
            {
                case nameof(ApplicationViewModel):
                    return IoC.IoC.Get<ApplicationViewModel>();

                default:
                    Debugger.Break();
                    return null;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
