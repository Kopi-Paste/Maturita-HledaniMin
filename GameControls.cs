using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GloriousMinesweeper
{
    static class GameControls
    {
        public static Game PlayedGame { get; set; }
        public static ChangableCoordinates CurrentMinefieldPosition { get; private set; }
        public static Tile CurrentTile { get; private set; }
        private static PositionedNumber UncoveredTiles { get; set; }
        private static PositionedNumber NumberOfFlags { get; set; }
        private static List<PositionedText> Labels {get; set;}
        private static int IncorrectFlags { get; set; }
        private static Stopwatch CompletionTime { get; set; }

        public static void SetDefault()
        {
            CurrentMinefieldPosition = new ChangableCoordinates(0, 0, PlayedGame.HorizontalTiles - 1, PlayedGame.VerticalTiles - 1);
            UncoveredTiles = new PositionedNumber(0, ConsoleColor.Black, Console.WindowWidth - 6, 3);
            NumberOfFlags = new PositionedNumber(0, ConsoleColor.Black, Console.WindowWidth - 6, 4);
            Labels = new List<PositionedText>();
            CompletionTime = new Stopwatch();
            Labels.Add(new PositionedText("Uncovered Tiles:", ConsoleColor.Black, Console.WindowWidth - 24, 3));
            Labels.Add(new PositionedText("Placed Flags:", ConsoleColor.Black, Console.WindowWidth - 21, 4));
        }
        
        public static bool GameWin(out int score)
        {
            CompletionTime.Stop();
            Console.Clear();
            Labels.Clear();
            Labels.Add(new PositionedText("Congratulations, you swept the mines!", ConsoleColor.Black, 6, 3));
            string time = String.Format("{0:00}:{1:00}:{2:00}.{3:00} ", CompletionTime.Elapsed.Hours, CompletionTime.Elapsed.Minutes, CompletionTime.Elapsed.Seconds, CompletionTime.Elapsed.Milliseconds / 10);
            Labels.Add(new PositionedText(time, ConsoleColor.Black, 6, 4));
            foreach (PositionedText label in Labels)
                label.Print(false);
            score = 10000;
            return true;
        }
        public static bool GameLose(out int score)
        {
            CompletionTime.Stop();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = PlayedGame.Text;
            Console.WriteLine("Mines are victorious.");
            string time = String.Format("{0:00}:{1:00}:{2:00}.{3:00} ",
                CompletionTime.Elapsed.Hours, CompletionTime.Elapsed.Minutes, CompletionTime.Elapsed.Seconds, CompletionTime.Elapsed.Milliseconds / 10);
            Console.WriteLine(time);
            score = 0;
            return false;
        }
        

        public static bool Gameplay(out int score)
        {
            foreach (PositionedText label in Labels)
                label.Print(false);
            CompletionTime.Start();
            do
            {
                if ((UncoveredTiles.Number == PlayedGame.HorizontalTiles * PlayedGame.VerticalTiles - PlayedGame.Mines) || (NumberOfFlags.Number == PlayedGame.Mines && IncorrectFlags == 0))
                    return GameWin(out score);
                CurrentTile = new HighlightedTile(PlayedGame.Minefield[CurrentMinefieldPosition.Horizontal, CurrentMinefieldPosition.Vertical]);
                UncoveredTiles.Print(false);
                NumberOfFlags.Print(false);
                ConsoleKey keypressed = Console.ReadKey(true).Key;
                if (keypressed == ConsoleKey.LeftArrow || keypressed == ConsoleKey.RightArrow || keypressed == ConsoleKey.UpArrow || keypressed == ConsoleKey.DownArrow)
                {
                    if (CurrentTile.Covered)
                        CurrentTile = new CoveredTile(CurrentTile);
                    else
                        CurrentTile = new UncoveredTile(CurrentTile);
                    switch (keypressed)
                    {
                        case ConsoleKey.LeftArrow:
                            CurrentMinefieldPosition.GoLeft();
                            break;
                        case ConsoleKey.RightArrow:
                            CurrentMinefieldPosition.GoRight();
                            break;
                        case ConsoleKey.UpArrow:
                            CurrentMinefieldPosition.GoUp();
                            break;
                        case ConsoleKey.DownArrow:
                            CurrentMinefieldPosition.GoDown();
                            break;
                    }
                }
                else
                {
                    switch (keypressed)
                    {
                        case ConsoleKey.Spacebar:
                            if (CurrentTile.Covered)
                            {
                                NumberOfFlags.ChangeBy(CurrentTile.FlagTile());
                                if (!CurrentTile.Mine && CurrentTile.Flag)
                                    IncorrectFlags += 1;
                                else if (!CurrentTile.Mine && !CurrentTile.Flag)
                                    IncorrectFlags -= 1;
                            }
                            break;
                        case ConsoleKey.Enter:
                            if (!CurrentTile.Flag)
                            {
                                if (UncoveredTiles.Number == 0)
                                {
                                    PlayedGame.MoveMinesOut(CurrentTile, CurrentTile.TilesAround);
                                    CurrentTile = PlayedGame.Minefield[CurrentMinefieldPosition.Horizontal, CurrentMinefieldPosition.Vertical];
                                }
                                if (CurrentTile.Mine)
                                    return GameLose(out score);
                                CurrentTile = new UncoveredTile(CurrentTile);
                                if (CurrentTile.MinesAround == 0)
                                {
                                    int automaticallyUnflaged = 0;
                                    UncoveredTiles.ChangeBy(UncoverTilesAround(CurrentTile));
                                    NumberOfFlags.ChangeBy(automaticallyUnflaged);
                                }
                                else
                                    UncoveredTiles.ChangeBy(1);
                            }
                            break;
                    }
                    PlayedGame.Minefield[CurrentMinefieldPosition.Horizontal, CurrentMinefieldPosition.Vertical] = CurrentTile;
                }
            } while (true);
        }
        public static int UncoverTilesAround(Tile UncoverAround)
        {
            int tilesUncovered = 0;
            foreach (Tile tile in UncoverAround.TilesAround)
            { 
                int currentHorizontal = tile.MinefieldPositon.Horizontal;
                int currentVertical = tile.MinefieldPositon.Vertical;
                if (PlayedGame.Minefield[currentHorizontal, currentVertical].Covered)
                {
                    tilesUncovered += 1;
                    if (PlayedGame.Minefield[currentHorizontal, currentVertical].Flag)
                    {
                        NumberOfFlags.ChangeBy(-1);
                        IncorrectFlags--;
                    }
                    PlayedGame.Minefield[currentHorizontal, currentVertical] =  new UncoveredTile(PlayedGame.Minefield[currentHorizontal, currentVertical]);
                    if (PlayedGame.Minefield[currentHorizontal, currentVertical].MinesAround == 0)
                    {
                        tilesUncovered += UncoverTilesAround(PlayedGame.Minefield[currentHorizontal, currentVertical]);
                    }
                }
            }
            return tilesUncovered;
        }
    }    
}
