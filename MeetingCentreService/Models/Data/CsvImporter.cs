using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using MeetingCentreService.Models.Entities;

namespace MeetingCentreService.Models.Data
{
    public static class CsvImporter
    {
        public async Task<IList<MeetingCentre>> ReadFromFile(string filePath)
        {
            Dictionary<string, MeetingCentre> centres = new Dictionary<string, MeetingCentre>();
            using(FileStream fs = File.OpenRead(filePath))
            using(StreamReader sr = new StreamReader(fs))
            {
                string line;
                ReadContentType reading = ReadContentType.None;
                while((line = await sr.ReadLineAsync()) != null)
                {
                    if (line == "MEETING_CENTRES") reading = ReadContentType.Centres;
                    else if (line == "MEETING_ROOMS") reading = ReadContentType.Rooms;
                }
                if (reading == ReadContentType.None) throw new InvalidDataException("File doesn't use required format");
            }
        }

        private enum ReadContentType
        {
            None, Centres, Rooms
        }
    }
}
