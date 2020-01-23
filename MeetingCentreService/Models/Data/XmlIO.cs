using MeetingCentreService.Models.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace MeetingCentreService.Models.Data
{
    public static class XmlIO
    {
        internal static Task<Entities.MeetingCentreService> ParseXmlAsync(string loadFrom)
        {
            return Task.Run<Entities.MeetingCentreService>(() =>
            {
                using (StreamReader reader = new StreamReader(loadFrom))
                using (XmlReader xml = new XmlTextReader(reader))
                {
                    XmlSerializer ser = new XmlSerializer(typeof(Entities.MeetingCentreService));
                    return ser.Deserialize(xml) as Entities.MeetingCentreService;
                }
            });
        }

        internal static Task<bool> ExportXmlAsync()
        {
            return Task.Run<bool>(() =>
            {
                using (StreamWriter writer = new StreamWriter(Entities.MeetingCentreService.Current.FilePath))
                using (XmlWriter xml = new XmlTextWriter(writer))
                {
                    try
                    {
                        XmlSerializer ser = new XmlSerializer(typeof(Entities.MeetingCentreService));
                        ser.Serialize(xml, Entities.MeetingCentreService.Current);
                        return true;
                    }
                    catch (XmlException)
                    {
                        return false;
                    }
                }
            });
        }
    }
}
