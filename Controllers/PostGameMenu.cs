using System;
using System.Collections.Generic;
using System.IO;

namespace GloriousMinesweeper
{
    class PostGameMenu
    {

        private static bool GameWon { get; set; }
        private static decimal Score { get; set; }
        private static bool IsHighscore { get; set; }
        private static string Nickname { get; set; }
        private static List<IGraphic> Labels { get; set; }
        private static List<PositionedText> SwitchableLabels { get; set; }
        private static int ChosenLabel { get; set; }
        private static int Position { get; set; }
        private static string Path { get; set; }
        public static bool ShowMenu(decimal score, bool won, SpecialisedStopwatch PlayTime)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
            GameWon = won;
            Score = score;
            Position = 1;
            Labels = new List<IGraphic>();
            SwitchableLabels = new List<PositionedText>();
            string firstMessage;
            if (won)
                firstMessage = "Congratulations, you've swept the mines.";
            else
                firstMessage = "Mines are victorious";
            Labels.Add(new PositionedText(firstMessage, ConsoleColor.Black, (Console.WindowWidth - firstMessage.Length) / 2, 5));
            if (won)
            {
                string secondMessage = "Your score is " + Score;
                Labels.Add(new PositionedText(secondMessage, ConsoleColor.Black, (Console.WindowWidth - secondMessage.Length) / 2, 7));
            }
            Labels.Add(new Border(0, 1, Console.WindowHeight - 1, Console.WindowWidth, ConsoleColor.Black, ConsoleColor.Gray, false));
            if (won)
                Labels.Add(new Border((Console.WindowWidth - firstMessage.Length) / 2 - 3, 3, 17, 46, ConsoleColor.Black, ConsoleColor.Gray, false));
            else
                Labels.Add(new Border((Console.WindowWidth - 35) / 2 - 3, 3, 17, 41, ConsoleColor.Black, ConsoleColor.Gray, false));
            string Time = "Your time: " + PlayTime.ToString();
            Labels.Add(new PositionedText(Time, ConsoleColor.Black, (Console.WindowWidth - Time.Length) / 2, 9));
            if (won)
                IsHighscore = CheckHighscores();
            else
                IsHighscore = false;
            if (IsHighscore)
            {
                Labels.Add(new PositionedText("Save Highscore", ConsoleColor.Black, (Console.WindowWidth - 14) / 2, 11));
                SwitchableLabels.Add(new PositionedText("Save Highscore", ConsoleColor.Black, (Console.WindowWidth - 14) / 2, 11));
            }
            else if (!GameWon)
            {
                Labels.Add(new PositionedText("View minefield", ConsoleColor.Black, (Console.WindowWidth - 14) / 2, 11));
                SwitchableLabels.Add(new PositionedText("View minefield", ConsoleColor.Black, (Console.WindowWidth - 14) / 2, 11));
            }
            else
            {
                Labels.Add(null);
                SwitchableLabels.Add(null);
            }
            Labels.Add(new PositionedText("Play again", ConsoleColor.Black, (Console.WindowWidth - 10) / 2, 13));
            SwitchableLabels.Add(new PositionedText("Play again", ConsoleColor.Black, (Console.WindowWidth - 10) / 2, 13));
            Labels.Add(new PositionedText("Play again with the same parameters", ConsoleColor.Black, (Console.WindowWidth - 35) / 2, 15));
            SwitchableLabels.Add(new PositionedText("Play again with the same parameters", ConsoleColor.Black, (Console.WindowWidth - 35) / 2, 15));
            Labels.Add(new PositionedText("Quit", ConsoleColor.Black, (Console.WindowWidth - 4) / 2, 17));
            SwitchableLabels.Add(new PositionedText("Quit", ConsoleColor.Black, (Console.WindowWidth - 4) / 2, 17));

