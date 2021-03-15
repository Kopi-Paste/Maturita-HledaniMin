using System;

namespace GloriousMinesweeper
{
    static class DiffSwitcher
    {
        public static GameMenu[] GameMenus;
        public static int ChosenMenu;
                
        public static void SwitchMenu(ConsoleKey consoleKey)
        {
            if (consoleKey == ConsoleKey.LeftArrow && ChosenMenu != 0)
            {
                ChosenMenu--;
                Console.Clear();
                GameMenus[ChosenMenu].PrintMenu(true);
            }
                
            else if (consoleKey == ConsoleKey.RightArrow)
            {
                ChosenMenu++;
                if (ChosenMenu < GameMenus.Length)
                    if (GameMenus[ChosenMenu] != null)
                    {
                        Console.Clear();
                        GameMenus[ChosenMenu].PrintMenu(true);
                    }
                    else
                        ChosenMenu--;
                else
                    ChosenMenu--;
            }
        }

        public static void SwitchTo(int number, bool highlightName)
        {
            ChosenMenu = number;

            Console.Clear();
            GameMenus[ChosenMenu].PrintMenu(highlightName);
        }

        public static Game EnableSwitch()
        {
            SwitchTo(0, true);
            ConsoleKey keypressed;
            do
            {
                keypressed = Console.ReadKey(true).Key;
                switch (keypressed)
                {
                    case ConsoleKey.LeftArrow:
                        SwitchMenu(keypressed);
                        break;
                    case ConsoleKey.RightArrow:
                        SwitchMenu(keypressed);
                        break;
                    case ConsoleKey.DownArrow:
                        keypressed = GameMenus[ChosenMenu].MenuAction();
                        break;
                }
            } while (keypressed != ConsoleKey.Enter);
            if (keypressed == ConsoleKey.Enter)
            {
                int[] parameters = new int[10];
                for (int x = 0; x != 10; x++)
                {
                    parameters[x] = GameMenus[ChosenMenu].GameSettings[x].SettingValue.Number;
                }
                Console.Clear();
                PositionedText loadingSign = new PositionedText("Loading...", ConsoleColor.Black, (Console.WindowWidth - 10) / 2, 12);
                loadingSign.Print(false);
                return new Game(parameters);
            }
            else
            {
                do
                {
                    keypressed = GameMenus[3].MenuAction();
                } while (keypressed != ConsoleKey.Enter && keypressed != ConsoleKey.UpArrow);
                if (keypressed == ConsoleKey.Enter)
                {
                    int[] parameters = new int[10];
                    for (int x = 0; x != 10; x++)
                    {
                        parameters[x] = GameMenus[ChosenMenu].GameSettings[x].SettingValue.Number;
                    }
                    Console.Clear();
                    PositionedText loadingSign = new PositionedText("Loading...", ConsoleColor.Black, (Console.WindowWidth - 10) / 2, 12);
                    loadingSign.Print(false);
                    return new Game(parameters);
                }
                else
                {
                    PrintMenuName(true);
                    return EnableSwitch();
                }
            }
        }
        public static void PrintMenuName(bool highlight)
        {
            GameMenus[ChosenMenu].Name.Print(highlight);
        }
    }
}
