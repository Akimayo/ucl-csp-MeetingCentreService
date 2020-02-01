using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace MeetingCentreService.Models
{
    /// <summary>
    /// Converts between boolean and a Segoe font
    /// </summary>
    class BooleanIconFontConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Icon font when true
            if (value is bool) return (bool)value ? "Segoe MDL2 Assets" : "Segoe UI";
            else throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Just a formality, never used
            if(value is string)
            {
                switch(value as string)
                {
                    case "Segoe MDL2 Assets":
                        return true;
                    case "Segoe UI":
                    case null:
                        return false;
                    default:
                        throw new NotImplementedException();
                }
            }
            else throw new NotImplementedException();
        }
    }
}
