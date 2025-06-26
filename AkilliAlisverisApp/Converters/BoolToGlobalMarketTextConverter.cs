using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace AkilliAlisverisApp.Converters
{
    public class BoolToGlobalMarketTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isGlobal && isGlobal)
            {
                return "(Global)"; // veya başka bir gösterge
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}