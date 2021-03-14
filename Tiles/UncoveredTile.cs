using System;

namespace GloriousMinesweeper
{
    class UncoveredTile : Tile
    {
        public UncoveredTile(Tile originalTile)
        {
            MinesAround = originalTile.MinesAround;
            Covered = false;
            Flag = false;
            Mine = originalTile.Mine;
            if (originalTile.Color == GameControls.PlayedGame.Cover)
                Color = GameControls.PlayedGame.Uncover;
            else if (originalTile.Color == GameControls.PlayedGame.CoverSecondary)
                Color = GameControls.PlayedGame.UncoverSecondary;
            else if (originalTile.OriginalColor == GameControls.PlayedGame.Cover || originalTile.OriginalColor == GameControls.PlayedGame.Uncover)
                Color = GameControls.PlayedGame.Uncover;
            else if (originalTile.OriginalColor == GameControls.PlayedGame.CoverSecondary || originalTile.OriginalColor == GameControls.PlayedGame.UncoverSecondary)
                Color = GameControls.PlayedGame.UncoverSecondary;
            Position = originalTile.Position;
            MinefieldPosition = originalTile.MinefieldPosition;
            TilesAround = originalTile.TilesAround;
            TilesAroundCalculator();
            PrintTile();
            //GameControls.PlayedGame.Minefield[MinefieldPosition.Horizontal, MinefieldPosition.Vertical] = this;
            foreach (Tile tile in TilesAround)
            {
                tile.MinesAroundCalculator();
            }
        }

        public override void PrintTile()
        {
            Position.GoTo();
            Console.BackgroundColor = Color;
            Console.ForegroundColor = GameControls.PlayedGame.Text;
            Console.Write(' ');
            if (MinesAround != 0)
                Console.Write(MinesAround);
            else
                Console.Write(' ');
        }
        public override int FlagTile()
        {
            throw new NotImplementedException();
        }
    }
}
