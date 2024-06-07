using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PoissonDisc2UI.Utilities
{
    /// <summary>
    /// Converter for converting <see cref="int"/> to <see cref="string"/> and vice versa.
    /// </summary>
    public class IntStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString() ?? "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return int.TryParse((string)value, out int ret) ? ret : 0;
        }
    }
}
