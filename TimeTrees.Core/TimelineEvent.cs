using System;
using System.Collections.Generic;
using System.Text;

namespace timetrees
{
    public class TimelineEvent
    {
        public DateTime time;
        public string description;
        public List<Person> participants = new();
    }
}
