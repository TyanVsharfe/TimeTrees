using System;
using System.IO;
using System.Globalization;
using System.Collections.Generic;
using Microsoft.VisualBasic;
using System.Linq;
using Newtonsoft.Json;

namespace timetrees
{
    class Program
    {
        private static List<Person> people;
        private static int countPeople;

        const int personIdIndex    = 0;
        const int personNameIndex  = 1;
        const int personBirthIndex = 2;
        const int personDeathIndex = 3;
        const int personOneParent  = 4;
        const int personTwoParent  = 5;

        const int timelineDateIndex        = 0;
        const int timelineDescriptionIndex = 1;
        
        private const string DeltaId       = "delta";
        private const string AddPersonId   = "addP";
        private const string AddEventId    = "addE";
        private const string FoundPeopleId = "Found";
        private const string LeapYearId    = "leap";
        private const string WritePeopleId = "writeP";
        private const string WriteEventId  = "writeE";
        private const string ExitId        = "exit";

        class MenuItem
        {
            public string Id;
            public string Text;
            public bool IsSelected;
        }

        struct Person
        {
            public int      id;
            public string   name;
            public DateTime birth;
            public DateTime death;
            public int      parentFirst;
            public int      parentSecond;
        }

        struct TimelineEvent
        {
            public DateTime time;
            public string   description;
        }

