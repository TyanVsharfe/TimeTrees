using System;
using System.IO;
using System.Globalization;
using Newtonsoft;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace timetrees
{
    class Program
    {  
        const int personIdIndex    = 0;
        const int personNameIndex  = 1;
        const int personBirthIndex = 2;
        const int personDeathIndex = 3;

        const int timelineDateIndex        = 0;
        const int timelineDescriptionIndex = 1;

        struct Person
        {
            public int      id;
            public string   name;
            public DateTime birth;
            public DateTime death;
        }

        struct TimelineEvent
        {
            public DateTime time;
            public string   description;
        }

        static void Main(string[] args)
        {
            string timeLineFile = "..\\..\\..\\..\\timeline.csv";
            string peopleFile   = "..\\..\\..\\..\\people.csv";
            string timeLineFileJson = "..\\..\\..\\..\\timeline.json";
            string peopleFileJson = "..\\..\\..\\..\\people.json";
            TimelineEvent[] timeLineData = ReadTimelineData(timeLineFile);
            Person[]        peopleData   = ReadPersonData(peopleFile);
            //Console.WriteLine(FindMinAndMaxDate(timeLineData));
            //Console.WriteLine("");
            (int years, int months, int days) = DeltaDate(timeLineData);
            Console.WriteLine($"ћежду макс и мин датами прошло: лет: {years}, мес€цев: {months} дней: {days}");
            Console.WriteLine("");
            Console.WriteLine("»мена людей, которые родились в високосный год и их возраст не более 20 лет: ");
            GetLeapYear(peopleData);
        }

        static string[][] ReadData(string path)
        {
            string[] data = File.ReadAllLines(path);
            string[][] splitData = new string[data.Length][];
            for (int i = 0; i < data.Length; i++)
            {
                var line = data[i]; // "1; »м€; 2000-06-06
                string[] parts = line.Split(";"); //["1", "»м€ 1", "2000-06-06"]
                splitData[i] = parts;
            }
            return splitData;
        }

        static Person[] ReadPersonData(string path)
        {
            string[][] data = ReadData(path);
            Person[] people = new Person[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                var parts = data[i];
                Person person = new Person();
                person.id = int.Parse(parts[personIdIndex]);
                person.name = parts[personNameIndex];
                person.birth = DateTime.Parse(parts[personBirthIndex]);
                if (parts.Length == 4)
                {
                    person.death = DateTime.Parse(parts[personDeathIndex]);
                }
                people[i] = person;
            }
            return people;
        }

        static TimelineEvent[] ReadTimelineData(string path)
        {
            string[][] data = ReadData(path);
            TimelineEvent[] timelineEvent = new TimelineEvent[data.Length];
            for (int i = 0; i<data.Length; i++)
            {
                var parts = data[i];
                TimelineEvent timeEvent = new TimelineEvent();
                timeEvent.time = DateTime.Parse(parts[timelineDateIndex]);
                timeEvent.description = parts[timelineDescriptionIndex];
                timelineEvent[i] = timeEvent;
            }
            return timelineEvent;
        }

        static (DateTime, DateTime) FindMinAndMaxDate(TimelineEvent[] timeline)
        {
            DateTime minDate = DateTime.MaxValue;
            DateTime maxDate = DateTime.MinValue;
            foreach (var timeDate in timeline)
            {
                DateTime date = timeDate.time;
                if (date < minDate) minDate = date;
                if (date > maxDate) maxDate = date;
            }
            return (maxDate, minDate);
        }

        static (int, int, int) DeltaDate(TimelineEvent[] timeline)
        {
            (DateTime maxDate, DateTime minDate) = FindMinAndMaxDate(timeline);
            TimeSpan delta = maxDate - minDate; 
            //Console.WriteLine(delta);
            DateTime diffdate = new DateTime() + delta;
            diffdate = diffdate.AddYears(-1);
            diffdate = diffdate.AddMonths(-1);
            diffdate = diffdate.AddDays(-3);
            return (diffdate.Year,diffdate.Month,diffdate.Day);
        }

        static void GetLeapYear(Person[] peopleData)
        {
            DateTime nowDate = DateTime.Now;
            foreach (var person in peopleData)
            {
                int age;
                DateTime birth = person.birth;
                DateTime deathDate = DateTime.MinValue;
                if (person.death == null) deathDate = DateTime.MinValue; else deathDate = person.death;
                if (deathDate == null)
                {
                    age = nowDate.Year - birth.Year;
                    if (DateTime.Now.DayOfYear < birth.DayOfYear) age++;   //на случай, если день рождени€ уже прошЄл
                }
                else
                {
                    age = deathDate.Year - birth.Year;
                    if (deathDate.DayOfYear < birth.DayOfYear) age++;   //на случай, если день рождени€ уже прошЄл
                }              
                if ((DateTime.IsLeapYear(birth.Year)) & (age < 20)) Console.WriteLine(person.name);
            }
        }
        static Person[] ReadPeopleFromJson(string path)
        {
            string json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<Person[]>(json);
        }

        static void WritePeopleFromJson(string path, Person[] people)
        {
            string json = JsonConvert.SerializeObject(people);
            File.WriteAllText(path,json);
        }
    }
}
