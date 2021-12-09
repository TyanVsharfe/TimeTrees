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
            List<TimelineEvent> timeLineData = DataReader.ReadListTimelineData();
            (int years, int months, int days) = DeltaDate(timeLineData);
            Console.WriteLine($"Между макс и мин датами прошло: лет: {years} месяцев: {months} дней: {days}");
        }
            
        static (int, int, int) DeltaDate(List<TimelineEvent> timeline)
        {
            (DateTime maxDate, DateTime minDate) = FindMinAndMaxDate(timeline);
            TimeSpan delta = maxDate - minDate;
            DateTime diffdate = new DateTime() + delta;
            diffdate = diffdate.AddYears(-1);
            diffdate = diffdate.AddMonths(-1);
            diffdate = diffdate.AddDays(-3);
            return (diffdate.Year, diffdate.Month, diffdate.Day);
        }

        static (DateTime, DateTime) FindMinAndMaxDate(List<TimelineEvent> timeline)
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
    }
}
