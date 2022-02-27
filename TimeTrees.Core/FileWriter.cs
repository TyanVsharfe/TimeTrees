using System;
using System.IO;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace timetrees
{
    public class FileWriter
    {
        public static void DoWritePeople()
        {
            string[] data = File.ReadAllLines("..\\..\\..\\..\\people.csv");
            for (int i = 0; i < data.Length; i++)
            {
                Console.WriteLine(data[i]);
            }
            string dataJson = JsonConvert.SerializeObject(data);
            File.WriteAllText("..\\..\\..\\..\\people.json", dataJson);
        }

        public static void WriteTimelineToJson()
        {

            File.WriteAllText(@"..\\..\\..\\..\\timeline.json", JsonConvert.SerializeObject(DataRepo.TimelineRepo));
        }

        public static void WriteDataInJsonEventFile(TimelineEvent timeline)
        {
            string text = JsonConvert.SerializeObject(timeline, Formatting.Indented);
            File.WriteAllText(@"..\\..\\..\\..\\timeline.json", text);
        }

        public static void WritePeopleToJson()
        {
            File.WriteAllText(@"..\\..\\..\\..\\people.json", JsonConvert.SerializeObject(DataRepo.PeopleRepo));
        }

        public static void WriteToJson(TimelineEvent timeline)
        {
            File.WriteAllText(@"..\\..\\..\\..\\timeline.json", JsonConvert.SerializeObject(timeline));
        }
    }
}
