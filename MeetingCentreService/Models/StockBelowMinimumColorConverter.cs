using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace MeetingCentreService.Models
{
    /// <summary>
    /// Converts a boolean from Accessory to a Brush for display
    /// </summary>
    class StockBelowMinimumColorConverter : IValueConverter
    {
        private static readonly SolidColorBrush Red = new SolidColorBrush(Colors.Red);
        private static readonly SolidColorBrush Black = new SolidColorBrush(Colors.Black);
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool) return (bool)value ? Red : Black;
            else throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
