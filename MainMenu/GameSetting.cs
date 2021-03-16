using System;
using System.Collections.Generic;

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
            text += ":  ";
            ConsoleColor background;
            if (colourable)
                background = (ConsoleColor)defaultValue;
            else
                background = ConsoleColor.Black;
            Colour = colourable;
            TextColour = textColour;
            Setting = new PositionedText(text, background, ((Console.WindowWidth - text.Length) / 2), line);
            SettingValue = new PositionedNumber(defaultValue, background, ((Console.WindowWidth + text.Length) / 2), line);
        }


        public void ChangeValue(int change, int chosenLine, int tiles, int mines)
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
                    SettingValue.ChangeTo(0);
                    ChangeValue(1, chosenLine, tiles, mines);
                    return;
                }
                else if (SettingValue.Number + change <= 0)
                {
                    SettingValue.ChangeTo(15);
                    ChangeValue(-1, chosenLine, tiles, mines);
                    return;
                }
                SettingValue.ChangeBy(change);
                if (Colour)
                {
                    Setting.ColourChangeTo(SettingValue.Number);
                    SettingValue.ColourChangeTo(SettingValue.Number);
                }
                else if (TextColour)
                    Program.DefaultTextColour = (ConsoleColor)SettingValue.Number;
                Print(true);
            }
            else if (Setting.Text.EndsWith("tiles"))
            {
                int otherValue = tiles / SettingValue.Number;
                if (SettingValue.Number + change >= 4 && SettingValue.Number + change <= 50 && (SettingValue.Number + change) * otherValue >= (mines + 20))
                    SettingValue.ChangeBy(change);
            }
            else
            {
                if ((SettingValue.Number + change) >= 2 && (SettingValue.Number + change) <= tiles - 20)
                    SettingValue.ChangeBy(change);
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
        public void Print(bool highlight)
        {
            Setting.Print(highlight);
            if (Colour || TextColour)
                SettingValue.PrintWithConsoleColourEnum(highlight);
            else
                SettingValue.Print(highlight);
        }


    }
}
