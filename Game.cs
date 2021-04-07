using System;
using System.Collections.Generic;

namespace GloriousMinesweeper
{
    class Game
    {
        public int HorizontalTiles { get; private set; }
        public int VerticalTiles { get; private set; }

        public int Mines { get; }

        public ConsoleColor Cover { get; private set; }
        public ConsoleColor CoverSecondary { get; private set; }
        
        public ConsoleColor Uncover { get; private set; }

        public ConsoleColor UncoverSecondary { get; private set; }
        public ConsoleColor Highlight { get; private set; }
        public ConsoleColor Flag { get; private set; }
        public ConsoleColor Text { get; private set; }

        public Tile[,] Minefield { get; set; } 


        public Game(int[] parameters)
        {
            HorizontalTiles = parameters[0];
            VerticalTiles = parameters[1];
            Mines = parameters[2];
            Cover = (ConsoleColor)parameters[3];
            CoverSecondary = (ConsoleColor)parameters[4];
            Uncover = (ConsoleColor)parameters[5];
            UncoverSecondary = (ConsoleColor)parameters[6];
            Flag = (ConsoleColor)parameters[7];
            Highlight = (ConsoleColor)parameters[8];
            Text = (ConsoleColor)parameters[9];
            Minefield = new Tile[HorizontalTiles, VerticalTiles];

            int remainingMines = Mines;
            int remainingTiles = HorizontalTiles * VerticalTiles;
            ConsoleColor currentColour;
            Random rng = new Random();
            bool mine;

            for (int x = 0; x != HorizontalTiles; x++)
            {
                if (x % 2 == 0)
                    currentColour = Cover;
                else
                    currentColour = CoverSecondary;

                    for (int y = 0; y != VerticalTiles; y++)
                    {
                        
                        
                        int mineDeterminator = rng.Next(remainingTiles);
                        if (mineDeterminator < remainingMines)
                        {
                            mine = true;
                            remainingMines--;
                            remainingTiles--;
                        }
                        else
                        {
                            mine = false;
                            remainingTiles--;
                        }
                        
                       
                        Minefield[x, y] = new CoveredTile(mine, currentColour, (Console.WindowWidth/2 - HorizontalTiles+2*x), y + ((Console.WindowHeight - VerticalTiles) / 2), x, y);
                        if (currentColour == Cover)
                            currentColour = CoverSecondary;
                        else
                            currentColour = Cover;
                    }
            }
        }
        public Game(string[] savedGame)
        {
            string[] parameters = savedGame[3].Split(',');
            HorizontalTiles = int.Parse(parameters[0]);
            VerticalTiles = int.Parse(parameters[1]);
            Mines = int.Parse(parameters[2]);
            Cover = (ConsoleColor)int.Parse(parameters[3]);
            CoverSecondary = (ConsoleColor)int.Parse(parameters[4]);
            Uncover = (ConsoleColor)int.Parse(parameters[5]);
            UncoverSecondary = (ConsoleColor)int.Parse(parameters[6]);
            Flag = (ConsoleColor)int.Parse(parameters[7]);
            Highlight = (ConsoleColor)int.Parse(parameters[8]);
            Text = (ConsoleColor)int.Parse(parameters[9]);
            Program.DefaultTextColour = Text;
            Minefield = new Tile[HorizontalTiles, VerticalTiles];
            ConsoleColor currentColour;
            DiffSwitcher.SetLoaded(parameters);
            //GameControls.PlayedGame.Resize(HorizontalTiles, VerticalTiles);

            for (int x = 0; x != HorizontalTiles; x++)
            {
                if (x % 2 == 0)
                    currentColour = Cover;
                else
                    currentColour = CoverSecondary;

                for (int y = 0; y != VerticalTiles; y++)
                {
                    Minefield[x, y] = new CoveredTile(false, currentColour, (Console.WindowWidth / 2 - HorizontalTiles + 2 * x), y + 3, x, y);
                    //if (x == 0 && y == 0)
                      //  GameControls.PlayedGame.Minefield[0, 0] = Minefield[0, 0];
                    if (currentColour == Cover)
                        currentColour = CoverSecondary;
                    else
                        currentColour = Cover;
                }
            }
            string[] uncovered = savedGame[2].Split(';');
            foreach (string coordinates in uncovered)
            {
                if (coordinates != "")
                {
                    string[] position = coordinates.Split(',');
                    Minefield[int.Parse(position[0]), int.Parse(position[1])] = new UncoveredTile(Minefield[int.Parse(position[0]), int.Parse(position[1])], false);
                }
            }
            string[] flagged = savedGame[0].Split(';');
            foreach (string coordinates in flagged)
            {
                if (coordinates != "")
                {
                    string[] position = coordinates.Split(',');
                    Minefield[int.Parse(position[0]), int.Parse(position[1])].FlagTile();
                }
            }
            string[] mines = savedGame[1].Split(';');
            foreach (string coordinates in mines)
            {
                if (coordinates != "")
                {
                    string[] position = coordinates.Split(',');
                    Minefield[int.Parse(position[0]), int.Parse(position[1])].PlantMine();
                }
            }
            //string[] time = savedGame[4].Split(';');
            //GameControls.SetLoaded(int.Parse(parameters[10]), int.Parse(parameters[11]), int.Parse(parameters[12]), decimal.Parse(parameters[13]), time[0], time[1]);

        }
        public void TilesAndMinesAroundCalculator()
        {
            foreach (Tile tile in Minefield)
                tile.TilesAroundCalculator();
            foreach (Tile tile in Minefield)
                tile.MinesAroundCalculator();
        }

