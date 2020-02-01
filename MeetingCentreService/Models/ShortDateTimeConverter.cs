using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace MeetingCentreService.Models
{
    /// <summary>
    /// Converts between DateTime or TimeSpan and string
    /// </summary>
    class ShortDateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime) return ((DateTime)value).ToShortDateString();
            else if (value is TimeSpan) return new DateTime().Add((TimeSpan)value).ToShortTimeString();
            else throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string) return DateTime.Parse(value as string);
            else throw new NotImplementedException();
        }
    }
}
