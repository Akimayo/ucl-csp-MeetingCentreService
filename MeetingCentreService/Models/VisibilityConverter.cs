using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace MeetingCentreService.Models
{
    class VisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Converts a boolean to the correspondig Visibility value
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool) return (bool)value ? Visibility.Visible : Visibility.Collapsed;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Converts a Visibility to the correspondig boolean
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility) return (Visibility)value == Visibility.Visible ? true : false;
            throw new NotImplementedException();
        }
    }
}
