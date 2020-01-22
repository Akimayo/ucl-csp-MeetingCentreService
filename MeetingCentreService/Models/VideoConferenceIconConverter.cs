using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace MeetingCentreService.Models
{
    class VideoConferenceIconConverter : IValueConverter
    {
        /// <summary>
        /// Converts a boolean value to an icon for VideoConference
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool) return (bool)value ? "\xE720" : "\xF781";
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
