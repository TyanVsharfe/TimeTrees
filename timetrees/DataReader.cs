using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
namespace timetrees
{
    public class DataReader
    {
        const  int personIdIndex           = 0;
        const  int personNameIndex         = 1;
        const  int personBirthIndex        = 2;
        const  int personDeathIndex        = 3;
        public const int personOneParent   = 4;
        public const int personTwoParent   = 5;

        public const int timelineDateIndex = 0;
        public const int timelineDescriptionIndex = 1;

        public static List<TimelineEvent> ReadListTimelineData()
        {
            string path = "..\\..\\..\\..\\timeline.csv";
            string[][] data = ReadData(path);
            List<TimelineEvent> timelineEvent = new List<TimelineEvent>();
            for (int i = 0; i < data.Length; i++)
            {
                var parts = data[i];
                TimelineEvent timeEvent = new TimelineEvent();
                timeEvent.time = DateTime.Parse(parts[timelineDateIndex]);
                timeEvent.description = parts[timelineDescriptionIndex];
                timelineEvent.Add(timeEvent);
            }
            return timelineEvent;
        }

        public static List<Person> ReadListPersons()
        {
            string[][] data = ReadData("..\\..\\..\\..\\people.csv");
            List<Person> people = new List<Person>();
            for (int i = 0; i < data.Length; i++)
            {
                var parts = data[i];
                Person person = new Person();
                person.id = int.Parse(parts[personIdIndex]);
                person.name = parts[personNameIndex];
                person.birth = DateTime.Parse(parts[personBirthIndex]);
                if ((parts.Length >= 4) && (parts[personDeathIndex] != ""))
                {
                    person.death = DateTime.Parse(parts[personDeathIndex]);
                }
                else person.death = null;
                if ((parts.Length >= 5) && (parts[personOneParent] != ""))
                {
                    person.parentFirst = int.Parse(parts[personOneParent]);
                }
                else person.parentFirst = null;
                if ((parts.Length == 6) && (parts[personTwoParent] != ""))
                {
                    person.parentSecond = int.Parse(parts[personTwoParent]);
                }
                else person.parentSecond = null;
                people.Add(person);
            }
            return people;
        }

        static string[][] ReadData(string path)
        {
            string[] data = File.ReadAllLines(path);
            string[][] splitData = new string[data.Length][];
            for (int i = 0; i < data.Length; i++)
            {
                var line = data[i]; // "1; Имя; 2000-06-06
                string[] parts = line.Split(";"); //["1", "Имя 1", "2000-06-06"]
                splitData[i] = parts;
            }
            return splitData;
        }

        static List<Person> ReadDataFromJson(string path)
        {
            string json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<List<Person>>(json);
        }
    }
}
