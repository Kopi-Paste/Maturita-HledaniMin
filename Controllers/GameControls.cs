using System;
using System.Collections.Generic;

namespace GloriousMinesweeper
{
    static class GameControls
    {
        public static Game PlayedGame { get; set; }
        public static ChangableCoordinates CurrentMinefieldPosition { get; private set; }
        public static Tile CurrentTile { get; private set; }
        private static bool EndGame { get; set; }
        public static PositionedNumber UncoveredTiles { get; private set; }
        public static PositionedNumber NumberOfFlags { get; private set; }
        private static List<IGraphic> Labels {get; set;}
        public static int IncorrectFlags { get; private set; }
        public static SpecialisedStopwatch CompletionTime { get; set; }
        public static decimal ScoreMultiplier { get; private set; }
        private static bool GameAborted { get; set; }
        public static void SetDefault()
        {
            CurrentMinefieldPosition = new ChangableCoordinates(0, 0, PlayedGame.HorizontalTiles - 1, PlayedGame.VerticalTiles - 1);
            CurrentTile = PlayedGame.Minefield[0, 0];
            IncorrectFlags = 0;
            UncoveredTiles = new PositionedNumber(0, ConsoleColor.Black, Console.WindowWidth - 34, 6);
            NumberOfFlags = new PositionedNumber(0, ConsoleColor.Black, Console.WindowWidth - 34, 7);
            Labels = new List<IGraphic>();
            CompletionTime = new SpecialisedStopwatch("0", "0");
            EndGame = false;
            GameAborted = false;
            ScoreMultiplier = 1;
            Labels.Add(new Border(0, 1, Console.WindowHeight - 1, Console.WindowWidth, ConsoleColor.Black, ConsoleColor.Gray, false));
            Labels.Add(new Border(new Coordinates(PlayedGame.Minefield[0, 0].Position, -2, -1), PlayedGame.VerticalTiles + 2, 2 * (PlayedGame.HorizontalTiles + 2), ConsoleColor.Black, ConsoleColor.White, false));
            Labels.Add(new Border(Console.WindowWidth - 54, 5, 4, 23 + (int)Math.Floor(Math.Log10((PlayedGame.HorizontalTiles * PlayedGame.VerticalTiles - PlayedGame.Mines))), ConsoleColor.Black, ConsoleColor.Gray, false));
            Labels.Add(new PositionedText("Uncovered Tiles:", ConsoleColor.Black, Console.WindowWidth - 52, 6));
            Labels.Add(new PositionedText("Placed Flags:", ConsoleColor.Black, Console.WindowWidth - 52, 7));
            Labels.Add(new Border(6, 16, 10, 22, PlayedGame.Uncover, PlayedGame.Cover, true));
            Labels.Add(new Border(6, 28, 10, 22, PlayedGame.UncoverSecondary, PlayedGame.CoverSecondary, true));
            Labels.Add(new Border(31, 16, 10, 22, PlayedGame.Uncover, PlayedGame.Cover, true));
            Labels.Add(new Border(31, 28, 10, 22, PlayedGame.UncoverSecondary, PlayedGame.CoverSecondary, true));
            Labels.Add(new Border(Console.WindowWidth - 54, 16, 27, 50, PlayedGame.Uncover, PlayedGame.Cover, true));
            Labels.Add(new PositionedText("Uncover all tiles", PlayedGame.Uncover, 8, 18));
            Labels.Add(new PositionedText("without mines and", PlayedGame.Uncover, 8, 19));
            Labels.Add(new PositionedText("flag all tiles", PlayedGame.Uncover, 8, 20));
            Labels.Add(new PositionedText("with them!", PlayedGame.Uncover, 8, 21));
            Labels.Add(new PositionedText("Numbers on tiles", PlayedGame.UncoverSecondary, 8, 30));
            Labels.Add(new PositionedText("indicate how many", PlayedGame.UncoverSecondary, 8, 31));
            Labels.Add(new PositionedText("mines are around.", PlayedGame.UncoverSecondary, 8, 32));
            Labels.Add(new PositionedText("Beaware, if you", PlayedGame.Uncover, 33, 18));
            Labels.Add(new PositionedText("try to uncover", PlayedGame.Uncover, 33, 19));
            Labels.Add(new PositionedText("tile with mine", PlayedGame.Uncover, 33, 20));
            Labels.Add(new PositionedText("you will lose.", PlayedGame.Uncover, 33, 21));
            Labels.Add(new PositionedText("If you have a tile", PlayedGame.UncoverSecondary, 33, 30));
            Labels.Add(new PositionedText("marked by a flag", PlayedGame.UncoverSecondary, 33, 31));
            Labels.Add(new PositionedText("you will not be", PlayedGame.UncoverSecondary, 33, 32));
            Labels.Add(new PositionedText("able to uncover", PlayedGame.UncoverSecondary, 33, 33));
            Labels.Add(new PositionedText("the tile.", PlayedGame.UncoverSecondary, 33, 34));
            Labels.Add(new PositionedText("Game controls:", PlayedGame.Uncover, Console.WindowWidth - 52, 18));
            Labels.Add(new PositionedText("Use arrow keys to move around", PlayedGame.Uncover, Console.WindowWidth - 52, 20));
            Labels.Add(new PositionedText("Use Enter to uncover tile", PlayedGame.Uncover, Console.WindowWidth - 52, 22));
            Labels.Add(new PositionedText("First uncover is 100 % safe", PlayedGame.Uncover, Console.WindowWidth - 52, 23));
            Labels.Add(new PositionedText("Use Spacebar to flag/unflag tile", PlayedGame.Uncover, Console.WindowWidth - 52, 25));
            Labels.Add(new PositionedText("Need help? Use H to get a hint", PlayedGame.Uncover, Console.WindowWidth - 52, 27));
            Labels.Add(new PositionedText("Warning: Doing so in cases when it", PlayedGame.Uncover, Console.WindowWidth - 52, 28));
            Labels.Add(new PositionedText("is not needed will lower your score", PlayedGame.Uncover, Console.WindowWidth - 52, 29));
            Labels.Add(new PositionedText("Or you can use S to let the game solve itself", PlayedGame.Uncover, Console.WindowWidth - 52, 31));
            Labels.Add(new PositionedText("Or Q to solve itself very very quickly", PlayedGame.Uncover, Console.WindowWidth - 52, 32));
            Labels.Add(new PositionedText("You can also use Escape to pause the game", PlayedGame.Uncover, Console.WindowWidth - 52, 34));
            Labels.Add(new PositionedText("or to stop auto-solve", PlayedGame.Uncover, Console.WindowWidth - 52, 35));
            Labels.Add(new PositionedText("Try to solve the game as quickly as possible", PlayedGame.Uncover, Console.WindowWidth - 52, 37));
            Labels.Add(new PositionedText("to achieve the highest score.", PlayedGame.Uncover, Console.WindowWidth - 52, 38));
            Labels.Add(new PositionedText("Good Luck!", PlayedGame.Uncover, Console.WindowWidth - 52, 40));
        }
        
