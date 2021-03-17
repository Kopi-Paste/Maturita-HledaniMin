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
            TakenColours = new List<ConsoleColor>();
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            
            DiffSwitcher.GameMenus = new GameMenu[4];
            DiffSwitcher.GameMenus[0] = new GameMenu("Easy", Difficulties.Easy);
            DiffSwitcher.GameMenus[1] = new GameMenu("Medium", Difficulties.Medium);
            DiffSwitcher.GameMenus[2] = new GameMenu("Hard", Difficulties.Hard);
            DiffSwitcher.GameMenus[3] = null;
            DiffSwitcher.ChosenMenu = 0;
            DiffSwitcher.SetDefaultColours();
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
                gameWon = GameControls.Gameplay(out decimal score);
                UserWantsToPlayAgain = PostGameMenu.ShowMenu(score, gameWon);
            } while (UserWantsToPlayAgain);
        }
        public static List<ConsoleColor> TakenColours { get; set; }
        public static ConsoleColor DefaultTextColour { get; set; }
        public static void ShowLeaderboards()
        {
            Console.Clear();
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Minesweeper", "highscores.txt");
            string[] leaderboards = File.ReadAllLines(path);
            Console.ForegroundColor = DefaultTextColour;
            Console.CursorVisible = false;
            for (int x = 1; x <= leaderboards.Length; x++)
            {
                string toPrint = x.ToString() + ".\t" + leaderboards[x - 1];
                Console.WriteLine(toPrint);
            }
            return;
        }
    }
    
}
