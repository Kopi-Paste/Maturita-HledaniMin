using System;
using System.Collections.Generic;

namespace GloriousMinesweeper
{
    class GameMenu
    {
        public PositionedText Name { get; }
        public GameSetting[] GameSettings { get; private set; }
        public int ChosenLine { get; protected set; }
        public Difficulties Difficulty { get; }



        public GameMenu(string name, Difficulties difficulty)
        {
            Difficulty = difficulty;
            Name = new PositionedText(name, ConsoleColor.Black, (Console.WindowWidth - name.Length) / 2, 0) ;
            int mines = 5 * (int)Math.Pow((int)difficulty, 2) * ((int)difficulty + 1);


            GameSettings = new GameSetting[10];
            Program.TakenColours = new List<ConsoleColor>();
            GameSettings[0] = new GameSetting("Number of horizontal tiles", (int)difficulty * 10, false, false, 7);
            GameSettings[1] = new GameSetting("Number of vertical tiles", (int)difficulty * 10, false, false, 9);
            GameSettings[2] = new GameSetting("Number of mines", mines, false, false, 11);
            GameSettings[3] = new GameSetting("Covered tiles colour", 1, true, false, 13);
            GameSettings[4] = new GameSetting("Covered tiles secondary colour", 2, true, false, 15);
            GameSettings[5] = new GameSetting("Uncovered tiles colour", 3, true, false, 17);
            GameSettings[6] = new GameSetting("Uncovered tiles secondary colour", 4, true, false, 19);
            GameSettings[7] = new GameSetting("Flag colour", 5, true, false, 21);
            GameSettings[8] = new GameSetting("Highlighted tile colour", 6, true, false, 23);
            GameSettings[9] = new GameSetting("Text colour", 7, false, true, 25);
            for (int x = 1; x < 8; x++)
                Program.TakenColours.Add((ConsoleColor)x);
            Program.DefaultTextColour = (ConsoleColor)GameSettings[9].SettingValue.Number;
            ChosenLine = 0;
        }

        public void PrintMenu(bool higlightName)
        {
            DiffSwitcher.PrintMenuName(higlightName);

            string secondLineText = "Game settings:";
            string thirdLineText = "Use arrow keys to operate and enter to confirm";

            PositionedText secondLine = new PositionedText(secondLineText, ConsoleColor.Black, (Console.WindowWidth - secondLineText.Length) / 2, 1);
            PositionedText thirdLine = new PositionedText(thirdLineText, ConsoleColor.Black, (Console.WindowWidth - thirdLineText.Length) / 2, 2);
            secondLine.Print(false);
            thirdLine.Print(false);
            foreach (GameSetting setting in GameSettings)
                setting.Print(false);
        }

        public virtual ConsoleKey MenuAction()
        {
            ConsoleKey keypressed;
            do
            {
                DiffSwitcher.PrintMenuName(false);
                GameSettings[ChosenLine].Print(true);
                keypressed = Console.ReadKey(true).Key;
                switch (keypressed)
                {
                    case ConsoleKey.UpArrow:
                        if (ChosenLine == 0)
                        {
                            GameSettings[ChosenLine].Print(false);
                            DiffSwitcher.PrintMenuName(true);
                            return keypressed;
                        }
                        else
                        {
                            GameSettings[ChosenLine].Print(false);
                            ChosenLine--;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (ChosenLine != 9)
                        {
                            GameSettings[ChosenLine].Print(false);
                            ChosenLine++;
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        if (GameSettings[ChosenLine].Colour || GameSettings[ChosenLine].TextColour)
                        {
                            Program.TakenColours.Remove((ConsoleColor)GameSettings[ChosenLine].SettingValue.Number);
                            foreach (GameMenu menu in DiffSwitcher.GameMenus)
                                if (menu == null)
                                    continue;
                                else
                                    menu.GameSettings[ChosenLine].ChangeValue(-1, Program.TakenColours, ChosenLine, GameSettings[0].SettingValue.Number * GameSettings[1].SettingValue.Number, GameSettings[2].SettingValue.Number);
                            Program.TakenColours.Add((ConsoleColor)GameSettings[ChosenLine].SettingValue.Number);
                            if (GameSettings[ChosenLine].TextColour)
                            {
                                PrintMenu(false);
                            }
                        }
                        else
                        {
                            GameSetting[] colourSettings = new GameSetting[7];
                            for (int x = 0; x != 7; x++)
                            {
                                colourSettings[x] = GameSettings[x + 3];
                            }
                            DiffSwitcher.GameMenus[3] = new CustomGameMenu(keypressed, colourSettings, ChosenLine);
                            return ConsoleKey.Tab;
                        }
                            
                        break;
                    case ConsoleKey.RightArrow:
                        if (GameSettings[ChosenLine].Colour || GameSettings[ChosenLine].TextColour)
                        {
                            Program.TakenColours.Remove((ConsoleColor)GameSettings[ChosenLine].SettingValue.Number);
                            foreach (GameMenu menu in DiffSwitcher.GameMenus)
                                if (menu == null)
                                    continue;
                                else
                                    menu.GameSettings[ChosenLine].ChangeValue(1, Program.TakenColours, ChosenLine, GameSettings[0].SettingValue.Number * GameSettings[1].SettingValue.Number, GameSettings[2].SettingValue.Number);
                            Program.TakenColours.Add((ConsoleColor)GameSettings[ChosenLine].SettingValue.Number);
                            if (GameSettings[ChosenLine].TextColour)
                            {
                                PrintMenu(false);
                            }
                        }
                        else
                        {
                            GameSetting[] colourSettings = new GameSetting[7];
                            for (int x = 0; x != 7; x++)
                            {
                                colourSettings[x] = GameSettings[x + 3];
                            }
                            DiffSwitcher.GameMenus[3] = new CustomGameMenu(keypressed, colourSettings, ChosenLine);
                            return ConsoleKey.Tab;
                        }
                        break;
                    case ConsoleKey.Enter:
                        return keypressed;
                }
                GameSettings[ChosenLine].Print(true);
                
            } while (true);
        }
    }
    public enum Difficulties
    {
        Easy = 1,
        Medium = 2,
        Hard = 3
    }
}
       
