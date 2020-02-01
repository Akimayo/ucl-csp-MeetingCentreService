using MeetingCentreService.Models.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace MeetingCentreService.Models.Data
{
    /// <summary>
    /// Interface for MeetingCentreService XML documents
    /// </summary>
    public static class XmlIO
    {
        /// <summary cref="Entities.MeetingCentreService">
        /// Asynchronously loads a XML MeetingCentreService document
        /// </summary>
        /// <param name="loadFrom">Path of loaded XML file</param>
        /// <returns>Loaded MeetingCentreService</returns>
        internal static Task<Entities.MeetingCentreService> ParseXmlAsync(string loadFrom)
        {
            return Task.Run<Entities.MeetingCentreService>(() =>
            {
                XmlSerializer ser = new XmlSerializer(typeof(Entities.MeetingCentreService));
                using (StreamReader reader = new StreamReader(loadFrom))
                using (XmlReader xml = new XmlTextReader(reader))
                {
                    Entities.MeetingCentreService service = ser.Deserialize(xml) as Entities.MeetingCentreService;
                    // References to parent entities aren't being serialized, thereofre there being added afterwards
                    foreach (MeetingCentre centre in service.MeetingCentres)
                    {
                        foreach (MeetingRoom room in centre.MeetingRooms)
                        {
                            room.AssignMeetingCentre(centre);
                            foreach (KeyValuePair<string, ObservableCollection<MeetingReservation>> reservations in room.Reservations)
                            {
                                foreach (MeetingReservation reservation in reservations.Value)
                                {
                                    reservation.AssignMeetingRoom(room);
                                }
                            }
                        }
                    }
                    return service;
                }
            });
        }

        /// <summary>
        /// Asynchronously exports current MeetingCentreService to its save file path.
        /// </summary>
        /// <returns>Boolean indicating success</returns>
        internal static Task<bool> ExportXmlAsync()
        {
            return Task.Run<bool>(() =>
            {
                XmlSerializer ser = new XmlSerializer(typeof(Entities.MeetingCentreService));
                using (StreamWriter writer = new StreamWriter(Entities.MeetingCentreService.Current.FilePath))
                using (XmlWriter xml = new XmlTextWriter(writer))
                {
                    try
                    {
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
