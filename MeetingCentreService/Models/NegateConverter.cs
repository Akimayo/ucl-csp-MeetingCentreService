using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace MeetingCentreService.Models
{
    /// <summary>
    /// Neagates a boolean
    /// </summary>
    class NegateConverter : IValueConverter
    {
        /// <summary>
        /// Simply negates the boolean value
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool) return !(bool)value;
            throw new NotImplementedException();
        }
        /// <summary>
        /// Simply negates the boolean value
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool) return !(bool)value;
            throw new NotImplementedException();
        }
    }
}
