using System;

namespace GloriousMinesweeper
{
    class CustomGameMenu : GameMenu
    {
        public CustomGameMenu(ConsoleKey keypressed, int chosenLine) : base("Custom", DiffSwitcher.GameMenus[DiffSwitcher.ChosenMenu].Difficulty)
        {
            ChosenLine = chosenLine;
            if (keypressed == ConsoleKey.LeftArrow)
                GameSettings[chosenLine].ChangeValue(-1, ChosenLine, GameSettings[0].SettingValue.Number * GameSettings[1].SettingValue.Number, GameSettings[2].SettingValue.Number);
            else if (keypressed == ConsoleKey.RightArrow)
                GameSettings[chosenLine].ChangeValue(1, ChosenLine, GameSettings[0].SettingValue.Number * GameSettings[1].SettingValue.Number, GameSettings[2].SettingValue.Number);
            Console.Clear();
            PrintMenu(false);   
        }

        public override int MenuAction()
        {
            ConsoleKey keypressed;
            do
            {
                PrintMenu(false);
                if (ChosenLine <= 2)
                    GameSettings[ChosenLine].Print(true);
                else
                    DiffSwitcher.Colours[ChosenLine - 3].Print(true);
                keypressed = Console.ReadKey(true).Key;
                switch (keypressed)
                {
                    case ConsoleKey.UpArrow:
                        if (ChosenLine == 0)
                        {
                            GameSettings[ChosenLine].Print(false);
                            //DiffSwitcher.PrintMenuName(true);
                            return 1;
                        }
                        else
                        {
                            if (ChosenLine <= 2)
                                GameSettings[ChosenLine].Print(false);
                            else
                                DiffSwitcher.Colours[ChosenLine - 3].Print(false);
                            ChosenLine--; 
                            break;
                        }
                    case ConsoleKey.DownArrow:
                        if (ChosenLine != 9)
                        {
                            if (ChosenLine <= 2)
                                GameSettings[ChosenLine].Print(false);
                            else
                                DiffSwitcher.Colours[ChosenLine - 3].Print(false);
                            ChosenLine++;
                            break;
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        if (ChosenLine > 2)
                        {
                            Program.TakenColours.Remove((ConsoleColor)DiffSwitcher.Colours[ChosenLine - 3].SettingValue.Number);
                            DiffSwitcher.Colours[ChosenLine - 3].ChangeValue(-1, ChosenLine, GameSettings[0].SettingValue.Number * GameSettings[1].SettingValue.Number, GameSettings[2].SettingValue.Number);
                            Program.TakenColours.Add((ConsoleColor)DiffSwitcher.Colours[ChosenLine - 3].SettingValue.Number);
                        }
                        else
                        {
                            GameSettings[ChosenLine].ChangeValue(-1, ChosenLine, GameSettings[0].SettingValue.Number * GameSettings[1].SettingValue.Number, GameSettings[2].SettingValue.Number);
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        if (ChosenLine > 2)
                        {
                            Program.TakenColours.Remove((ConsoleColor)DiffSwitcher.Colours[ChosenLine - 3].SettingValue.Number);
                            DiffSwitcher.Colours[ChosenLine - 3].ChangeValue(1, ChosenLine, GameSettings[0].SettingValue.Number * GameSettings[1].SettingValue.Number, GameSettings[2].SettingValue.Number);
                            Program.TakenColours.Add((ConsoleColor)DiffSwitcher.Colours[ChosenLine - 3].SettingValue.Number);
                        }
                        else
                        {
                            GameSettings[ChosenLine].ChangeValue(1, ChosenLine, GameSettings[0].SettingValue.Number * GameSettings[1].SettingValue.Number, GameSettings[2].SettingValue.Number);
                        }
                        break;
                    case ConsoleKey.Enter:
                        return 0;

                }
                /*ConsoleKey keypressed;
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
                                return (int)keypressed;
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
                                for(int x = 0; x < 4; x++)
                                        DiffSwitcher.GameMenus[x].GameSettings[ChosenLine].ChangeValue(-1, Program.TakenColours, ChosenLine, GameSettings[0].SettingValue.Number * GameSettings[1].SettingValue.Number, GameSettings[2].SettingValue.Number);
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
                                for (int x = 0; x < 4; x++)
                                    DiffSwitcher.GameMenus[x].GameSettings[ChosenLine].ChangeValue(1, Program.TakenColours, ChosenLine, GameSettings[0].SettingValue.Number * GameSettings[1].SettingValue.Number, GameSettings[2].SettingValue.Number);
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
                            return (int)keypressed;
                    }
                    GameSettings[ChosenLine].Print(true);*/
            } while (true);

        }
    }
}
