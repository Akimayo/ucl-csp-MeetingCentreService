using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace MeetingCentreService.Models
{
    class CentreFormConverter : IValueConverter
    {
        /// <summary>
        /// Converts a MeetingCentreForm to a saved MeetingCentre
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null) return null;
            if (value is Entities.MeetingCentre.MeetingCentreForm) return ((Entities.MeetingCentre.MeetingCentreForm)value).Save();
            throw new NotImplementedException();
        }
        /// <summary>
        /// Converts a MeetingCentre to it's MeetingCentreForm
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null) return Entities.MeetingCentre.MeetingCentreForm.Empty;
            if (value is Entities.MeetingCentre) return (value as Entities.MeetingCentre).GetEditForm();
            throw new NotImplementedException();
        }
    }
}