            ChosenLabel = 1;
            return EnableSwitch();
        }
        private static bool CheckHighscores()
        {
            if (Score == 0)
                return false;
            Path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Minesweeper");
            try
            {
                if (!Directory.Exists(Path))
                    Directory.CreateDirectory(Path);
            }
            catch
            {
                Console.WriteLine($"Floder {Path} does not exist and can not be created, please check your acces-rights.");
                Console.WriteLine("Your score can not be saved.");
                return false;
            }
            Path = System.IO.Path.Combine(Path, "highscores.txt");
            try
            {
                if (!File.Exists(Path))
                    File.Create(Path).Dispose();
            }
            catch
            {
                Console.WriteLine($"File {Path} does not exist and can not be created, please check your acces-rights.");
                Console.WriteLine("Your score can not be saved.");
                return false;
            }
            try
            {
                string[] allLines = File.ReadAllLines(Path);
                foreach (string line in allLines)
                {
                    string[] parts = line.Split("   ");
                    if (Score < Decimal.Parse(parts[1]))
                        Position++;
                    else
                        return true;
                }
                if (Position == 11)
                    return false;
                else
                    return true;
            }
            catch (IndexOutOfRangeException)
            {
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine($"File {Path} can not be reached.");
                Console.WriteLine(e.Message);
                Console.WriteLine("Your score can not be saved.");
                return false;
            }
        }
        private static bool EnableSwitch()
        {
            foreach (IGraphic label in Labels)
            {
                if (label == null)
                    continue;
                label.Print(label.GetType() == (typeof(Border)));
            }
            for (int x = 0; x < 4; x++)
            {
                if (SwitchableLabels[x] == null)
                    continue;
                SwitchableLabels[x].Print(x == ChosenLabel);
            }
            ConsoleKey keypressed;
            do
            {
                for (int x = 0; x < 4; x++)
                {
                    if (SwitchableLabels[x] == null)
                        continue;
                    SwitchableLabels[x].Print(x == ChosenLabel);
                }
                keypressed = Console.ReadKey(true).Key;
                switch (keypressed)
                {
                    case ConsoleKey.UpArrow:
                        if (ChosenLabel != 0)
                            if (SwitchableLabels[ChosenLabel - 1] != null)
                                ChosenLabel--;
                        break;
                    case ConsoleKey.DownArrow:
                        if (ChosenLabel != 3)
                            ChosenLabel++;
                        break;
                    case ConsoleKey.R:
                        try
                        {
                            foreach (IGraphic label in Labels)
                            {
                                if (label == null)
                                    continue;
                                label.Print(label.GetType() == (typeof(Border)));
                            }
                            for (int x = 0; x < 4; x++)
                            {
                                if (SwitchableLabels[x] == null)
                                    continue;
                                SwitchableLabels[x].Print(x == ChosenLabel);
                            }
                        }
                        catch
                        {
                            Program.WaitForFix();
                            foreach (IGraphic label in Labels)
                            {
                                if (label == null)
                                    continue;
                                label.Print(label.GetType() == (typeof(Border)));
                            }
                            for (int x = 0; x < 4; x++)
                            {
                                if (SwitchableLabels[x] == null)
                                    continue;
                                SwitchableLabels[x].Print(x == ChosenLabel);
                            }
                        }
                        break;
                }
            } while (keypressed != ConsoleKey.Enter);
            switch (ChosenLabel)
            {
                case 0:
                    SwitchableLabels[0].Print(false);
                    if (GameWon)
                    {
                        SaveHighscore();
                        SwitchableLabels[0] = null;
                        Labels[5] = null;
                        ChosenLabel = 1;
                    }
                    else
                    {
                        GameControls.PlayedGame.PrintMinefield(true);
                        Console.ReadKey();
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Clear();
                    }
                    return EnableSwitch();
                case 1:
                    foreach (GameMenu menu in DiffSwitcher.GameMenus)
                    {
                        if (menu == null)
                            continue;
                        menu.ChooseLine(0);
                    }
                    Console.Clear();
                    DiffSwitcher.PrintGraphics(true);
                    DiffSwitcher.SwitchTo(DiffSwitcher.ChosenMenu, true);
                    GameControls.PlayedGame = DiffSwitcher.EnableSwitch();
                    return true;
                case 2:
                    int[] parameters = new int[10];
                    for (int x = 0; x != 10; x++)
                    {
                        if (x <= 2)
                            parameters[x] = DiffSwitcher.GameMenus[DiffSwitcher.ChosenMenu].GameSettings[x].SettingValue.Number;
                        else
                            parameters[x] = DiffSwitcher.Colours[x - 3].SettingValue.Number;
                        GameControls.PlayedGame = new Game(parameters);
                    }
                    return true;
                case 3:
                    Environment.Exit(0);
                    break;
            }
            return false;
        }
        private static void SaveHighscore()
        {
            PositionedText nickname = new PositionedText("Enter your Nickname: ", ConsoleColor.Black, (Console.WindowWidth - 40) / 2, 21);
            nickname.Print(true);
            do
            {
                Console.CursorVisible = true;
                Console.SetCursorPosition(Console.WindowWidth / 2 + 1, 21);
                Console.Write(new string(' ', 50));
                Console.SetCursorPosition(Console.WindowWidth / 2 + 1, 21);
                Nickname = Console.ReadLine();
                if (Nickname.Contains("   ") || Nickname.Length > 50 || Nickname == "")
                {
                    (new PositionedText("Invalid Nickname!", ConsoleColor.Black, Console.WindowWidth / 2 - 9, 23)).Print(false);
                    Nickname = "";
                }
            } while (Nickname == "");
            Console.CursorVisible = false;
            try
            {
                List<string> currentLeaderboard = new List<string>(File.ReadAllLines(Path));
                if (currentLeaderboard.Count == Position - 1)
                {
                    currentLeaderboard.Add (Nickname + "   " + Score);
                }
                else
                {
                    currentLeaderboard.Insert(Position - 1, Nickname + "   " + Score);
                    if (currentLeaderboard.Count == 11)
                        currentLeaderboard = currentLeaderboard.GetRange(0, 10);
                }
                File.WriteAllLines(Path, currentLeaderboard);
            }
            catch (Exception e)
            {
                Console.WriteLine($"The file {Path} couldn't be changed. Please check your acces-rights");
                Console.WriteLine("Your score couldn't be saved");
                Console.WriteLine(e.Message);
            }
            Program.ShowLeaderboards();
            return;
        }
    }
}
