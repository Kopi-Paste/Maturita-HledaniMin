using System;

namespace GloriousMinesweeper
{
    class GameSetting
    {

        public PositionedText Setting { get; private set; }
        public PositionedNumber SettingValue { get; private set; }
        public bool Colour { get; }
        public bool TextColour { get; }

        public GameSetting(string text, int defaultValue, bool colourable, bool textColour, int line)
        {
            text += ": ";
            ConsoleColor background;
            if (colourable)
                background = (ConsoleColor)defaultValue;
            else
                background = ConsoleColor.Black;
            Colour = colourable;
            TextColour = textColour;
            Setting = new PositionedText(text, background, ((Console.WindowWidth - (text.Length + 8)) / 2), line);
            SettingValue = new PositionedNumber(defaultValue, background, ((Console.WindowWidth + text.Length - 8) / 2), line);
        }


        public void ChangeValue(int change, int chosenLine, int tiles, int mines, Action Reprint)
        {
            if (Colour || TextColour)
            {
                while (Program.TakenColours.Contains((ConsoleColor)SettingValue.Number + change))
                {
                    if (change < 0)
                        change--;
                    else
                        change++;
                }
                if (SettingValue.Number + change >= 15)
                {
                    SettingValue.ChangeTo(0, Reprint);
                    ChangeValue(1, chosenLine, tiles, mines, Reprint);
                    return;
                }
                else if (SettingValue.Number + change <= 0)
                {
                    SettingValue.ChangeTo(15, Reprint);
                    ChangeValue(-1, chosenLine, tiles, mines, Reprint);
                    return;
                }
                SettingValue.ChangeBy(change, Reprint);
                if (Colour)
                {
                    Setting.ChangeColour(SettingValue.Number);
                    SettingValue.ChangeColour(SettingValue.Number);
                }
                else if (TextColour)
                {
                    Program.DefaultTextColour = (ConsoleColor)SettingValue.Number;
                    DiffSwitcher.GameMenus[DiffSwitcher.ChosenMenu].PrintMenu(false);
                }
                Print(true, Reprint);
            }
            else if (Setting.Text.EndsWith("tiles: "))
            {
                int otherValue = tiles / SettingValue.Number;
                if (((SettingValue.Number + change) < 4 || (SettingValue.Number + change) > 50) || (((SettingValue.Number + change) * otherValue) < (mines + 20)))
                { }
                else if ((Setting.Text == "Number of horizontal tiles: ") && (SettingValue.Number + change) > (Console.WindowWidth - (2 * 56)))
                { }
                else if ((Setting.Text == "Number of vertical tiles: ") && (SettingValue.Number + change) > (Console.WindowHeight - 4))
                { }
                else
                    SettingValue.ChangeBy(change, Reprint);
                /*int otherValue = tiles / SettingValue.Number;
                if ((SettingValue.Number + change) >= 4 && (SettingValue.Number + change) <= 50 && (SettingValue.Number + change) * otherValue >= (mines + 20))
                    SettingValue.ChangeBy(change);*/
            }
            else
            {
                if ((SettingValue.Number + change) < 2 || (SettingValue.Number + change) > (tiles - 20))
                { }
                else
                    SettingValue.ChangeBy(change, Reprint);
                /*if ((SettingValue.Number + change) >= 2 && (SettingValue.Number + change) <= (tiles - 20))
                    SettingValue.ChangeBy(change);*/
            }
            /*if (Colour || TextColour)
            {
                while (takenColours.Contains((ConsoleColor)SettingValue.Number + change))
                {
                    if (change < 0)
                        change--;
                    else
                        change++;
                }
                if ((SettingValue.Number + change) >= 15)
                {
                    SettingValue.ChangeTo(0);
                    ChangeValue(1, takenColours, chosenLine, tiles, mines);
                }
                else if ((SettingValue.Number + change) <= 0)
                {
                    SettingValue.ChangeTo(15);
                    ChangeValue(-1, takenColours, chosenLine, tiles, mines);
                }
                else
                    SettingValue.ChangeBy(change);
                if (Colour)
                {
                    Setting.ColourChangeTo(SettingValue.Number);
                    SettingValue.ColourChangeTo(SettingValue.Number);
                }
                else
                    Program.DefaultTextColour = (ConsoleColor)SettingValue.Number;
                Print(true);
            }
            else if (chosenLine == 0 || chosenLine == 1)
            {
                int otherValue = tiles / SettingValue.Number;
                if (((SettingValue.Number + change) < 4 || (SettingValue.Number + change) > 50) || (((SettingValue.Number + change) * otherValue) < (mines + 20)))
                { }
                else
                    SettingValue.ChangeBy(change);
            }
            else
            {
                if ((SettingValue.Number + change) < 2 || (SettingValue.Number + change) > (tiles - 20))
                { }
                else
                    SettingValue.ChangeBy(change);
            }*/
        }
        public void ChangeValueTo(int newValue, Action Reprint)
        {
            if (!Colour && !TextColour)
                return;
            SettingValue.ChangeTo(newValue, Reprint);
            if (Colour)
            {
                Setting.ChangeColour(newValue);
                SettingValue.ChangeColour(newValue);
            }
            else
            {
                Program.DefaultTextColour = (ConsoleColor)newValue;
            }

        }
        public void Print(bool highlight, Action Reprint)
        {
            Setting.Print(highlight, Reprint);
            if (Colour || TextColour)
                SettingValue.PrintWithConsoleColourEnum(highlight, Reprint);
            else
                SettingValue.Print(highlight, Reprint);
        }


    }
}
