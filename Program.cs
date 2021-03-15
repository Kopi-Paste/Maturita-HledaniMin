using System;
using System.Collections.Generic;
using System.IO;

namespace GloriousMinesweeper
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Console.BackgroundColor = ConsoleColor.Black;
            DefaultTextColour = ConsoleColor.White;
                
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            

            GameMenu easy = new GameMenu("Easy", Difficulties.Easy);
            GameMenu medium = new GameMenu("Medium", Difficulties.Medium);
            GameMenu hard = new GameMenu("Hard", Difficulties.Hard);
            DiffSwitcher.GameMenus = new GameMenu[4];
            DiffSwitcher.GameMenus[0] = easy;
            DiffSwitcher.GameMenus[1] = medium;
            DiffSwitcher.GameMenus[2] = hard;
            DiffSwitcher.GameMenus[3] = null;
            DiffSwitcher.ChosenMenu = 0;
            
            GameControls.PlayedGame = DiffSwitcher.EnableSwitch();
            Console.Clear();
            bool gameWon;
            bool UserWantsToPlayAgain = false;
            do
            {
                GameControls.PlayedGame.PrintMinefield();
                GameControls.PlayedGame.TilesAndMinesAroundCalculator();
                GameControls.SetDefault();
                gameWon = GameControls.Gameplay(out decimal score);
                PostGameMenu postGameMenu = new PostGameMenu(score, gameWon, out UserWantsToPlayAgain);
            } while (UserWantsToPlayAgain);
        }
        public static ConsoleColor DefaultTextColour { get; set; }
        public static List<ConsoleColor> TakenColours { get; set; }
        public static void ShowLeaderboards()
        {
            Console.Clear();
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Minesweeper", "highscores.txt");
            string[] leaderboards = File.ReadAllLines(path);
            for (int x = 1; x <= leaderboards.Length; x++)
            {
                string toPrint = x.ToString() + ".\t" + leaderboards[x - 1];
                Console.WriteLine(toPrint);
            }
            return;
        }
    }
    
}
