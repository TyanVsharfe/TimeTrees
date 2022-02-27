using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

namespace timetrees
{
    public class FIleReader
    {
        public const int personOneParent   = 4;
        public const int personTwoParent   = 5;

        public const int timelineDateIndex = 0;
        public const int timelineDescriptionIndex = 1;

        public static void ReadTimelineFromJson()
        {
            string json = File.ReadAllText(@"..\\..\\..\\..\\timeline.json");
            DataRepo.TimelineRepo = JsonConvert.DeserializeObject<List<TimelineEvent>>(json);
        }

        public static void ReadPeopleFromJson()
        {
            string json = File.ReadAllText(@"..\\..\\..\\..\\people.json");
            DataRepo.PeopleRepo = JsonConvert.DeserializeObject<List<Person>>(json);
        }
    }
}
