using System;
using System.Collections.Generic;
using System.IO;

namespace GloriousMinesweeper
{
    class Program
    {
        private static int ChosenLabel { get; set; }
        static void Main()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            DefaultTextColour = ConsoleColor.White;
            TakenColours = new List<ConsoleColor>();
            Console.CursorVisible = false;
            Console.WriteLine("Please fullscreen");
            Console.WriteLine("Alt+Enter is highly recommended");
            Console.WriteLine("It is not recommended to Alt+Tab during the game or to un-fullscreen the game");
            while (((Console.LargestWindowWidth - 5) > Console.WindowWidth) || ((Console.LargestWindowHeight - 3) > Console.WindowHeight))
            {}
            while (Console.KeyAvailable)
                Console.ReadKey(true);
            Console.CursorVisible = false;
            DefaultTextColour = ConsoleColor.Gray;
            PositionedText PlayGame = new PositionedText("Play Minesweeper", ConsoleColor.Black, Console.WindowWidth / 2 - 8, 10);
            PositionedText ShowHighscores = new PositionedText("See Highscores", ConsoleColor.Black, Console.WindowWidth / 2 - 7, 12);
            PositionedText Quit = new PositionedText("Quit", ConsoleColor.Black, Console.WindowWidth / 2 - 2, 14);
            Border MainMenuSmallBorder = new Border(Console.WindowWidth / 2 - 10, 8, 10, 20, ConsoleColor.Black, ConsoleColor.White, false);
            Border MainMenuBigBorder = new Border(0, 1, Console.WindowHeight - 1, Console.WindowWidth, ConsoleColor.Black, ConsoleColor.Gray, false);
            ChosenLabel = 0;
            List<IGraphic> Labels = new List<IGraphic> { PlayGame, ShowHighscores, Quit, MainMenuSmallBorder, MainMenuBigBorder };
            ConsoleKey keypressed = 0;
            Console.Clear();
            for (int x = 3; x < 5; x++)
            {
                Labels[x].Print(x == 4);
            }
            while (true)
            {
                for (int x = 0; x < 3; x++)
                {
                    Labels[x].Print(x == ChosenLabel);
                }
                keypressed = Console.ReadKey(true).Key;
                switch (keypressed)
                {
                    case ConsoleKey.UpArrow:
                        if (ChosenLabel != 0)
                            ChosenLabel--;
                        break;
                    case ConsoleKey.DownArrow:
                        if (ChosenLabel != 2)
                            ChosenLabel++;
                        break;
                    case ConsoleKey.Enter:
                        switch (ChosenLabel)
                        {
                            case 0:
                                DiffSwitcher.StartMenu();
                                Console.BackgroundColor = ConsoleColor.Black;
                                Console.Clear();
                                for (int x = 3; x < 5; x++)
                                {
                                    Labels[x].Print(x == 4);
                                }
                                break;
                            case 1:
                                ShowLeaderboards();
                                for (int x = 3; x < 5; x++)
                                {
                                    Labels[x].Print(x == 4);
                                }
                                break;
                            case 2:
                                Environment.Exit(0);
                                break;
                        }
                        break;
                    case ConsoleKey.R:
                        try
                        {
                            Console.Clear();
                            for (int x = 0; x < 5; x++)
                            {
                                Labels[x].Print(x == 4 || x == ChosenLabel);
                            }
                        }
                        catch
                        {
                            WaitForFix();
                            Console.Clear();
                            for (int x = 0; x < 5; x++)
                            {
                                Labels[x].Print(x == 4 || x == ChosenLabel);
                            }
                        }
                        break;
                }
            }

            /*DiffSwitcher.GameMenus = new GameMenu[4];
            DiffSwitcher.GameMenus[0] = new GameMenu("Easy", Difficulties.Easy);
            DiffSwitcher.GameMenus[1] = new GameMenu("Medium", Difficulties.Medium);
            DiffSwitcher.GameMenus[2] = new GameMenu("Hard", Difficulties.Hard);
            DiffSwitcher.GameMenus[3] = null;
            DiffSwitcher.ChosenMenu = 0;
            DiffSwitcher.SetDefault();
            DiffSwitcher.SwitchTo(0, true);
            
            GameControls.PlayedGame = DiffSwitcher.EnableSwitch();
            Console.Clear();
            bool gameWon;
            bool UserWantsToPlayAgain;
            do
            {
                GameControls.PlayedGame.PrintMinefield();
                GameControls.PlayedGame.TilesAndMinesAroundCalculator();
                GameControls.SetDefault();
                gameWon = GameControls.Gameplay(out decimal score, out SpecialisedStopwatch playTime);
                if (score != -1)
                    UserWantsToPlayAgain = PostGameMenu.ShowMenu(score, gameWon, playTime);
                else
                    UserWantsToPlayAgain = false;
            } while (UserWantsToPlayAgain);*/
        }
        public static List<ConsoleColor> TakenColours { get; set; }
        public static ConsoleColor DefaultTextColour { get; set; }
        public static void ShowLeaderboards()
        {
            Console.Clear();
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Minesweeper", "highscores.txt");
            if (!File.Exists(path))
            {
                (new PositionedText("No saved scores yet.", ConsoleColor.Black, Console.WindowWidth / 2 - 10, 10)).Print(false);
                (new Border(0, 0, Console.WindowHeight, Console.WindowWidth, ConsoleColor.Black, ConsoleColor.Gray, false)).Print(true);
                Console.ReadKey(true);
                Console.Clear();
                return;
            }
            string[] leaderboards = File.ReadAllLines(path);
            Console.ForegroundColor = DefaultTextColour;
            Console.CursorVisible = false;
            Border HighscoresBigBorder = new Border(0, 0, Console.WindowHeight, Console.WindowWidth, ConsoleColor.Black, ConsoleColor.Gray, false);
            Border HighscoresSmallBorder = new Border(Console.WindowWidth / 2 - 34, 9, leaderboards.Length + 4, 68, ConsoleColor.Black, ConsoleColor.Gray, false);
            HighscoresBigBorder.Print(true);
            HighscoresSmallBorder.Print(true);
            for (int x = 1; x <= leaderboards.Length; x++)
            {
                string toPrint = x.ToString() + ".   " + leaderboards[x - 1];
                new PositionedText(toPrint, ConsoleColor.Black, (Console.WindowWidth - toPrint.Length) / 2, 10 + x).Print(false);
            }
            Console.ReadKey(true);
            Console.Clear();
            return;
        }
        public static void WaitForFix()
        {
            Console.SetCursorPosition(0, 0);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine("Please fullscreen using Alt+Enter");
            while (((Console.LargestWindowWidth - 5) > Console.WindowWidth) || ((Console.LargestWindowHeight - 3) > Console.WindowHeight))
            { }
            while (Console.KeyAvailable)
                Console.ReadKey(true);
        }
    }
    
}
