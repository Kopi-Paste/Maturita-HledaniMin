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
        private static bool EndGame { get; set; }
        private static PositionedNumber UncoveredTiles { get; set; }
        private static PositionedNumber NumberOfFlags { get; set; }
        private static List<PositionedText> Labels {get; set;}
        private static int IncorrectFlags { get; set; }
        private static Stopwatch CompletionTime { get; set; }
        private static decimal ScoreMultiplier { get; set; }

        public static void SetDefault()
        {
            CurrentMinefieldPosition = new ChangableCoordinates(0, 0, PlayedGame.HorizontalTiles - 1, PlayedGame.VerticalTiles - 1);
            CurrentTile = PlayedGame.Minefield[0, 0];
            IncorrectFlags = 0;
            UncoveredTiles = new PositionedNumber(0, ConsoleColor.Black, Console.WindowWidth - 6, 3);
            NumberOfFlags = new PositionedNumber(0, ConsoleColor.Black, Console.WindowWidth - 6, 4);
            Labels = new List<PositionedText>();
            CompletionTime = new Stopwatch();
            EndGame = false;
            ScoreMultiplier = 1;
            Labels.Add(new PositionedText("Uncovered Tiles:", ConsoleColor.Black, Console.WindowWidth - 24, 3));
            Labels.Add(new PositionedText("Placed Flags:", ConsoleColor.Black, Console.WindowWidth - 21, 4));
        }
        
        public static bool GameWin(out decimal score)
        {
            CompletionTime.Stop();
            Console.Clear();
            Labels.Clear();
            Labels.Add(new PositionedText("Congratulations, you swept the mines!", ConsoleColor.Black, 6, 3));
            string time = String.Format("{0:00}:{1:00}:{2:00}.{3:00} ", CompletionTime.Elapsed.Hours, CompletionTime.Elapsed.Minutes, CompletionTime.Elapsed.Seconds, CompletionTime.Elapsed.Milliseconds / 10);
            Labels.Add(new PositionedText(time, ConsoleColor.Black, 6, 4));
            foreach (PositionedText label in Labels)
                label.Print(false);
            score = 1000 * ScoreMultiplier * PlayedGame.HorizontalTiles * PlayedGame.VerticalTiles * PlayedGame.Mines * PlayedGame.Mines / CompletionTime.ElapsedMilliseconds;
            return true;
        }
        public static bool GameLose(out decimal score)
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
        

        public static bool Gameplay(out decimal score)
        {
            foreach (PositionedText label in Labels)
                label.Print(false);
            CompletionTime.Start();
            do
            {
                EndGame = GameTurn();
            } while (!EndGame);
            if ((UncoveredTiles.Number == PlayedGame.HorizontalTiles * PlayedGame.VerticalTiles - PlayedGame.Mines) || (NumberOfFlags.Number == PlayedGame.Mines && IncorrectFlags == 0))
                return GameWin(out score);
            else
                return GameLose(out score);

            /*do
            {
                if (endGame)
                    return GameLose(out score);
                if ((UncoveredTiles.Number == PlayedGame.HorizontalTiles * PlayedGame.VerticalTiles - PlayedGame.Mines) || (NumberOfFlags.Number == PlayedGame.Mines && IncorrectFlags == 0))
                    return GameWin(out score);
                CurrentTile = new HighlightedTile(PlayedGame.Minefield[CurrentMinefieldPosition.Horizontal, CurrentMinefieldPosition.Vertical]);
                UncoveredTiles.Print(false);
                NumberOfFlags.Print(false);
                ConsoleKey keypressed = Console.ReadKey(true).Key;
                endGame = GameAction(keypressed);
            } while (true);*/
        }
        public static bool GameTurn()
        {
            if (UncoveredTiles.Number == PlayedGame.HorizontalTiles * PlayedGame.VerticalTiles || (NumberOfFlags.Number == PlayedGame.Mines && IncorrectFlags == 0))
                return true;
            CurrentTile = new HighlightedTile(PlayedGame.Minefield[CurrentMinefieldPosition.Horizontal, CurrentMinefieldPosition.Vertical]);
            UncoveredTiles.Print(false);
            NumberOfFlags.Print(false);
            ConsoleKey keypressed = Console.ReadKey(true).Key;
            return GameAction(keypressed);
        }
        public static bool GameTurn(ConsoleKey keypressed)
        {
            if (UncoveredTiles.Number == PlayedGame.HorizontalTiles * PlayedGame.VerticalTiles || (NumberOfFlags.Number == PlayedGame.Mines && IncorrectFlags == 0))
                return true;
            CurrentTile = new HighlightedTile(PlayedGame.Minefield[CurrentMinefieldPosition.Horizontal, CurrentMinefieldPosition.Vertical]);
            UncoveredTiles.Print(false);
            NumberOfFlags.Print(false);
            return GameAction(keypressed);
        }
        public static bool GameAction(ConsoleKey keypressed)
        {
            if (CurrentTile.Covered)
                CurrentTile = new CoveredTile(CurrentTile);
            else
                CurrentTile = new UncoveredTile(CurrentTile);
            if (keypressed == ConsoleKey.LeftArrow || keypressed == ConsoleKey.RightArrow || keypressed == ConsoleKey.UpArrow || keypressed == ConsoleKey.DownArrow)
            {
                
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
                CurrentTile = new HighlightedTile(PlayedGame.Minefield[CurrentMinefieldPosition.Horizontal, CurrentMinefieldPosition.Vertical]);
            }
            else
            {
                switch (keypressed)
                {
                    case ConsoleKey.H:
                        if (Hint(false))
                        { }
                        else
                            ScoreMultiplier /= 2;
                        break;
                    case ConsoleKey.S:
                        Solve(false);
                        break;
                    case ConsoleKey.Q:
                        Solve(true);
                        break;
                    case ConsoleKey.Spacebar:
                        if (CurrentTile.Covered)
                        {
                            NumberOfFlags.ChangeBy(CurrentTile.FlagTile());
                            if (!CurrentTile.Mine && CurrentTile.Flag)
                                IncorrectFlags += 1;
                            else if (!CurrentTile.Mine && !CurrentTile.Flag)
                                IncorrectFlags -= 1;
                        }
                        PlayedGame.Minefield[CurrentMinefieldPosition.Horizontal, CurrentMinefieldPosition.Vertical] = CurrentTile;
                        break;
                    case ConsoleKey.Enter:
                        if (CurrentTile.Covered && !CurrentTile.Flag)
                        {
                            if (UncoveredTiles.Number == 0)
                            {
                                PlayedGame.MoveMinesOut(CurrentTile, CurrentTile.TilesAround);
                                CurrentTile = PlayedGame.Minefield[CurrentMinefieldPosition.Horizontal, CurrentMinefieldPosition.Vertical];
                            }
                            if (CurrentTile.Mine)
                                return true;
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
                        PlayedGame.Minefield[CurrentMinefieldPosition.Horizontal, CurrentMinefieldPosition.Vertical] = CurrentTile;
                        break;
                }
            }
            return false;
        }
        
        public static int UncoverTilesAround(Tile UncoverAround)
        {
            int tilesUncovered = 0;
            for (int x = 0; x < UncoverAround.TilesAround.Count; x++)
            {
                int currentHorizontal = UncoverAround.TilesAround[x].MinefieldPosition.Horizontal;
                int currentVertical = UncoverAround.TilesAround[x].MinefieldPosition.Vertical;
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
        public static int CountCoveredAround(Tile tile)
        {
            int coveredAround = 0;
            foreach (Tile tileAround in tile.TilesAround)
            {
                if (tileAround.Covered)
                    coveredAround++;
            }
            return coveredAround;
        }
        public static int CountFlagsAround(Tile tile)
        {
            int flagsAround = 0;
            foreach (Tile tileAround in tile.TilesAround)
            {
                if (tileAround.Flag)
                    flagsAround++;
            }
            if (flagsAround == 0)
                return -1;
            else
                return flagsAround;
        }
        public static void Solve(bool quick)
        {
            while (!EndGame)
                if (Hint(quick))
                { }
                else
                    ScoreMultiplier /= 2;
            Console.Write("Solve finished");
            return;
        }
        public static bool Hint(bool quick)
        {
            if (UncoveredTiles.Number == PlayedGame.HorizontalTiles * PlayedGame.VerticalTiles || (NumberOfFlags.Number == PlayedGame.Mines && IncorrectFlags == 0))
            {
                EndGame = true;
                return true;
            }
            if (UncoveredTiles.Number == 0)
            {
                EndGame = GameTurn(ConsoleKey.Enter);
                return false;
            }
            
            foreach (Tile tile in PlayedGame.Minefield)
            {
                
                if (tile.Covered)
                    continue;
                tile.TilesAroundCalculator();
                if (CountCoveredAround(tile) == tile.MinesAround)
                {
                    for (int x = 0; x < tile.TilesAround.Count; x++)
                    {
                        Tile potentialFlagTile = tile.TilesAround[x];
                        if (!potentialFlagTile.Flag && potentialFlagTile.Covered)
                        {
                            NavigateToTile(potentialFlagTile, quick);
                            EndGame = GameTurn(ConsoleKey.Spacebar);

                            return false;

                        }
                    }
                }
                if (CountFlagsAround(tile) == tile.MinesAround)
                {
                    for (int x = 0; x < tile.TilesAround.Count; x++)
                    {
                        Tile potentialUncoverTile = tile.TilesAround[x];
                        if (potentialUncoverTile.Covered && !potentialUncoverTile.Flag)
                        {
                            NavigateToTile(potentialUncoverTile, quick);
                            EndGame = GameTurn(ConsoleKey.Enter);
                            return false;
                        }
                    }
                }
            }
            foreach (Tile tile in PlayedGame.Minefield)
            {
                if (tile.Covered && tile.Mine && !tile.Flag)
                {
                    NavigateToTile(tile, quick);
                    EndGame = GameTurn(ConsoleKey.Spacebar);
                    return true;
                }
                if (tile.Covered && !tile.Mine)
                {
                    NavigateToTile(tile, quick);
                    EndGame = GameTurn(ConsoleKey.Enter);
                    return true;
                }    
            }
            return true;
        }
        
        public static void NavigateToTile(Tile tile, bool quick)
        {
            if (tile.MinefieldPosition.Horizontal > CurrentMinefieldPosition.Horizontal)
            {
                while (tile.MinefieldPosition.Horizontal != CurrentMinefieldPosition.Horizontal)
                {
                    EndGame = GameTurn(ConsoleKey.RightArrow);
                    if (!quick)
                        System.Threading.Thread.Sleep(50);
                }
            }
            else
            {
                while (tile.MinefieldPosition.Horizontal != CurrentMinefieldPosition.Horizontal)
                {
                    EndGame = GameTurn(ConsoleKey.LeftArrow);
                    if (!quick)
                        System.Threading.Thread.Sleep(50);
                }
            }
            if (tile.MinefieldPosition.Vertical > CurrentMinefieldPosition.Vertical)
            {
                while (tile.MinefieldPosition.Vertical != CurrentMinefieldPosition.Vertical)
                {
                    EndGame = GameTurn(ConsoleKey.DownArrow);
                    if (!quick)
                        System.Threading.Thread.Sleep(50);
                }
            }
            else
            {
                while (tile.MinefieldPosition.Vertical != CurrentMinefieldPosition.Vertical)
                {
                    EndGame = GameTurn(ConsoleKey.UpArrow);
                    if (!quick)
                        System.Threading.Thread.Sleep(50);
                }
            }

            return;
        }
    }
}    

