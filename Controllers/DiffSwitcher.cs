using System;
using System.Collections.Generic;

namespace GloriousMinesweeper
{
    static class DiffSwitcher
    {
        public static GameSetting[] Colours { get; private set; }
        public static GameMenu[] GameMenus { get; private set; }
        private static List<IGraphic> Labels { get; set; }
        public static int ChosenMenu { get; private set; }
                
        public static void StartMenu(bool resetColours)
        {
            if (((Console.LargestWindowWidth - 5) > Console.WindowWidth) || ((Console.LargestWindowHeight - 3) > Console.WindowHeight))
                Program.WaitForFix();
            GameMenus = new GameMenu[4];
            GameMenus[0] = new GameMenu("Easy", Difficulties.Easy);
            GameMenus[1] = new GameMenu("Medium", Difficulties.Medium);
            GameMenus[2] = new GameMenu("Hard", Difficulties.Hard);
            GameMenus[3] = null;
            ChosenMenu = 0;
            SetDefault(resetColours);
            SwitchTo(0, true);
            Game newGame = EnableSwitch();
            if (newGame == null)
                return;
            GameControls.PlayedGame = newGame;
            Console.Clear();
            bool gameWon;
            bool UserWantsToPlayAgain;
            do
            {
                if (GameControls.PlayedGame == null)
                    return;
                GameControls.PlayedGame.PrintMinefield();
                GameControls.PlayedGame.TilesAndMinesAroundCalculator();
                GameControls.SetDefault();
                gameWon = GameControls.Gameplay(out decimal score, out SpecialisedStopwatch playTime);
                if (score != -1)
                    UserWantsToPlayAgain = PostGameMenu.ShowMenu(score, gameWon, playTime);
                else
                    UserWantsToPlayAgain = false;
            } while (UserWantsToPlayAgain);
        }
        public static void SwitchMenu(ConsoleKey consoleKey)
        {
            if (consoleKey == ConsoleKey.LeftArrow && ChosenMenu != 0)
            {
                ChosenMenu--;
                Console.Clear();
                PrintGraphics(true);
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
                PrintGraphics(true);
            }
            GameMenus[ChosenMenu].ChooseLine(0);
        }

        public static void SwitchTo(int number, bool highlightName, bool immediatePrint = true)
        {
            ChosenMenu = number;
            if (immediatePrint)
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
                        PrintMenuName(false);
                        int keypressedint = GameMenus[ChosenMenu].MenuAction();
                        /*if (keypressedint == -1)
                        {
                            SwitchTo(3, false);
                            PrintGraphics(true);
                            keypressedint = GameMenus[ChosenMenu].MenuAction();
                            if (keypressedint == 0)
                                keypressed = ConsoleKey.Enter;
                            else
                                PrintMenuName(true);
                        }
                        else if (keypressedint == 0)
                            keypressed = ConsoleKey.Enter;
                        else
                            PrintMenuName(true);*/
                        switch (keypressedint)
                        {
                            case 1:
                                PrintMenuName(true);
                                break;
                            case 0:
                                keypressed = ConsoleKey.Enter;
                                break;
                            case -1:
                                SwitchTo(3, false);
                                PrintGraphics(true);
                                keypressedint = GameMenus[ChosenMenu].MenuAction();
                                switch (keypressedint)
                                {
                                    case 1:
                                        PrintMenuName(true);
                                        break;
                                    case 0:
                                        keypressed = ConsoleKey.Enter;
                                        break;
                                    case -2:
                                        return null;
                                }
                                break;
                            case -2:
                                return null;
                        }
                        break;
                    case ConsoleKey.Escape:
                        return null;
                    case ConsoleKey.R:
                        try
                        {
                            Reprint();
                        }
                        catch
                        {
                            Program.WaitForFix();
                            Reprint();
                        }
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
            loadingSign.Print(false, Reprint);
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
        public static void SetDefault(bool resetColours)
        {
            PositionedText secondLine = new PositionedText("Game settings:", ConsoleColor.Black, (Console.WindowWidth - 14) / 2, 4);
            PositionedText thirdLine = new PositionedText("Use arrow keys to operate and enter to confirm", ConsoleColor.Black, (Console.WindowWidth - 46) / 2, 5);
            Border GameMenuBigBorder = new Border(0, 1, Console.WindowHeight - 1, Console.WindowWidth, ConsoleColor.Black, ConsoleColor.Gray, false);
            Border GameMenuSmallBorder = new Border(Console.WindowWidth / 2 - 37, 2, 31, 74, ConsoleColor.Black, ConsoleColor.White, false);
            Labels = new List<IGraphic>() { secondLine, thirdLine, GameMenuBigBorder, GameMenuSmallBorder };
            
            if (resetColours)
            {
                Colours = new GameSetting[7];
                Colours[0] = new GameSetting("Covered tiles colour", 10, true, false, 17);
                Colours[1] = new GameSetting("Covered tiles secondary colour", 2, true, false, 19);
                Colours[2] = new GameSetting("Uncovered tiles colour", 9, true, false, 21);
                Colours[3] = new GameSetting("Uncovered tiles secondary colour", 1, true, false, 23);
                Colours[4] = new GameSetting("Flag colour", 12, true, false, 25);
                Colours[5] = new GameSetting("Highlighted tile colour", 13, true, false, 27);
                Colours[6] = new GameSetting("Text colour", 7, false, true, 29);
                Program.DefaultTextColour = (ConsoleColor)Colours[6].SettingValue.Number;
                Program.TakenColours.Clear();
                foreach (GameSetting gameSetting in Colours)
                {
                    Program.TakenColours.Add((ConsoleColor)gameSetting.SettingValue.Number);
                }
            }
            Console.Clear();
            PrintGraphics(true);
        }
        public static void SetLoaded(string[] Parameters)
        {
            for (int x = 0; x < 7; x++)
                Colours[x].ChangeValueTo(Int32.Parse(Parameters[x + 3]), GameControls.Reprint);
            Program.TakenColours.Clear();
            foreach (GameSetting setting in Colours)
            {
                Program.TakenColours.Add((ConsoleColor)setting.SettingValue.Number);
            }
        }
        public static void PrintMenuName(bool highlight)
        {
            GameMenus[ChosenMenu].Name.Print(highlight, Reprint);
        }
        public static void PrintGraphics(bool printBorders)
        {
            if (printBorders)
                for (int x = 0; x < 4; x++)
                    Labels[x].Print(x == 2, Reprint);
            else
                for (int x = 0; x < 2; x++)
                    Labels[x].Print(false, Reprint);
        }
        private static void Reprint()
        {
            Console.Clear();
            PrintGraphics(true);
            GameMenus[ChosenMenu].PrintMenu(true);
        }
    }
}
