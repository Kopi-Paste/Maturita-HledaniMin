using System;
using System.Collections.Generic;

namespace GloriousMinesweeper
{
    abstract class Tile
    {
        public int MinesAround { get; protected set; }
        public bool Covered { get; protected set; }
        public bool Flag { get; protected set; }
        public bool Mine { get; protected set; }
        public ConsoleColor Color { get; protected set; }
        public ConsoleColor OriginalColor { get; protected set; }
        public List<Tile> TilesAround { get; set; }
        public Coordinates Position { get; protected set; }
        public Coordinates MinefieldPosition { get; protected set; }

        

        public void TilesAroundCalculator()
        {
            if (TilesAround == null)
                TilesAround = new List<Tile>();
            TilesAround.Clear();
            int horizontal = MinefieldPosition.Horizontal;
            int vertical = MinefieldPosition.Vertical;
            if (horizontal != 0)
            {
                TilesAround.Add(GameControls.PlayedGame.Minefield[horizontal - 1, vertical]);
                if (vertical != 0)
                {
                    TilesAround.Add(GameControls.PlayedGame.Minefield[horizontal - 1, vertical - 1]);
                    TilesAround.Add(GameControls.PlayedGame.Minefield[horizontal, vertical - 1]);
                }
                if (vertical != GameControls.PlayedGame.VerticalTiles - 1)
                { 
                    TilesAround.Add(GameControls.PlayedGame.Minefield[horizontal - 1, vertical + 1]);
                    TilesAround.Add(GameControls.PlayedGame.Minefield[horizontal, vertical + 1]);
                }
            }
            if (horizontal != GameControls.PlayedGame.HorizontalTiles - 1)
            {
                TilesAround.Add(GameControls.PlayedGame.Minefield[horizontal + 1, vertical]);
                if (vertical != 0)
                {
                    TilesAround.Add(GameControls.PlayedGame.Minefield[horizontal + 1, vertical - 1]);
                    if (!TilesAround.Contains(GameControls.PlayedGame.Minefield[horizontal, vertical - 1]))
                        TilesAround.Add(GameControls.PlayedGame.Minefield[horizontal, vertical - 1]);
                }   
                if (vertical != GameControls.PlayedGame.VerticalTiles - 1)
                {
                    TilesAround.Add(GameControls.PlayedGame.Minefield[horizontal + 1, vertical + 1]);
                    if (!TilesAround.Contains(GameControls.PlayedGame.Minefield[horizontal, vertical + 1]))
                        TilesAround.Add(GameControls.PlayedGame.Minefield[horizontal, vertical + 1]);
                }
            }
            return;
        }
        public void MinesAroundCalculator()
        {
            MinesAround = 0;
            foreach (Tile tile in TilesAround)
            {
                if (tile.Mine)
                    MinesAround++;
            }
        }
        
        public abstract void PrintTile();
        public abstract int FlagTile(bool immediatePrint = true);
        public void ClearMine()
        {
            Mine = false;
        }
        public void PlantMine()
        {
            Mine = true;
        }
        public void PrintType()
        {
            Console.SetCursorPosition(25, 50);
            Console.WriteLine(GetType());
            Console.WriteLine(Covered);
        }
       
    }
}