        public static bool GameWin(out decimal score, out SpecialisedStopwatch playTime)
        {
            CompletionTime.Stop();
            /*Console.Clear();
            Labels.Clear();
            Labels.Add(new PositionedText("Congratulations, you swept the mines!", ConsoleColor.Black, 6, 3));
            string time = String.Format("{0:00}:{1:00}:{2:00}.{3:00} ", CompletionTime.Elapsed.Hours, CompletionTime.Elapsed.Minutes, CompletionTime.Elapsed.Seconds, CompletionTime.Elapsed.Milliseconds / 10);
            Labels.Add(new PositionedText(time, ConsoleColor.Black, 6, 4));
            foreach (PositionedText label in Labels)
                label.Print(false);*/
            score = Math.Round(1000 * ScoreMultiplier * PlayedGame.HorizontalTiles * PlayedGame.VerticalTiles * PlayedGame.Mines * PlayedGame.Mines / CompletionTime.ElapsedMilliseconds, 5);
            playTime = CompletionTime;
            return true;
        }
        public static bool GameLose(out decimal score, out SpecialisedStopwatch playTime)
        {
            CompletionTime.Stop();
            /*Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = PlayedGame.Text;
            Console.WriteLine("Mines are victorious.");
            string time = String.Format("{0:00}:{1:00}:{2:00}.{3:00} ",
                CompletionTime.Elapsed.Hours, CompletionTime.Elapsed.Minutes, CompletionTime.Elapsed.Seconds, CompletionTime.Elapsed.Milliseconds / 10);
            Console.WriteLine(time);*/
            score = 0;
            playTime = CompletionTime;
            return false;
        }
        

