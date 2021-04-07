using System;
using System.Collections.Generic;

namespace GloriousMinesweeper
{
    class CoveredTile : Tile
    {
        public CoveredTile(bool mine, ConsoleColor color, int horizontal, int vertical, int x, int y)
        {
            Mine = mine;
            MinesAround = 0;
            Covered = true;
            Flag = false;
            Color = color;
            OriginalColor = color;
            TilesAround = new List<Tile>();
            Position = new Coordinates(horizontal, vertical);
            MinefieldPosition = new Coordinates(x, y);
        }
        public CoveredTile(Tile originalTile)
        {
            Mine = originalTile.Mine;
            MinesAround = originalTile.MinesAround;
            Covered = true;
            Flag = originalTile.Flag;
            Color = originalTile.OriginalColor;
            TilesAround = originalTile.TilesAround;
            Position = originalTile.Position;
            MinefieldPosition = originalTile.MinefieldPosition;
            PrintTile();
        }

        public override void PrintTile()
        {
            Position.GoTo();
            if (Flag)
                Console.BackgroundColor = GameControls.PlayedGame.Flag;
            else
                Console.BackgroundColor = Color;
            Console.Write("  ");
        }
        public override int FlagTile()
        {
            Flag = !Flag;
            Console.SetCursorPosition(Position.Horizontal, Position.Vertical);
            if (Flag)
            {
                Console.BackgroundColor = GameControls.PlayedGame.Flag;
                Console.Write("  ");
                return 1;
            }
            else
            {
                Console.BackgroundColor = GameControls.PlayedGame.Highlight;
                Console.Write("  ");
                return -1;
            }
        }
    }
}

