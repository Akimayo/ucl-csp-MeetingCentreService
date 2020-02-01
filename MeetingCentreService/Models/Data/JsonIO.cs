using MeetingCentreService.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Collections.ObjectModel;

namespace MeetingCentreService.Models.Data
{
    /// <summary>
    /// Interface for MeetingCentreService JSON documents
    /// </summary>
    public static class JsonIO
    {
        /// <summary cref="Entities.MeetingCentreService">
        /// Asynchronously loads a JSON as a MeetingCentreService
        /// </summary>
        /// <param name="loadFrom">Path of loaded JSON file</param>
        /// <returns>Loaded MeetingCentreService</returns>
        internal static Task<Entities.MeetingCentreService> ParseJsonAsync(string loadFrom)
        {
            return Task.Run<Entities.MeetingCentreService>(() =>
            {
                JsonSerializer serializer = new JsonSerializer();
                using (StreamReader reader = new StreamReader(loadFrom))
                using (JsonReader json = new JsonTextReader(reader))
                {
                    Entities.MeetingCentreService service = serializer.Deserialize<Entities.MeetingCentreService>(json);
                    // References to parent entities aren't being serialized, thereofre there being added afterwards
                    foreach (MeetingCentre centre in service.MeetingCentres)
                    {
                        foreach (MeetingRoom room in centre.MeetingRooms)
                        {
                            room.AssignMeetingCentre(centre);
                            foreach(KeyValuePair<string, ObservableCollection<MeetingReservation>> reservations in room.Reservations)
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
        internal static Task<bool> ExportJsonAsync()
        {
            return Task.Run<bool>(() =>
            {
                JsonSerializer serializer = new JsonSerializer();
                using (StreamWriter writer = new StreamWriter(Entities.MeetingCentreService.Current.FilePath))
                using (JsonWriter json = new JsonTextWriter(writer))
                {
                    try
                    {
                        serializer.Serialize(json, Entities.MeetingCentreService.Current);
                        return true;
                    }
                    catch (JsonSerializationException)
                    {
                        return false;
                    }
                }
            });
        }
    }
}