        public static bool Gameplay(out decimal score, out SpecialisedStopwatch playTime)
        {
            for (int x = 0; x < Labels.Count; x++)
                Labels[x].Print(x < 2);
            CompletionTime.Start();
            do
            {
                EndGame = GameTurn();
            } while (!EndGame);
            if ((UncoveredTiles.Number == PlayedGame.HorizontalTiles * PlayedGame.VerticalTiles - PlayedGame.Mines) || (NumberOfFlags.Number == PlayedGame.Mines && IncorrectFlags == 0))
                return GameWin(out score, out playTime);
            else if (GameAborted)
            {
                score = -1;
                playTime = CompletionTime;
                return false;
            }
            else
                return GameLose(out score, out playTime);

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
            if (UncoveredTiles.Number == (PlayedGame.HorizontalTiles * PlayedGame.VerticalTiles - PlayedGame.Mines) || (NumberOfFlags.Number == PlayedGame.Mines && IncorrectFlags == 0))
                return true;
            CurrentTile = new HighlightedTile(PlayedGame.Minefield[CurrentMinefieldPosition.Horizontal, CurrentMinefieldPosition.Vertical]);
            UncoveredTiles.Print(false);
            NumberOfFlags.Print(false);
            ConsoleKey keypressed = Console.ReadKey(true).Key;
            return GameAction(keypressed);
        }
        public static bool GameTurn(ConsoleKey keypressed)
        {
            if (UncoveredTiles.Number == (PlayedGame.HorizontalTiles * PlayedGame.VerticalTiles - PlayedGame.Mines) || (NumberOfFlags.Number == PlayedGame.Mines && IncorrectFlags == 0))
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
                CurrentTile = new UncoveredTile(CurrentTile, true);
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
                    case ConsoleKey.Escape:
                        CompletionTime.Stop();
                        bool unpause = PauseGame();
                        if (unpause)
                        {
                            PlayedGame.PrintMinefield();
                            Labels[2] = (new Border(Console.WindowWidth - 54, 5, 4, 23 + (int)Math.Floor(Math.Log10((PlayedGame.HorizontalTiles * PlayedGame.VerticalTiles - PlayedGame.Mines))), ConsoleColor.Black, ConsoleColor.Gray, false));
                            Labels[5] = new Border(6, 16, 10, 22, PlayedGame.Uncover, PlayedGame.Cover, true);
                            Labels[6] = new Border(6, 28, 10, 22, PlayedGame.UncoverSecondary, PlayedGame.CoverSecondary, true);
                            Labels[7] = new Border(31, 16, 10, 22, PlayedGame.Uncover, PlayedGame.Cover, true);
                            Labels[8] = new Border(31, 28, 10, 22, PlayedGame.UncoverSecondary, PlayedGame.CoverSecondary, true);
                            Labels[9] = new Border(Console.WindowWidth - 54, 16, 27, 50, PlayedGame.Uncover, PlayedGame.Cover, true);
                            for (int x = 0; x < Labels.Count; x++)
                            {
                                if ((x > 9 && x < 14) || (x > 16 && x < 21) || (x > 25))
                                    Labels[x].ChangeColour((int)PlayedGame.Uncover);
                                else if (x > 10)
                                    Labels[x].ChangeColour((int)PlayedGame.UncoverSecondary);
                                Labels[x].Print(x < 2);
                            }
                            CompletionTime.Start();
                        }
                        else
                        {
                            GameAborted = true;
                            return true;
                        }
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
                            CurrentTile = new UncoveredTile(CurrentTile, true);
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
                    case ConsoleKey.R:
                        try
                        {
                            PlayedGame.PrintMinefield();
                            for (int x = 0; x < Labels.Count; x++)
                                Labels[x].Print(x < 2);
                        }
                        catch
                        {
                            Program.WaitForFix();
                            PlayedGame.PrintMinefield();
                            for (int x = 0; x < Labels.Count; x++)
                                Labels[x].Print(x < 2);
                        }
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
                    PlayedGame.Minefield[currentHorizontal, currentVertical] =  new UncoveredTile(PlayedGame.Minefield[currentHorizontal, currentVertical], true);
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
            do
            {
                while (!Console.KeyAvailable && !EndGame)
                {
                    if (!Hint(quick))
                        ScoreMultiplier /= 2;
                }
                if (EndGame)
                    break;
            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
            return;
        }
        public static bool Hint(bool quick)
        {
            if (UncoveredTiles.Number == PlayedGame.HorizontalTiles * PlayedGame.VerticalTiles || (NumberOfFlags.Number == PlayedGame.Mines && IncorrectFlags == 0))
            {
                EndGame = true;
                return true;
            }
            if (IncorrectFlags != 0)
            {
                foreach (Tile tile in PlayedGame.Minefield)
                {
                    if (tile.Flag && !tile.Mine)
                    {
                        NavigateToTile(tile, quick);
                        EndGame = GameTurn(ConsoleKey.Spacebar);
                        return false;
                    }
                }
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
                if (tile.Covered && tile.Mine && !tile.Flag && (CountCoveredAround(tile) != tile.TilesAround.Count || CountFlagsAround(tile) != 0))
                {
                    NavigateToTile(tile, quick);
                    EndGame = GameTurn(ConsoleKey.Spacebar);
                    return true;
                }
                if (tile.Covered && !tile.Mine && (CountCoveredAround(tile) != tile.TilesAround.Count || CountFlagsAround(tile) != 0))
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
        public static bool PauseGame()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
            return PauseMenu.PauseGameMenu();
        }
        public static void SetLoaded(string[] SavedGame)
        {
            string[] parameters = SavedGame[3].Split(',');
            string[] time = SavedGame[4].Split(';');
            CurrentMinefieldPosition = new ChangableCoordinates(0, 0, PlayedGame.HorizontalTiles - 1, PlayedGame.VerticalTiles - 1);
            CurrentTile = PlayedGame.Minefield[0, 0];
            UncoveredTiles.ChangeTo(Int32.Parse(parameters[10]));
            NumberOfFlags.ChangeTo(Int32.Parse(parameters[11]));
            IncorrectFlags = Int32.Parse(parameters[12]);
            ScoreMultiplier = Decimal.Parse(parameters[13]);
            CompletionTime = new SpecialisedStopwatch(time[0], time[1]);
            Labels[1] = new Border(new Coordinates(PlayedGame.Minefield[0, 0].Position, -2, -1), PlayedGame.VerticalTiles + 2, 2 * (PlayedGame.HorizontalTiles + 2), ConsoleColor.Black, ConsoleColor.White, false);
        }
    }
}  