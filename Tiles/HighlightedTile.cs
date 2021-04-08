using System;

namespace GloriousMinesweeper
{
    class HighlightedTile : Tile
    {
        public HighlightedTile(Tile originalTile)
        {
            if (originalTile.Color == (ConsoleColor)DiffSwitcher.Colours[5].SettingValue.Number)
                OriginalColor = originalTile.OriginalColor;
            else
                OriginalColor = originalTile.Color;
            MinesAround = originalTile.MinesAround;
            Covered = originalTile.Covered;
            Flag = originalTile.Flag;
            Mine = originalTile.Mine;
            Color = GameControls.PlayedGame.Highlight;
            TilesAround = originalTile.TilesAround;
            Position = originalTile.Position;
            MinefieldPosition = originalTile.MinefieldPosition;
            PrintTile();
            foreach (Tile tile in TilesAround)
            {
                tile.TilesAroundCalculator();
            }
        }
        

        public override void PrintTile()
        {
            Position.GoTo();
            Console.ForegroundColor = ConsoleColor.White;
            if (Flag)
                Console.BackgroundColor = GameControls.PlayedGame.Flag;
            else
                Console.BackgroundColor = GameControls.PlayedGame.Highlight;
            if (Covered || MinesAround == 0)
                Console.Write("  ");
            else
            {
                Console.Write(' ');
                Console.Write(MinesAround);
            }
        }
        public override int FlagTile(bool immediatePrint = true)
        {
            Flag = !Flag;
            Console.SetCursorPosition(Position.Horizontal, Position.Vertical);
            if (Flag)
            {
                if (immediatePrint)
                {
                    Console.BackgroundColor = GameControls.PlayedGame.Flag;
                    Console.Write("  ");
                }
                return 1;
            }
            else
            {
                if (immediatePrint)
                {
                    Console.BackgroundColor = GameControls.PlayedGame.Highlight;
                    Console.Write("  ");
                }
                return -1;
            }
        }
    }
}
