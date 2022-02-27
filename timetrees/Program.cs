using System;
using System.IO;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace timetrees
{
    class Program
    {
        private const string DeltaId       = "delta";
        private const string AddPersonId   = "addP";
        private const string AddEventId    = "addE";
        private const string EditPeopleId  = "edit";
        private const string LeapYearId    = "leap";
        private const string WritePeopleId = "writeP";
        private const string WriteEventId  = "writeE";
        private const string ExitId        = "exit";

        static void Main(string[] args)
        {
            FIleReader.ReadTimelineFromJson();
            FIleReader.ReadPeopleFromJson();
            WorkMenu();          
        }

        static void WorkMenu()
        {
            Console.CursorVisible = false;
            List<MenuItem> menu = new List<MenuItem>
            {
                new MenuItem {Id = DeltaId,         Text = "Найти дельту дат между событиями", IsSelected = true},
                new MenuItem {Id = AddPersonId,     Text = "Добавить человека"},
                new MenuItem {Id = AddEventId,      Text = "Добавить событие" },
                new MenuItem {Id = EditPeopleId,    Text = "Отредактировать данные человека"},
                new MenuItem {Id = LeapYearId,      Text = "Найти людей, родившихся в високосный год"},
                new MenuItem {Id = WritePeopleId,   Text = "Вывести всех людей в списке"},
                new MenuItem {Id = WriteEventId,    Text = "Вывести все события в списке"},
                new MenuItem {Id = ExitId,          Text = "Выход"}
            };
            bool exit = false;
            do
            {
                MenuTemplate.DrawMenu(menu);
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.DownArrow) MenuTemplate.MenuSelectNext(menu);
                if (keyInfo.Key == ConsoleKey.UpArrow) MenuTemplate.MenuSelectPrevious(menu);
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    var selectedItem = menu.First(x => x.IsSelected);
                    Execute(selectedItem.Id);
                    FileWriter.WritePeopleToJson();
                    FileWriter.WriteTimelineToJson();
                    Console.WriteLine("Хотите продолжить? Y/N");
                    if (!AnswerLogic.GetAnswer())
                    {
                        Exit.DoExit();
                    }
                }
            }
            while (!exit);
        }

        static void Execute(string doProgram)
        {
            Console.Clear();
            if (doProgram == DeltaId)       DeltaTimelineEvents.WriteDeltaDate();
            if (doProgram == AddPersonId)   AddMenu.WritePerson();
            if (doProgram == AddEventId)    AddMenu.WriteEvent();
            if (doProgram == EditPeopleId)  PersonEditor.EditPerson();
            if (doProgram == LeapYearId)    LeapYearPersons.DoGetLeapYear();
            if (doProgram == WritePeopleId) MenuTemplate.ShowPeople();
            if (doProgram == WriteEventId)  MenuTemplate.ShowEvent();
            if (doProgram == ExitId)        Exit.DoExit();
        }
    }
}
