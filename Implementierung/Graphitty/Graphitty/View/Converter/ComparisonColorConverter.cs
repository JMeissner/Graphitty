using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;

namespace Graphitty.View.Converter
{
    /// <summary>
    /// Used in the CorrelationView to compare the threshold with a correlation-coefficient.
    /// It returns a red SolidColorBrush if coefficient >= threshold.
    /// </summary>
    class ComparisonColorConverter : IMultiValueConverter
    {
        #region Public Methods

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Count() == 2 && values[1] is double && values[0] is double)
            {
                double coefficient = (double)values[0];
                double threshold = (double)values[1];
                if (Math.Abs(coefficient) >= Math.Abs(threshold)) return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#78e08f"));
            }
            return new SolidColorBrush(Colors.Transparent);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods
    }
}