        public void PrintMinefield(bool flagMines = false)
        {
            Console.BackgroundColor = 0;
            Console.Clear();
            foreach (Tile tile in Minefield)
            {
                if (flagMines)
                {
                    if (tile.Mine && !tile.Flag)
                        tile.FlagTile();
                    else if (!tile.Mine && tile.Flag)
                        tile.FlagTile();
                }
                tile.PrintTile();
            }
        }
        public void MoveMinesOut(Tile selectedTile, List<Tile> forbbidenTiles)
        {
            int clearedMines = 0;
            if (selectedTile.Mine)
            {
                Minefield[selectedTile.MinefieldPosition.Horizontal, selectedTile.MinefieldPosition.Vertical].ClearMine();
                clearedMines++;
            }
            foreach (Tile tile in forbbidenTiles)
            {
                if (tile.Mine)
                {
                    Minefield[tile.MinefieldPosition.Horizontal, tile.MinefieldPosition.Vertical].ClearMine();
                    clearedMines++;
                }
            }
            while (clearedMines != 0)
            {
                Random rnd = new Random();
                int horizontalPosition = 0;
                int verticalPosition = 0;
                do
                {
                    horizontalPosition = rnd.Next(HorizontalTiles);
                }
                while (Math.Abs(horizontalPosition - selectedTile.MinefieldPosition.Horizontal) <= 1);
                do
                {
                    verticalPosition = rnd.Next(VerticalTiles);
                }
                while (Math.Abs(verticalPosition - selectedTile.MinefieldPosition.Vertical) <= 1);
                if (!Minefield[horizontalPosition, verticalPosition].Mine)
                {
                    Minefield[horizontalPosition, verticalPosition].PlantMine();
                    clearedMines--;
                }
            }
            TilesAndMinesAroundCalculator();
        }
        /*private void Resize(int NewHorizontal, int NewVertical)
        {
            HorizontalTiles = NewHorizontal;
            VerticalTiles = NewVertical;
        }*/
        public int[] GetParameters()
        {
            int[] Parameters = new int[10];
            Parameters[0] = HorizontalTiles;
            Parameters[1] = VerticalTiles;
            Parameters[2] = Mines;
            Parameters[3] = (int)Cover;
            Parameters[4] = (int)CoverSecondary;
            Parameters[5] = (int)Uncover;
            Parameters[6] = (int)UncoverSecondary;
            Parameters[7] = (int)Flag;
            Parameters[8] = (int)Highlight;
            Parameters[9] = (int)Text;
            return Parameters;
        }
        public override string ToString()
        {
            string ToString = HorizontalTiles.ToString() + ',' + VerticalTiles.ToString() + ',' + Mines.ToString() + ',' + ((int)Cover).ToString() + ',' + ((int)CoverSecondary).ToString() + ',' + ((int)Uncover).ToString() + ',' + ((int)UncoverSecondary).ToString() + ',' + ((int)Flag).ToString() + ',' + ((int)Highlight).ToString() + ',' + ((int)Text).ToString() + ',' + GameControls.UncoveredTiles.ToString() + ',' + GameControls.NumberOfFlags.ToString() + ',' + GameControls.IncorrectFlags.ToString() + ',' + GameControls.ScoreMultiplier.ToString();
            return ToString;
        }
    }
}
