using System;
using System.IO;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace timetrees
{
    public class DeltaTimelineEvents
    {
        public static void WriteDeltaDate()
        {
            (int years, int months, int days) = DeltaDate();
            Console.WriteLine($"Между макс и мин датами прошло: лет: {years} месяцев: {months} дней: {days}");
        }
            
        static (int, int, int) DeltaDate()
        {
            (DateTime maxDate, DateTime minDate) = FindMinAndMaxDate();
            TimeSpan delta = maxDate - minDate;
            DateTime diffdate = new DateTime() + delta;
            return (diffdate.Year - 1, diffdate.Month - 1, diffdate.Day - 1);
        }

        static (DateTime, DateTime) FindMinAndMaxDate()
        {
            DateTime minDate = DateTime.MaxValue;
            DateTime maxDate = DateTime.MinValue;
            foreach (var timeDate in DataRepo.TimelineRepo)
            {
                DateTime date = timeDate.time;
                if (date < minDate) minDate = date;
                if (date > maxDate) maxDate = date;
            }
            return (maxDate, minDate);
        }
    }
}
