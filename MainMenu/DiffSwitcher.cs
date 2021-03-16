using System;

namespace GloriousMinesweeper
{
    static class DiffSwitcher
    {
        public static GameSetting[] Colours;
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
            GameMenus[ChosenMenu].ChooseLine(0);
        }

        public static void SwitchTo(int number, bool highlightName)
        {
            ChosenMenu = number;

            Console.Clear();
            GameMenus[ChosenMenu].PrintMenu(highlightName);
        }

        public static Game EnableSwitch()
        {
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
                        int keypressedint = GameMenus[ChosenMenu].MenuAction();
                        if (keypressedint == -1)
                        {
                            SwitchTo(3, false);
                            GameMenus[ChosenMenu].MenuAction();
                        }
                        else if (keypressedint == 0)
                            keypressed = ConsoleKey.Enter;
                        else
                            PrintMenuName(true);
                        break;
                }
            } while (keypressed != ConsoleKey.Enter);
            int[] parameters = new int[10];
            for (int x = 0; x < 10; x++)
            {
                if (x <= 2)
                    parameters[x] = GameMenus[ChosenMenu].GameSettings[x].SettingValue.Number;
                else
                    parameters[x] = Colours[x - 3].SettingValue.Number;
            }
            Console.Clear();
            PositionedText loadingSign = new PositionedText("Loading...", ConsoleColor.Black, (Console.WindowWidth - 10) / 2, 12);
            loadingSign.Print(false);
            return new Game(parameters);
            /*ConsoleKey keypressed;
            int keypressedint = 0;
            do
            {
                keypressed = Console.ReadKey(true).Key;
                switch (keypressed)
                {
                    case ConsoleKey.LeftArrow:
                        SwitchMenu(keypressed);
                        keypressedint = (int)ConsoleKey.LeftArrow;
                        break;
                    case ConsoleKey.RightArrow:
                        SwitchMenu(keypressed);
                        keypressedint = (int)ConsoleKey.RightArrow;
                        break;
                    case ConsoleKey.DownArrow:
                        keypressedint = GameMenus[ChosenMenu].MenuAction();
                        break;
                }
            } while (keypressedint != (int)ConsoleKey.Enter && keypressedint != -1);
            if (keypressedint == ()ConsoleKey.Enter)
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
                    keypressedint = GameMenus[3].MenuAction();
                } while (keypressedint != (int)ConsoleKey.Enter && keypressedint != (int)ConsoleKey.UpArrow);
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
            }*/
        }
        public static void SetDefaultColours()
        {
            Colours = new GameSetting[7];
            Colours[0] = new GameSetting("Covered tiles colour", 10, true, false, 13);
            Colours[1] = new GameSetting("Covered tiles secondary colour", 2, true, false, 15);
            Colours[2] = new GameSetting("Uncovered tiles colour", 9, true, false, 17);
            Colours[3] = new GameSetting("Uncovered tiles secondary colour", 1, true, false, 19);
            Colours[4] = new GameSetting("Flag colour", 12, true, false, 21);
            Colours[5] = new GameSetting("Highlighted tile colour", 13, true, false, 23);
            Colours[6] = new GameSetting("Text colour", 7, false, true, 25);
            Program.DefaultTextColour = (ConsoleColor)Colours[6].SettingValue.Number;
            foreach (GameSetting gameSetting in Colours)
            {
                Program.TakenColours.Add((ConsoleColor)gameSetting.SettingValue.Number);
            }
        }
        public static void PrintMenuName(bool highlight)
        {
            GameMenus[ChosenMenu].Name.Print(highlight);
        }
    }
}
