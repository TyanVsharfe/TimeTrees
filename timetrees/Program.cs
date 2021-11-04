using System;
using System.IO;
using System.Globalization;
using System.Collections.Generic;
using Microsoft.VisualBasic;
using System.Linq;

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

        private const string DeltaId     = "delta";
        private const string AddPersonId = "addP";
        private const string AddEventId  = "addE";
        private const string LeapYearId  = "leap";
        private const string ExitId      = "exit";

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
            Console.CursorVisible = false;
            List<MenuItem> menu = new List<MenuItem>
            {
                new MenuItem {Id = DeltaId, Text = "����� ������ ��� ����� ���������", IsSelected = true},
                new MenuItem {Id = AddPersonId, Text = "�������� ��������"},
                new MenuItem {Id = AddEventId, Text = "�������� �������" },
                new MenuItem {Id = LeapYearId, Text = "����� �����, ���������� � ���������� ���"},
                new MenuItem {Id = ExitId, Text = "�����"}
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
                   Console.WriteLine("������ ����������? y/n");
                   string answer = Console.ReadLine();
                   if (answer == "n" || answer == "no")
                    {
                        break;
                    }
                }
            }
            while (!exit);
        }

        static void DrawMenu(List<MenuItem> menu)
        {
            Console.Clear();
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
            if (doProgram == LeapYearId) DoGetLeapYear();
            if (doProgram == ExitId) DoExit();
        }

        static string[][] ReadData(string path)
        {
            string[] data = File.ReadAllLines(path);
            string[][] splitData = new string[data.Length][];
            for (int i = 0; i < data.Length; i++)
            {
                var line = data[i]; // "1; ���; 2000-06-06
                string[] parts = line.Split(";"); //["1", "��� 1", "2000-06-06"]
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

        static void DoDeltaDate()
        {
            string timeLineFile = "..\\..\\..\\..\\timeline.csv";
            TimelineEvent[] timeLineData = ReadTimelineData(timeLineFile);
            (int years, int months, int days) = DeltaDate(timeLineData);
            Console.WriteLine($"����� ���� � ��� ������ ������: ���: {years}, �������: {months} ����: {days}");

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
            Console.WriteLine("����� �����, ������� �������� � ���������� ��� � �� ������� �� ����� 20 ���: ");
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
                    if (DateTime.Now.DayOfYear < birth.DayOfYear) age++;   //�� ������, ���� ���� �������� ��� ������
                }
                else
                {
                    age = deathDate.Year - birth.Year;
                    if (deathDate.DayOfYear < birth.DayOfYear) age++;   //�� ������, ���� ���� �������� ��� ������
                }              
                if ((DateTime.IsLeapYear(birth.Year)) & (age < 20)) Console.WriteLine(person.name);
            }
        }

        static void DoExit()
        {
            Environment.Exit(0);
        }

        static void WritePerson()
        {
            string[] data = File.ReadAllLines("..\\..\\..\\..\\people.csv"); // �������� ����� ���������� ID

            Person person = new Person();
            Console.Clear();
            Console.WriteLine("������� ���");
            person.id = data.Length+1; // ���� ������ ��������, ���� ����������
            person.name = Console.ReadLine();

            Console.Clear();
            Console.WriteLine("������� ���� ��������");
            DateTime timebirthTrue = DateTime.MinValue;
            do
            {
                string date = Console.ReadLine();
                DateTime.TryParse(date, out DateTime result);
                timebirthTrue = result;
                if (result == DateTime.MinValue) Console.WriteLine("���� ������� �����������, ���������� �����");
                else person.birth = DateTime.Parse(date);
            }
            while (timebirthTrue == DateTime.MinValue);

            Console.Clear();
            Console.WriteLine("������� ���� ������ (��� ���������� ������� Enter)");
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
                    if (result == DateTime.MinValue) Console.WriteLine("���� ������� �����������, ���������� �����");
                    else
                    {
                        person.birth = DateTime.Parse(date);
                        death = date;
                    }
                }
               
            }
            while (timeDeathTrue == DateTime.MinValue);

            Console.Clear();
            Console.WriteLine("������� ���������� ��������� (0-2)");

            string parents = Console.ReadLine();
            if ((int.Parse(parents) > 2)|(int.Parse(parents)<0)) Console.WriteLine("�� ������� �������� ������, ���������� ������");
            if (int.Parse(parents) == 1)
            {
                Console.WriteLine("������� ID ������������");
                int trueParent = 0;
                do
                {
                    string parent = Console.ReadLine();
                    bool trueP = int.TryParse(parent, out trueParent);
                    if ((trueP)&(trueParent <= data.Length)) person.parentFirst = trueParent;
                    else Console.WriteLine("������ �������� ��� � �������, ���������� �����");
                } while(person.parentFirst == 0);
            }
            if (int.Parse(parents) == 2)
            {
                Console.WriteLine("������� ID ������� ������������");
                int trueParent = 0;
                do
                {
                    string parent = Console.ReadLine();
                    bool trueP = int.TryParse(parent, out trueParent);
                    if ((trueP) & (trueParent <= data.Length)) person.parentFirst = trueParent;
                    else Console.WriteLine("������ �������� ��� � �������, ���������� �����");
                } while (person.parentFirst == 0);
                Console.WriteLine("������� ID ������� ������������");
                do
                {
                    string parent = Console.ReadLine();
                    bool trueP = int.TryParse(parent, out trueParent);
                    if ((trueP) & (trueParent <= data.Length)) person.parentSecond = trueParent;
                    else Console.WriteLine("������ �������� ��� � �������, ���������� �����");
                } while (person.parentSecond == 0);
            }

            System.IO.StreamWriter People = new System.IO.StreamWriter("..\\..\\..\\..\\people.csv", true);
            //���� ��������� ���������, ���� �������� ��� ������� ����� ������� � ������� \/
            if (death != "") People.WriteLine($"{person.id};{person.name};{person.birth.Year}-{person.birth.Month}-{person.birth.Day};{person.death.Year}-{person.death.Month}-{person.death.Day};{person.parentFirst};{person.parentSecond}"); 
            else People.WriteLine($"{person.id};{person.name};{person.birth.Year}-{person.birth.Month}-{person.birth.Day};;{person.parentFirst};{person.parentSecond}");
            People.Close();
        }

        static void WriteEvent()
        {
            TimelineEvent timeEvent = new TimelineEvent();

            Console.Clear();
            Console.WriteLine("������� ���� �������");
            DateTime timeTrue = DateTime.MinValue;
            do
            {
                string date = Console.ReadLine();
                DateTime.TryParse(date, out DateTime result);
                timeTrue = result;
                if (result == DateTime.MinValue) Console.WriteLine("���� ������� �����������, ���������� �����");
                else timeEvent.time = DateTime.Parse(date);
            } 
            while(timeTrue == DateTime.MinValue);

            Console.Clear();
            Console.WriteLine("������� �������� �������");
            timeEvent.description = Console.ReadLine();

            Console.Clear();
            Console.WriteLine("������� ID �����, ������������� � ������ ������� ����� ������ (��� �������)");
            string path = Console.ReadLine();
            string[] peopleId = path.Split(" ");
            System.IO.StreamWriter Event = new System.IO.StreamWriter("..\\..\\..\\..\\timeline.csv", true);
            Event.Write($"{timeEvent.time.Year}-{timeEvent.time.Month}-{timeEvent.time.Day};{timeEvent.description}");
            if (path != "")
            {
                Event.Write(";");
                for (int i = 0; i < peopleId.Length; i++)
                {
                    Event.Write(peopleId[i]);
                    if (i < peopleId.Length) Event.Write(";");
                }
                Event.WriteLine();
            }
            Event.Close();
        }
    }
}
