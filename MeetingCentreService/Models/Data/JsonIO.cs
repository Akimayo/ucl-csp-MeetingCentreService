using MeetingCentreService.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace MeetingCentreService.Models.Data
{
    public static class JsonIO
    {
        internal static Task<Entities.MeetingCentreService> ParseJsonAsync(string loadFrom)
        {
            return Task.Run<Entities.MeetingCentreService>(() =>
            {
                JsonSerializer serializer = new JsonSerializer();
                using (StreamReader reader = new StreamReader(loadFrom))
                using (JsonReader json = new JsonTextReader(reader))
                {
                    return serializer.Deserialize<Entities.MeetingCentreService>(json);
                }
            });
        }

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
