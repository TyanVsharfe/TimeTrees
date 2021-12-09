using System;
using System.Collections.Generic;
using System.Text;

namespace timetrees
{
    public class LeapYearPersons
    {
        public static void DoGetLeapYear()
        {
            List<Person> peopleData = DataReader.ReadListPersons();
            Console.WriteLine("Имена людей, которые родились в високосный год и их возраст не более 20 лет: ");
            GetLeapYear(peopleData);
        }

        static void GetLeapYear(List<Person> peopleData)
        {
            DateTime nowDate = DateTime.Now;
            foreach (var person in peopleData)
            {
                int age;
                DateTime birth = person.birth;
                DateTime? deathDate = DateTime.MinValue;
                if (person.death == null) deathDate = DateTime.MinValue; else deathDate = person.death;
                if (deathDate == null)
                {
                    age = nowDate.Year - birth.Year;
                    if (DateTime.Now.DayOfYear < birth.DayOfYear) age++;   //на случай, если день рождения уже прошёл
                }
                else
                {
                    age = Convert.ToDateTime(deathDate).Year - birth.Year;
                    if (Convert.ToDateTime(deathDate).DayOfYear < birth.DayOfYear) age++;   //на случай, если день рождения уже прошёл
                }
                if ((DateTime.IsLeapYear(birth.Year)) & (age < 20)) Console.WriteLine(person.name);
            }
        }
    }
}
