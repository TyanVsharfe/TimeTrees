using System;
using System.IO;
using System.Globalization;

namespace timetrees
{
    class Program
    {
        static void Main(string[] args)
        {
            string timeLineFile = "..\\..\\..\\..\\timeline.csv";
            string peopleFile = "..\\..\\..\\..\\people.csv";

            string[] lines = File.ReadAllLines(timeLineFile);
            foreach (var line in lines)
            {
                Console.WriteLine(line);
            }
            Console.WriteLine("");

            string[] liness = File.ReadAllLines(peopleFile);
            foreach (var line in liness)
            {
                Console.WriteLine(line);
            }
            Console.WriteLine("");

            string[][] timeLineData = ReadData(timeLineFile);
            string[][] peopleData = ReadData(peopleFile);
            Console.WriteLine(FindMinAndMaxDate(timeLineData));
            Console.WriteLine("");
            (int years, int months, int days) = DeltaDate(timeLineData);
            Console.WriteLine($"ћежду макс и мин датами прошло: {years} лет, {months} мес€цев и {days} дней");
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

        static (DateTime, DateTime) FindMinAndMaxDate(string[][] timeline)
        {
            DateTime minDate = DateTime.MaxValue;
            DateTime maxDate = DateTime.MinValue;
            foreach (var line in timeline)
            {
                DateTime date = DateTime.Parse(line[0]);
                if (date < minDate) minDate = date;
                if (date > maxDate) maxDate = date;
            }
            return (maxDate, minDate);
        }

        static (int, int, int) DeltaDate(string[][] timeline)
        {
            (DateTime maxDate, DateTime minDate) = FindMinAndMaxDate(timeline);
            TimeSpan delta = maxDate - minDate; 
            Console.WriteLine(delta);
            DateTime diffdate = new DateTime() + delta;
            Console.WriteLine(diffdate);
            diffdate = diffdate.AddYears(-1);
            diffdate = diffdate.AddMonths(-1);
            diffdate = diffdate.AddDays(-3);
            Console.WriteLine(diffdate);
            return (diffdate.Year,diffdate.Month,diffdate.Day);
        }

        static void GetLeapYear(string[][] peopleData)
        {
            DateTime nowDate = DateTime.Now;
            foreach (var line in peopleData)
            {
                int age;
                DateTime birth = DateTime.Parse(line[2]);
                DateTime deathDate = DateTime.MinValue;
                if (line[2] == "") deathDate = DateTime.MinValue; else DateTime.ParseExact(line[2], "yyyy-mm-dd", CultureInfo.InvariantCulture);
                if (deathDate == DateTime.MinValue)
                {
                    age = nowDate.Year - birth.Year;
                    if (DateTime.Now.DayOfYear < birth.DayOfYear) age++;   //на случай, если день рождени€ уже прошЄл
                }
                else
                {
                    age = deathDate.Year - birth.Year;
                    if (deathDate.DayOfYear < birth.DayOfYear) age++;   //на случай, если день рождени€ уже прошЄл
                }              
                if ((DateTime.IsLeapYear(birth.Year)) & (age < 20)) Console.WriteLine(line[1]);
            }
        }
    }
}
