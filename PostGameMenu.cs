using System;
using System.Collections.Generic;
using System.IO;

namespace GloriousMinesweeper
{
    class PostGameMenu
    {

        private bool GameWon { get; }
        private decimal Score { get; }
        private bool IsHighscore { get; }
        private string Nickname { get; set; }
        private List<PositionedText> Labels { get; }
        private List<PositionedText> SwitchableLabels { get; }
        private int ChosenLabel { get; set; }
        private int Position { get; set; }
        string Path { get; set; }
        public PostGameMenu(decimal score, bool won, out bool UserWantsToPlayAgain)
        {
            GameWon = won;
            Score = score;
            Position = 1;
            Labels = new List<PositionedText>();
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
            if (won)
                IsHighscore = CheckHighscores();
            else
                IsHighscore = false;
            if (IsHighscore)
            {
                Labels.Add(new PositionedText("Save Highscore", ConsoleColor.Black, (Console.WindowWidth - 14) / 2, 9));
                SwitchableLabels.Add(new PositionedText("Save Highscore", ConsoleColor.Black, (Console.WindowWidth - 14) / 2, 9));
            }
            else
                SwitchableLabels.Add(null);
            Labels.Add(new PositionedText("Play again", ConsoleColor.Black, (Console.WindowWidth - 10) / 2, 11));
            SwitchableLabels.Add(new PositionedText("Play again", ConsoleColor.Black, (Console.WindowWidth - 10) / 2, 11));
            Labels.Add(new PositionedText("Play again with the same parameters", ConsoleColor.Black, (Console.WindowWidth - 35) / 2, 13));
            SwitchableLabels.Add(new PositionedText("Play again with the same parameters", ConsoleColor.Black, (Console.WindowWidth - 35) / 2, 13));
            Labels.Add(new PositionedText("Quit", ConsoleColor.Black, (Console.WindowWidth - 4) / 2, 15));
            SwitchableLabels.Add(new PositionedText("Quit", ConsoleColor.Black, (Console.WindowWidth - 4) / 2, 15));
            
            ChosenLabel = 1;
            UserWantsToPlayAgain = EnableSwitch();
        }
        private bool CheckHighscores()
        {
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
                    File.Create(Path);
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
                    string[] parts = line.Split('\t');
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
        private bool EnableSwitch()
        {
            foreach (PositionedText label in Labels)
            {
                if (label == null)
                    continue;
                label.Print(false);
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
                }
            } while (keypressed != ConsoleKey.Enter);
            switch (ChosenLabel)
            {
                case 0:
                    SwitchableLabels[0].Print(false);
                    SaveHighscore();
                    SwitchableLabels[0] = null;
                    Labels[2] = null;
                    ChosenLabel = 1;
                    return EnableSwitch();
                case 1:
                    DiffSwitcher.SwitchTo(0, true);
                    foreach (GameMenu menu in DiffSwitcher.GameMenus)
                    {
                        if (menu == null)
                            continue;
                        menu.ChooseLine(0);
                    }
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
                    return false;
            }
            return false;
        }
        private void SaveHighscore()
        {
            PositionedText nickname = new PositionedText("Enter your Nickname: ", ConsoleColor.Black, (Console.WindowWidth - 40) / 2, 17);
            nickname.Print(true);
            Console.CursorVisible = true;
            Nickname = Console.ReadLine();
            Console.CursorVisible = false;
            try
            {
                List<string> currentLeaderboard = new List<string>(File.ReadAllLines(Path));
                if (currentLeaderboard.Count == Position - 1)
                {
                    currentLeaderboard.Add (Nickname + "\t" + Score);
                }
                else
                {
                    int x = currentLeaderboard.Count + 1;
                    if (currentLeaderboard.Count != 10)
                        currentLeaderboard.Add(null);
                    else
                        currentLeaderboard[9] = null;
                    while (x != Position)
                    {
                        currentLeaderboard[x - 1] = currentLeaderboard[x - 2];
                        x--;
                    }
                    currentLeaderboard[Position - 1] = Nickname + "\t" + Score;
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
            Console.ReadKey();
            Console.Clear();
            return;
        }
    }
}
