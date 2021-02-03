using System;

namespace GloriousMinesweeper
{
    class CustomGameMenu : GameMenu
    {
        public CustomGameMenu(ConsoleKey keypressed, GameSetting[] colours, int chosenLine) : base("Custom", DiffSwitcher.GameMenus[DiffSwitcher.ChosenMenu].Difficulty)
        {
            ChosenLine = chosenLine;
            DiffSwitcher.GameMenus[3] = this;
            DiffSwitcher.SwitchTo(3, false);
            if (keypressed == ConsoleKey.LeftArrow)
                GameSettings[chosenLine].ChangeValue(-1, Program.TakenColours, ChosenLine, GameSettings[0].SettingValue.Number * GameSettings[1].SettingValue.Number, GameSettings[2].SettingValue.Number);
            else if (keypressed == ConsoleKey.RightArrow)
                GameSettings[chosenLine].ChangeValue(1, Program.TakenColours, ChosenLine, GameSettings[0].SettingValue.Number * GameSettings[1].SettingValue.Number, GameSettings[2].SettingValue.Number);
            for (int x = 3; x != 10; x++)
            {
                GameSettings[x] = colours[x - 3];
            }
            Program.DefaultTextColour = (ConsoleColor)GameSettings[9].SettingValue.Number;
            Console.Clear();
            PrintMenu(false);   
        }

        public override ConsoleKey MenuAction()
        {
            ConsoleKey keypressed;
            do
            {
                GameSettings[ChosenLine].Print(true);
                keypressed = Console.ReadKey(true).Key;
                switch (keypressed)
                {
                    case ConsoleKey.UpArrow:
                        if (ChosenLine == 0)
                        {
                            GameSettings[ChosenLine].Print(false);
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
                            GameSettings[ChosenLine].ChangeValue(-1, Program.TakenColours, ChosenLine, GameSettings[0].SettingValue.Number * GameSettings[1].SettingValue.Number, GameSettings[2].SettingValue.Number);

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
                            GameSettings[ChosenLine].ChangeValue(1, Program.TakenColours, ChosenLine, GameSettings[0].SettingValue.Number * GameSettings[1].SettingValue.Number, GameSettings[2].SettingValue.Number);
                        break;
                    case ConsoleKey.Enter:
                        return keypressed;
                }
                GameSettings[ChosenLine].Print(true);
            } while (true);
        }

    }
}
