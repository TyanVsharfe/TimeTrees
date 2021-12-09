using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace timetrees
{
    class PersonEditor
    {
        private const string EditName    = "editName";
        private const string EditBirth   = "editBirth";
        private const string EditDeath   = "editDeath";
        private const string EditParents = "editParents";
        private const string SavePerson  = "savePerson";

        private const string EditFirstP     = "EditFirstP";
        private const string EditSecondP    = "EditSecondP";
        private const string EditBothP      = "EditBothP";
        private const string DeleteFirstP   = "DeleteFirstP";
        private const string DeleteSecondP  = "DeleteSecondP";
        private const string DeleteBothP    = "DeleteBothP";

        public static void EditPerson() //сократить, не проверил все остальное
        {
            List<Person> people = DataReader.ReadListPersons();
            Person editPerson = PersonSearchMenu.FindPersonMenu();
            Console.Clear();

            List<MenuItem> editMenu = new List<MenuItem>
            {
                new MenuItem {Id = EditName,    Text = "Изменить имя", IsSelected = true},
                new MenuItem {Id = EditBirth,   Text = "Изменить дату рождения"},
                new MenuItem {Id = EditDeath,   Text = "Изменить Дату смерти"},
                new MenuItem {Id = EditParents, Text = "Изменить данные о родителях"},
                new MenuItem {Id = SavePerson,  Text = "Сохранить данные"},
            };
            bool exit = false;
            do
            {
                MenuTemplate.DrawMenu(editMenu);
                Console.WriteLine();
                Console.WriteLine($"{editPerson.name}\t{editPerson.birth:D}\t{editPerson.death:D}");
                Console.WriteLine($"Первый родитель: {editPerson.parents[0].name}\t   {editPerson.parents[0].birth:D}\t {editPerson.parents[0].death:D}");
                Console.WriteLine($"Второй родитель: {editPerson.parents[1].name}\t   {editPerson.parents[1].birth:D}\t {editPerson.parents[1].death:D}");
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.DownArrow) MenuTemplate.MenuSelectNext(editMenu);
                if (keyInfo.Key == ConsoleKey.UpArrow) MenuTemplate.MenuSelectPrevious(editMenu);
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    var selectedItem = editMenu.First(x => x.IsSelected);
                    ExecuteEdit(selectedItem.Id, editPerson, people);
                    Console.WriteLine("Хотите продолжить редактирование? Y/N");
                    if (!AnswerLogic.GetAnswer()) break;
                }
            }
            while (!exit);
        }

        public static void ExecuteEdit(string doProgram, Person editPerson, List<Person> people)
        {
            if (doProgram == EditName)
            {
                Console.WriteLine("Введите новое имя");
                editPerson.name = Console.ReadLine();
            }
            if (doProgram == EditBirth)
            {
                Console.WriteLine("Введите новую дату рождения");
                editPerson.birth = ValidationCheck.GetTrueDateTime();
            }
            if (doProgram == EditDeath)
            {
                Console.WriteLine("Введите новую дату смерти");
                editPerson.death = ValidationCheck.GetTrueDateTime();
                if (editPerson.death == DateTime.MinValue) editPerson.death = null;
            }
            if (doProgram == EditParents)
            {
                List<MenuItem> editParents = new List<MenuItem>
                {
                    new MenuItem {Id = EditFirstP,      Text = "Изменить первого родителя", IsSelected = true},
                    new MenuItem {Id = EditSecondP,     Text = "Изменить второго родителя"},
                    new MenuItem {Id = EditBothP,       Text = "Изменить оба родителя"},
                    new MenuItem {Id = DeleteFirstP,    Text = "Удалить первого родителя"},
                    new MenuItem {Id = DeleteSecondP,   Text = "Удалить второго родителя"},
                    new MenuItem {Id = DeleteBothP,     Text = "Удалить всех родителей"},
                };
                bool exit = false;
                do
                {
                    MenuTemplate.DrawMenu(editParents);
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    if (keyInfo.Key == ConsoleKey.DownArrow) MenuTemplate.MenuSelectNext(editParents);
                    if (keyInfo.Key == ConsoleKey.UpArrow) MenuTemplate.MenuSelectPrevious(editParents);
                    if (keyInfo.Key == ConsoleKey.Enter)
                    {
                        var selectedItem = editParents.First(x => x.IsSelected);
                        ExecuteEditParents(selectedItem.Id, editPerson, people);
                        Console.WriteLine("Хотите продолжить? Y/N");
                        if (!AnswerLogic.GetAnswer()) break;
                    }
                }
                while (!exit);
            }
            people.RemoveAt(editPerson.id - 1);
            people.Insert(editPerson.id - 1, editPerson);
            System.IO.StreamWriter People = new System.IO.StreamWriter("..\\..\\..\\..\\people.csv", false);
            foreach (Person person in people)
            {
                People.WriteLine($"{person.id};{person.name};{person.birth};{person.death};{person.parentFirst};{person.parentSecond}");
            }
            People.Close();
        }

        static void ExecuteEditParents(string doProgram, Person editPerson, List<Person> people)
        {
            if (doProgram == EditFirstP)
            {
                Person parent = PersonSearchMenu.FindPersonMenu();
                editPerson.parents.Add(parent);
            }
            if (doProgram == EditSecondP)
            {
                Person parent = PersonSearchMenu.FindPersonMenu();
                editPerson.parents.Add(parent);
            }
            if (doProgram == EditBothP)
            {
                Person parent = PersonSearchMenu.FindPersonMenu();
                editPerson.parents.Add(parent);
                parent = PersonSearchMenu.FindPersonMenu();
                editPerson.parents.Add(parent);
            }
            if (doProgram == DeleteFirstP)
            {
                editPerson.parentFirst = editPerson.parentSecond;
                editPerson.parentSecond = null;
            }
            if (doProgram == DeleteSecondP)
            {
                editPerson.parentSecond = null;
            }
            if (doProgram == DeleteBothP)
            {
                editPerson.parentFirst = null;
                editPerson.parentSecond = null;
            }
            people.RemoveAt(editPerson.id - 1);
            people.Insert(editPerson.id - 1, editPerson);
            System.IO.StreamWriter People = new System.IO.StreamWriter("..\\..\\..\\..\\people.csv", false);
            foreach (Person person in people)
            {
                People.WriteLine($"{person.id};{person.name};{person.birth};{person.death};{person.parents[0]};{person.parents[1]}");
            }
            People.Close();
        }
    }
}
