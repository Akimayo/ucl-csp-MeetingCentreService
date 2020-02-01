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
        /// Converts VideoConference boolean to a corresponding Segoe MDL2 Assets Icon
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool) return (bool)value ? "\xE720" : "\xF781";
            else return "\xE894";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
