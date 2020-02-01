using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace MeetingCentreService.Models
{
    class BooleanIconFontConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool) return (bool)value ? "Segoe MDL2 Assets" : "Segoe UI";
            else throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
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
