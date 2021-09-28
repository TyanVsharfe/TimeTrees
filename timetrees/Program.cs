using System;
using System.IO;

namespace timetrees
{
    class Program
    {
        static void Main(string[] args)
        {
            string timeLineFile = "..\\..\\..\\..\\timeline.csv";
            string[] lines = File.ReadAllLines(timeLineFile);
            foreach (var line in lines)
            {
                Console.WriteLine(line);
            }
            string[][] timeLineData = ReadData(timeLineFile);
            Console.WriteLine(FindMinAndMaxDate(timeLineData));
            Console.WriteLine(DeltaDate(timeLineData));
        }

        static string[][] ReadData(string path)
        {
            string[] data = File.ReadAllLines(path);
            string[][] splitData = new string[data.Length][];
            for (int i=0; i<data.Length; i++)
            {
                var line = data[i];
                string[] parts = line.Split(";");
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
            (DateTime  maxDate, DateTime minDate) = FindMinAndMaxDate(timeline);            
            return (maxDate.Year - minDate.Year, maxDate.Month - minDate.Month, maxDate.Day - minDate.Day);
        }
    }
}
