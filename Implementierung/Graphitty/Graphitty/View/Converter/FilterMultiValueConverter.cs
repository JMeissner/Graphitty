using System;
using System.Globalization;
using System.Windows.Data;

namespace Graphitty.View
{
    class FilterMultiValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType,
            object parameter, System.Globalization.CultureInfo culture)
        {
            if(values.Length == 2)
            {
                string str1 = values[0] as string;
                string str2 = values[1] as string;

                return string.Format("{0}:{1}", str1, str2);
            }
            string str3 = values[0] as string;
            return string.Format("{0}:compare", str3);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Cannot convert back");
        }
    }
}
