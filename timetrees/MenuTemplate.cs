using System;
using System.Collections.Generic;
using System.Linq;

namespace timetrees
{
    class MenuTemplate
    {
        public static void DrawMenu(List<MenuItem> menu)
        {
            RemoveScreenBlink();
            foreach (MenuItem menuItems in menu)
            {
                if (menuItems.IsSelected)
                    Console.BackgroundColor = ConsoleColor.Magenta;
                else
                    Console.BackgroundColor = ConsoleColor.Black;

                Console.WriteLine(menuItems.Text);
            }
            Console.BackgroundColor = ConsoleColor.Black;
        }

        public static void MenuSelectNext(List<MenuItem> menu)
        {
            var selectedItem = menu.First(x => x.IsSelected);
            int selectedIndex = menu.IndexOf(selectedItem);
            selectedItem.IsSelected = false;

            if (selectedIndex == menu.Count - 1) selectedIndex = 0;
            else selectedIndex = ++selectedIndex;

            menu[selectedIndex].IsSelected = true;
        }

        public static int MenuSelectNextPerson(int selectedIndex, int listCount)
        {
            if (selectedIndex == 0) return selectedIndex + 1;
            else if (selectedIndex + 1 < listCount) return selectedIndex + 1;
            else return 0;
        }

        public static int MenuSelectPrevPerson(int selectedIndex, int listCount)
        {
            if (selectedIndex - 1 < 0) return listCount - 1;
            else return selectedIndex - 1;
        }

        public static void MenuSelectPrevious(List<MenuItem> menu)
        {
            var selectedItem = menu.First(x => x.IsSelected);
            int selectedIndex = menu.IndexOf(selectedItem);
            selectedItem.IsSelected = false;

            if (selectedIndex == 0) selectedIndex = menu.Count - 1;
            else selectedIndex = --selectedIndex;

            menu[selectedIndex].IsSelected = true;
        }
        public static void RemoveScreenBlink()
        {
            for (int i = 0; i < Console.WindowHeight; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write(new string(' ', Console.WindowWidth));
            }
            Console.SetCursorPosition(0, 0);
        }
    }
}
