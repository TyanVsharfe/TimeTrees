using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace timetrees
{
    internal class AddMenu
    {
        public static void WritePerson()
        {
            Person person = new();
            Console.Clear();
            Console.WriteLine("Введите имя");
            person.id = DataRepo.PeopleRepo.Count + 1;
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
            DataRepo.PeopleRepo.Add(person);
            Console.Clear();
        }

        public static void WriteEvent()
        {
            TimelineEvent timeEvent = new();

            Console.Clear();
            Console.WriteLine("Введите дату события");
            timeEvent.time = ValidationCheck.GetTrueDateTime();

            Console.Clear();
            Console.WriteLine("Введите описание события");
            timeEvent.description = Console.ReadLine();

            Console.Clear();
            Console.WriteLine("Сколько людей участвовало в событии?");
            int countEP = int.Parse(Console.ReadLine());

            for (int i = 0; i < countEP; i++)
            {
                Person peopleEvent = PersonSearchMenu.FindPersonMenu();
                timeEvent.participants.Add(peopleEvent);
            }
            DataRepo.TimelineRepo.Add(timeEvent);
            Console.Clear();
        }
    }
}