        static void Main(string[] args)
        {
            people = ReadListPersons();
            countPeople = people.Count;
            Console.CursorVisible = false;
            List<MenuItem> menu = new List<MenuItem>
            {
                new MenuItem {Id = DeltaId,         Text = "Найти дельту дат между событиями", IsSelected = true},
                new MenuItem {Id = AddPersonId,     Text = "Добавить человека"},
                new MenuItem {Id = AddEventId,      Text = "Добавить событие" },
                new MenuItem {Id = FoundPeopleId,   Text = "Поиск людей"},
                new MenuItem {Id = LeapYearId,      Text = "Найти людей, родившихся в високосный год"},
                new MenuItem {Id = WritePeopleId,   Text = "Вывести всех людей в списке"},
                new MenuItem {Id = WriteEventId,    Text = "Вывести все события в списке"},
                new MenuItem {Id = ExitId,          Text = "Выход"}
            };
            bool exit = false;
            do
            {
                DrawMenu(menu);
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.DownArrow) MenuSelectNext(menu);
                if (keyInfo.Key == ConsoleKey.UpArrow) MenuSelectPrevious(menu);
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                   var selectedItem = menu.First(x => x.IsSelected);
                   Execute(selectedItem.Id);
                   Console.WriteLine("Хотите продолжить? Y/N");
                    string answer;
                    do
                    {                      
                        answer = Console.ReadLine();
                        if (GetNegativeAnswer(answer)) DoExit();
                        if (GetPositiveAnswer(answer) == false) Console.WriteLine("Ваш ответ некорректен, введите Y/N");
                    } while (GetPositiveAnswer(answer) == false & GetNegativeAnswer(answer) == false);
                }
            }
            while (!exit);
        }

        static bool GetNegativeAnswer(string answer)
        {   
            string[] negativeAnswers = new[] { "n", "no", "N", "No","NO", "н", "Н", "нет", "Нет", "НЕТ"};
            if (negativeAnswers.Contains(answer)) return true;
            else return false;
        }

        static bool GetPositiveAnswer(string answer)
        {
            string[] positiveAnswers = new[] { "y", "yes", "Y", "Yes", "YES", "д", "Д", "да", "Да", "ДА"};
            if (positiveAnswers.Contains(answer)) return true;
            else return false;
        }

        static void DrawMenu(List<MenuItem> menu)
        {
            RemoveScreenBlink();
            foreach(MenuItem menuItems in menu)
            {
                if (menuItems.IsSelected) 
                    Console.BackgroundColor = ConsoleColor.Magenta;
                else 
                    Console.BackgroundColor = ConsoleColor.Black;            

                Console.WriteLine(menuItems.Text);
            }
            Console.BackgroundColor = ConsoleColor.Black;
        }
        
        static void RemoveScreenBlink()
        {
            for (int i = 0; i < Console.WindowHeight; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write(new string(' ',Console.WindowWidth));
            }
            Console.SetCursorPosition(0, 0);
        }

        static void MenuSelectNext(List<MenuItem> menu)
        {
            var selectedItem = menu.First(x => x.IsSelected);
            int selectedIndex   = menu.IndexOf(selectedItem);
            selectedItem.IsSelected = false;

            if (selectedIndex == menu.Count - 1) selectedIndex = 0;
            else selectedIndex = ++selectedIndex;

            menu[selectedIndex].IsSelected = true;
        }

        static void MenuSelectPrevious(List<MenuItem> menu)
        {
            var selectedItem = menu.First(x => x.IsSelected);
            int selectedIndex = menu.IndexOf(selectedItem);
            selectedItem.IsSelected = false;

            if (selectedIndex == 0) selectedIndex = menu.Count - 1;
            else selectedIndex = --selectedIndex;

            menu[selectedIndex].IsSelected = true;
        }

        static void Execute(string doProgram)
        {
            Console.Clear();
            if (doProgram == DeltaId) DoDeltaDate();
            if (doProgram == AddPersonId) WritePerson();
            if (doProgram == AddEventId) WriteEvent();
            if (doProgram == FoundPeopleId) FindPersonMenu();
            if (doProgram == LeapYearId) DoGetLeapYear();
            if (doProgram == WritePeopleId) DoWritePeople();
            if (doProgram == WriteEventId) DoWriteEvent();
            if (doProgram == ExitId) DoExit();
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

        static List<Person> ReadListPersons()
        {
            Person[] people = ReadPersonData("..\\..\\..\\..\\people.csv");
            List<Person> result = new List<Person>();

            foreach (var element in people)
            {            
               result.Add(element);
            }
            Console.WriteLine(result);
            return result;
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

        static List<Person> ReadDataFromJson(string path)
        {
            string json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<List<Person>>(json);
        }

        static List<Person> WriteDataToJson(string path)
        {
            string json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<List<Person>>(json);
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

        static void DoDeltaDate()
        {
            string timeLineFile = "..\\..\\..\\..\\timeline.csv";
            TimelineEvent[] timeLineData = ReadTimelineData(timeLineFile);
            (int years, int months, int days) = DeltaDate(timeLineData);
            Console.WriteLine($"Между макс и мин датами прошло: лет: {years}, месяцев: {months} дней: {days}");

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

        static void DoGetLeapYear()
        {
            string peopleFile = "..\\..\\..\\..\\people.csv";
            Person[] peopleData = ReadPersonData(peopleFile);
            Console.WriteLine("Имена людей, которые родились в високосный год и их возраст не более 20 лет: ");
            GetLeapYear(peopleData);
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
                    if (DateTime.Now.DayOfYear < birth.DayOfYear) age++;   //на случай, если день рождения уже прошёл
                }
                else
                {
                    age = deathDate.Year - birth.Year;
                    if (deathDate.DayOfYear < birth.DayOfYear) age++;   //на случай, если день рождения уже прошёл
                }              
                if ((DateTime.IsLeapYear(birth.Year)) & (age < 20)) Console.WriteLine(person.name);
            }
        }

        static void DoWritePeople()
        {
            string[] data = File.ReadAllLines("..\\..\\..\\..\\people.csv");
            for (int i = 0; i < data.Length; i++)
            {
                Console.WriteLine(data[i]);            
            }
            string dataJson = JsonConvert.SerializeObject(data);
            File.WriteAllText("..\\..\\..\\..\\people.json", dataJson);
        }

        static void DoWriteEvent()
        {
            string[] data = File.ReadAllLines("..\\..\\..\\..\\timeline.csv");
            for (int i = 0; i < data.Length; i++)
            {
                Console.WriteLine(data[i]);
            }
            string dataJson = JsonConvert.SerializeObject(data);
            File.WriteAllText("..\\..\\..\\..\\timeline.json",dataJson);
        }

        static void DoExit()
        {
            Environment.Exit(0);
        }

        static Person FindPersonMenu()
        {
            List<Person> found = new List<Person>();
            string name = string.Empty;
            int selectedIndex = 0;
            Person selectedPerson = new Person();
            do
            {
                RemoveScreenBlink();
                Console.WriteLine("ПОИСК ЛЮДЕЙ");
                Console.CursorVisible = true;
                Console.WriteLine($"Начните вводить имя: {name}");
                if (string.IsNullOrEmpty(name)) found = people;
                else found = FilterPeople(people,name);

                PrintPeople(found, selectedIndex);

                Console.SetCursorPosition($"Начните вводить имя: {name}".Length, 1);
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                if (Char.IsLetter(keyInfo.KeyChar))
                {
                    name += keyInfo.KeyChar;
                    selectedIndex = 0;
                }
                else if ((keyInfo.Key == ConsoleKey.Backspace)&(name != ""))
                {
                    name = name.Remove(name.Length - 1);
                }
                else if(keyInfo.Key == ConsoleKey.DownArrow)
                {
                    if (selectedIndex == 0) selectedIndex++;
                    else if (selectedIndex + 1 < found.Count) selectedIndex++;
                    else selectedIndex = 0;
                }
                else if(keyInfo.Key == ConsoleKey.UpArrow)
                {
                    if (selectedIndex == 0) selectedIndex = found.Count - 1;
                    else if (selectedIndex - 1 < 0) selectedIndex = found.Count - 1;
                    else selectedIndex--;
                }
                else if(keyInfo.Key == ConsoleKey.Enter)
                {
                    if (selectedIndex != 0)
                    {
                        selectedPerson = found[selectedIndex];
                        break;
                    }
                }
                else if(keyInfo.Key == ConsoleKey.Escape)
                {
                    break;
                }

            } while (true);
            return selectedPerson;
        }

        static List<Person> FilterPeople(List<Person> people, string name)
        {
            List<Person> result = new List<Person>();
            foreach (Person person in people)
            {
                if (person.name.Contains(name))
                    result.Add(person);

            }
            return result;
        }

        static void PrintPeople(List<Person> people, int selectedIndex)
        {
            for (var i = 0; i < people.Count; i++)
            {
                Person person = people[i];
                if (selectedIndex == i)
                {
                    Console.BackgroundColor = ConsoleColor.Magenta;
                }
                if (person.death != DateTime.MinValue) Console.WriteLine($"{person.id}\t"+$"{person.name}\t"+$"{person.birth}\t"+$"{person.death}");
                else Console.WriteLine($"{person.id}\t" + $"{person.name}\t" + $"{person.birth}\t" + $"жив");

                Console.BackgroundColor = ConsoleColor.Black;
            }
        }

        static void EditPerson()
        {
            Person editPerson = FindPersonMenu();
        }

        static void WritePerson()
        {
            Person person = new Person();
            Console.Clear();
            Console.WriteLine("Введите имя");
            person.id = countPeople + 1;
            person.name = Console.ReadLine();

            Console.Clear();
            Console.WriteLine("Введите Дату рождения");
            DateTime timebirthTrue = DateTime.MinValue;
            do
            {
                string date = Console.ReadLine();
                DateTime.TryParse(date, out DateTime result);
                timebirthTrue = result;
                if (result == DateTime.MinValue) Console.WriteLine("Дата введена некорректно, попробуйте снова");
                else person.birth = DateTime.Parse(date);
            }
            while (timebirthTrue == DateTime.MinValue);

            Console.Clear();
            Console.WriteLine("Введите Дату смерти (при отсутствии нажмите Enter)");
            DateTime timeDeathTrue = DateTime.MinValue;
            string death = "";
            do
            {
                string date = Console.ReadLine();
                if (date == "")
                {              
                    break;                   
                }
                else
                {
                    DateTime.TryParse(date, out DateTime result);
                    timeDeathTrue = result;
                    if (result == DateTime.MinValue) Console.WriteLine("Дата введена некорректно, попробуйте снова");
                    else
                    {
                        person.birth = DateTime.Parse(date);
                        death = date;
                    }
                }         
            }
            while (timeDeathTrue == DateTime.MinValue);

            Console.Clear();
            Console.WriteLine("Укажите количество родителей (0-2)");

            string parents = Console.ReadLine();
            if ((int.Parse(parents) > 2)|(int.Parse(parents)<0)) Console.WriteLine("Вы указали неверные данные, попробуйте заново");
            if (int.Parse(parents) == 1)
            {
                Person parent = FindPersonMenu();
                person.parentFirst = parent.id;
            }
            if (int.Parse(parents) == 2)
            {
                Person parent = FindPersonMenu();
                person.parentFirst = parent.id;
                parent = FindPersonMenu();
                person.parentSecond = parent.id;
            }

            System.IO.StreamWriter People = new System.IO.StreamWriter("..\\..\\..\\..\\people.csv", true);
            //Мега дибильное ветвление, надо подумать как сделать более красиво и покроче \/
            if (death != "") People.WriteLine($"{person.id};{person.name};{person.birth.Year}-{person.birth.Month}-{person.birth.Day};{person.death.Year}-{person.death.Month}-{person.death.Day};{person.parentFirst};{person.parentSecond}"); 
            else People.WriteLine($"{person.id};{person.name};{person.birth.Year}-{person.birth.Month}-{person.birth.Day};;{person.parentFirst};{person.parentSecond}");
            People.Close();
        }

        static void WriteEvent()
        {
            TimelineEvent timeEvent = new TimelineEvent();

            Console.Clear();
            Console.WriteLine("Введите дату события");
            DateTime timeTrue = DateTime.MinValue;
            do
            {
                string date = Console.ReadLine();
                if (ParseDate(date) == DateTime.MinValue) Console.WriteLine("Дата введена некорректно, попробуйте снова");
                else
                {
                    timeEvent.time = ParseDate(date);
                    timeTrue = ParseDate(date);
                }
            } 
            while(timeTrue == DateTime.MinValue);

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
                Person parent = FindPersonMenu();             
                if (i <= countEP) Event.Write(";");
                Event.Write($"{parent.id}");               
            }
            Event.Close();   
      
        }

        public static DateTime ParseDate(string value)
        {
            DateTime date;
            if (!DateTime.TryParseExact(value, "yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            {
                if (!DateTime.TryParseExact(value, "yyyy-mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                {
                    if (!DateTime.TryParseExact(value, "yyyy-mm-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                    {
                        return DateTime.MinValue;
                    }
                }
            }
            return date;
        }
    }
}
