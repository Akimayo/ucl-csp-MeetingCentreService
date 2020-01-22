using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeetingCentreService.Models.Entities;

namespace MeetingCentreService.Models.Data
{
    /// <summary>
    /// Parser for legacy CSV-style save files
    /// </summary>
    public static class CsvImporter
    {
        /// <summary>
        /// Parses legacy CSV-style save file to object model
        /// </summary>
        /// <param name="filePath">Path of file to be inported</param>
        /// <returns>IList collection of filled MeetingCentre objects</returns>
        public static async Task<IList<MeetingCentre>> ReadFromFileAsync(string filePath)
        {
            Dictionary<string, MeetingCentre> centres = new Dictionary<string, MeetingCentre>();
            using(StreamReader sr = new StreamReader(filePath, Encoding.UTF8))
            {
                ReadContentType reading = ReadContentType.None;
                string line;
                string[] data;
                while((line = await sr.ReadLineAsync()) != null)
                {
                    data = line.Split(",");
                    if (data[0] == "MEETING_CENTRES") reading = ReadContentType.Centres; // Start creating MeetingCentre objects
                    else if (data[0] == "MEETING_ROOMS") reading = ReadContentType.Rooms; // Start creating MeetingRoom objects
                    else if (reading == ReadContentType.Centres)
                    {
                        centres.Add(data[1], new MeetingCentre() { Name = data[0], Code = data[1], Description = data[2] });
                    }
                    else if (reading == ReadContentType.Rooms)
                    {
                        bool? video = null;
                        if (data[4] == "NO") video = false;
                        else if (data[4] == "YES") video = true;
                        centres[data[5]].MeetingRooms.Add(new MeetingRoom(centres[data[5]]) { Name = data[0], Code = data[1], Description = data[2], Capacity = int.Parse(data[3]), VideoConference = (bool)video });
                    }
                    else reading = ReadContentType.None;
                }
                // In case the reading variable hasn't been changed, the file format is invalid and cannot be parsed
                if (reading == ReadContentType.None) throw new InvalidDataException("File doesn't use required format");
            }
            return centres.Values.ToList(); // Extract list from helper dictionary
        }

        /// <summary>
        /// Helper enum for file parsing
        /// </summary>
        /// <remarks>
        /// This enum helps the parser tell which object it should create.
        /// </remarks>
        private enum ReadContentType
        {
            None, Centres, Rooms
        }
    }
}
