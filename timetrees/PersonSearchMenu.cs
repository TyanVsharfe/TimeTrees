using System;
using System.Collections.Generic;
using System.Text;

namespace timetrees
{
    public class PersonSearchMenu
    {
        public static Person FindPersonMenu()
        {
            List<Person> people = DataRepo.PeopleRepo;
            List<Person> found = new List<Person>();
            string name = string.Empty;
            int selectedIndex = 0;
            Person selectedPerson = new Person();
            do
            {
                MenuTemplate.RemoveScreenBlink();
                Console.WriteLine("ПОИСК ЛЮДЕЙ");
                Console.CursorVisible = true;
                Console.WriteLine($"Начните вводить имя: {name}");
                if (string.IsNullOrEmpty(name)) found = people;
                else found = FilterPeople(people, name);

                PrintPeople(found, selectedIndex);

                Console.SetCursorPosition($"Начните вводить имя: {name}".Length, 1);
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                if (Char.IsLetter(keyInfo.KeyChar))
                {
                    name += keyInfo.KeyChar;
                    selectedIndex = 0;
                }
                else if ((keyInfo.Key == ConsoleKey.Backspace) & (name != ""))
                {
                    name = name.Remove(name.Length - 1);
                }
                else if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    selectedIndex = MenuTemplate.MenuSelectNextPerson(selectedIndex, found.Count);
                }
                else if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    selectedIndex = MenuTemplate.MenuSelectPrevPerson(selectedIndex, found.Count);
                }
                else if ((keyInfo.Key == ConsoleKey.Enter) & (selectedIndex != 0))
                {
                    selectedPerson = found[selectedIndex];
                    break;
                }
                else if (keyInfo.Key == ConsoleKey.Escape)
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
                if (person.death != null)
                {
                    Console.WriteLine($"{person.id}\t" + $"{person.name}\t" + $"{person.birth:d}\t" + $"{person.death:d}");
                }
                else
                {
                    Console.WriteLine($"{person.id}\t" + $"{person.name}\t" + $"{person.birth:d}\t" + $"жив");
                }
                Console.BackgroundColor = ConsoleColor.Black;
            }
        }
    }
}
