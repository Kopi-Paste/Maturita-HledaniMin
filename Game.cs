using System;
using System.Collections.Generic;

namespace GloriousMinesweeper
{
    class Game
    {
        public int HorizontalTiles { get; }
        public int VerticalTiles { get; }

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
                        
                       
                        Minefield[x, y] = new CoveredTile(mine, currentColour, 2*x, y, x, y);
                        if (currentColour == Cover)
                            currentColour = CoverSecondary;
                        else
                            currentColour = Cover;
                    }
            }
        }
        public void TilesAndMinesAroundCalculator()
        {
            foreach (Tile tile in Minefield)
                tile.TilesAroundCalculator();
            foreach (Tile tile in Minefield)
                tile.MinesAroundCalculator();
        }

        public void PrintMinefield()
        {
            Console.Clear();
            foreach (Tile tile in Minefield)
                tile.PrintTile();
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
    }
}
