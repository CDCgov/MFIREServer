using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace MFireServer
{
    [ValueConversion(typeof(int), typeof(string))]
    internal class SimTimestepConverter : MarkupExtension, IValueConverter
    {
        private static SimTimestepConverter _converter = new SimTimestepConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var intVal = (int)value;

            return string.Format("{0}", intVal);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var stringVal = (string)value;

            if (int.TryParse(stringVal, out var result))
            {
                return result;
            }

            return 10;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter; 
        }
    }
}
