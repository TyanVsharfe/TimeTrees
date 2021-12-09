using System;
using System.IO;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace timetrees
{
    class DataWriter
    {
        public static void WritePerson()
        {
            List<Person> people = DataReader.ReadListPersons();
            int countPeople = people.Count;

            Person person = new Person();
            Console.Clear();
            Console.WriteLine("Введите имя");
            person.id = countPeople + 1;
            person.name = Console.ReadLine();

            Console.Clear();
            Console.WriteLine("Введите Дату рождения");
            person.birth = ValidationCheck.GetTrueDateTime();

            Console.Clear();
            Console.WriteLine("Введите Дату смерти (при отсутствии нажмите Enter)");
            person.death = ValidationCheck.GetTrueDateTime();
            if (person.death == DateTime.MinValue) person.death = null;


            Console.Clear();
            Console.WriteLine("Укажите количество родителей (0-2)");

            string parents = Console.ReadLine();
            if ((int.Parse(parents) > 2) | (int.Parse(parents) < 0)) Console.WriteLine("Вы указали неверные данные, попробуйте заново");
            if (int.Parse(parents) == 1)
            {
                Person parent = PersonSearchMenu.FindPersonMenu();
                person.parents.Add(parent);
            }
            if (int.Parse(parents) == 2)
            {
                Person parent = PersonSearchMenu.FindPersonMenu();
                person.parents.Add(parent); ;
                parent = PersonSearchMenu.FindPersonMenu();
                person.parents.Add(parent);
            }

            System.IO.StreamWriter People = new System.IO.StreamWriter("..\\..\\..\\..\\people.csv", true);
            People.WriteLine($"{person.id};{person.name};{person.birth};{person.death};{person.parentFirst};{person.parentSecond}"); 
            People.Close();
            Console.Clear();
        }

        public static void WriteEvent()
        {
            TimelineEvent timeEvent = new TimelineEvent();

            Console.Clear();
            Console.WriteLine("Введите дату события");
            timeEvent.time = ValidationCheck.GetTrueDateTime();

            Console.Clear();
            Console.WriteLine("Введите описание события");
            timeEvent.description = Console.ReadLine();

            Console.Clear();
            Console.WriteLine("Сколько людей участвовало в событии?");
            int countEP = int.Parse(Console.ReadLine());

            System.IO.StreamWriter Event = new System.IO.StreamWriter("..\\..\\..\\..\\timeline.csv", true);
            Event.Write($"{timeEvent.time.Year}-{timeEvent.time.Month}-{timeEvent.time.Day};{timeEvent.description}");
            for (int i = 1; i <= countEP; i++)
            {
                Person parent = PersonSearchMenu.FindPersonMenu();
                if (i < countEP) Event.Write(";");
                Event.Write($"{parent.id}");
            }
            Event.WriteLine();
            Event.Close();
        }

        public static void DoWritePeople()
        {
            string[] data = File.ReadAllLines("..\\..\\..\\..\\people.csv");
            for (int i = 0; i < data.Length; i++)
            {
                Console.WriteLine(data[i]);
            }
            string dataJson = JsonConvert.SerializeObject(data);
            File.WriteAllText("..\\..\\..\\..\\people.json", dataJson);
        }

        public static void ShowPeople()
        {
            List<Person> people = DataReader.ReadListPersons();
            for (var i = 0; i < people.Count; i++)
            {
                Person person = people[i];
                if (person.death != null)
                {
                    Console.WriteLine($"{person.id}\t" + $"{person.name}\t" + $"{person.birth:d}\t" + $"{person.death:d}");
                }
                else
                {
                    Console.WriteLine($"{person.id}\t" + $"{person.name}\t" + $"{person.birth:d}\t" + $"жив");
                }
            }
        }

        public static void ShowEvent()
        {
            List<Person> people = DataReader.ReadListPersons();
            string[] data = File.ReadAllLines("..\\..\\..\\..\\timeline.csv");
            for (int i = 0; i < data.Length; i++)
            {
                var line = data[i];
                string[] parts = line.Split(";");
                Console.Write($"{parts[DataReader.timelineDateIndex]}\t{parts[DataReader.timelineDescriptionIndex]}\t Участники: ");
                if (parts.Length >= 3) for (int j = 2; j < parts.Length; j++) Console.Write($"{people[int.Parse(parts[j])-1].name}\t");
                else Console.Write("нет");
                Console.WriteLine();
            }
            string dataJson = JsonConvert.SerializeObject(data);
            File.WriteAllText("..\\..\\..\\..\\timeline.json", dataJson);
        }

        static List<Person> WriteDataToJson(string path)
        {
            string json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<List<Person>>(json);
        }
    }
}
