using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace MeetingCentreService.Models
{
    class CentreFormRoomTupleConverter : IValueConverter
    {
        /// <summary>
        /// Converts MeetingCentreForm's collection of changes in MeetingRooms collection to only the MeetingRooms
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null) return null;
            if (value is IEnumerable<(Entities.MeetingRoom, Entities.MeetingCentre.MeetingCentreForm.CollectionAction)>) 
                return (value as IEnumerable<(Entities.MeetingRoom, Entities.MeetingCentre.MeetingCentreForm.CollectionAction)>).Select(n => n.Item1);
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
