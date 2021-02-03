using System;
using System.Collections.Generic;

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

            DiffSwitcher.SwitchTo(0, true);
            
            int[] gameParameteres = DiffSwitcher.EnableSwitch();
            Console.Clear();
            PositionedText loadingSign = new PositionedText("Loading...", ConsoleColor.Black, (Console.WindowWidth - 10) / 2, 12);
            loadingSign.Print(false);
            bool gameWon;
            GameControls.PlayedGame = new Game(gameParameteres);
            GameControls.PlayedGame.TilesAndMinesAroundCalculator();
            Console.Clear();
            GameControls.PlayedGame.PrintMinefield();
            GameControls.SetDefault();
            gameWon = GameControls.Gameplay(out int score);
            Console.Write("Your score: " + score);
        }
        public static ConsoleColor DefaultTextColour { get; set; }
        public static List<ConsoleColor> TakenColours { get; set; }
    }
    
}
