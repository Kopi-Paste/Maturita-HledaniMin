using System;

namespace GloriousMinesweeper
{
    class UncoveredTile : Tile
    {
        public UncoveredTile(Tile originalTile, bool immediatePrint)
        {
            MinesAround = originalTile.MinesAround;
            Covered = false;
            Flag = false;
            Mine = originalTile.Mine;
            if (originalTile.Color == (ConsoleColor)DiffSwitcher.Colours[0].SettingValue.Number)
                Color = (ConsoleColor)DiffSwitcher.Colours[2].SettingValue.Number;
            else if (originalTile.Color == (ConsoleColor)DiffSwitcher.Colours[1].SettingValue.Number)
                Color = (ConsoleColor)DiffSwitcher.Colours[3].SettingValue.Number;
            else if (originalTile.OriginalColor == (ConsoleColor)DiffSwitcher.Colours[0].SettingValue.Number || originalTile.OriginalColor == (ConsoleColor)DiffSwitcher.Colours[2].SettingValue.Number)
                Color = (ConsoleColor)DiffSwitcher.Colours[2].SettingValue.Number;
            else if (originalTile.OriginalColor == (ConsoleColor)DiffSwitcher.Colours[1].SettingValue.Number || originalTile.OriginalColor == (ConsoleColor)DiffSwitcher.Colours[3].SettingValue.Number)
                Color = (ConsoleColor)DiffSwitcher.Colours[3].SettingValue.Number;
            Position = originalTile.Position;
            MinefieldPosition = originalTile.MinefieldPosition;
            TilesAround = originalTile.TilesAround;
            /*Colours[0] = Covered tiles colour
            Colours[1] = Covered tiles secondary colour
            Colours[2] = Uncovered tiles colour
            Colours[3] = Uncovered tiles secondary colour
            Colours[4] = Flag colour
            Colours[5] = Highlighted tile colour
            Colours[6] = Text colour*/
            //TilesAroundCalculator();
            if (immediatePrint)
                PrintTile();
            //GameControls.PlayedGame.Minefield[MinefieldPosition.Horizontal, MinefieldPosition.Vertical] = this;
            /*foreach (Tile tile in TilesAround)
            {
                tile.MinesAroundCalculator();
            }*/
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
        public override int FlagTile(bool immediatePrint = true)
        {
            throw new NotImplementedException();
        }
    }
}
