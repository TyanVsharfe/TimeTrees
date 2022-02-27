using System;
using System.Collections.Generic;
using System.Text;

namespace timetrees
{
    public class Person
    {
        public int id;
        public string name;
        public DateTime birth;
        public DateTime? death;
        public List<Person> parents = new(2);
    }
}
