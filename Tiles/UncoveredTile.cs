using System;

namespace GloriousMinesweeper
{
    class UncoveredTile : Tile
    {
        ///Shrnutí
        ///Otočené políčko, které ukazuje kolik min je okolo něj a nic moc jiného se s ním nedá dělat 
        public UncoveredTile(Tile originalTile, bool immediatePrint) //Konstruktor který přijme políčko, ze kterého se udělá UncoveredTile. Dále přijímá bool jestli se má okamžitě po změně políčko vytisknout
        {
            MinesAround = originalTile.MinesAround; //Počet min okolo se nemění
            Covered = false; //toto políčko už nebude nikdy zakryté
            Flag = false; //Odkryté políčko nemůže být označeno vlaječkou. Pokud ji má, tak se odstraní
            Questionmark = false; //Odkryté políčko nemůže mít otazník
            Mine = false; //Odkryté políčko nemůže mít minu. Jinak by hra skončila prohrou
            if (originalTile.Color == (ConsoleColor)DiffSwitcher.Colours[0].SettingValue.Number) //Pokud má původní políčko barvu Cover, bude mít toto políčko barvu Uncover
                Color = (ConsoleColor)DiffSwitcher.Colours[2].SettingValue.Number; 
            else if (originalTile.Color == (ConsoleColor)DiffSwitcher.Colours[1].SettingValue.Number) //Pokud má původní políčko barvu CoverSecondary, bude mít toto políčko barvu UncoverSecondary
                Color = (ConsoleColor)DiffSwitcher.Colours[3].SettingValue.Number;
            else if (originalTile.OriginalColor == (ConsoleColor)DiffSwitcher.Colours[0].SettingValue.Number || originalTile.OriginalColor == (ConsoleColor)DiffSwitcher.Colours[2].SettingValue.Number) //Pokud nemá ani Cover ani CoverSecondary, tak je jasné, že originální políčko je HighlightedTile, a proto se budeme soustředit na OriginalColor. Pokud je OriginalColor Cover nebo Uncover, bude mít toto políčko barvu Uncover.
                Color = (ConsoleColor)DiffSwitcher.Colours[2].SettingValue.Number;
            else if (originalTile.OriginalColor == (ConsoleColor)DiffSwitcher.Colours[1].SettingValue.Number || originalTile.OriginalColor == (ConsoleColor)DiffSwitcher.Colours[3].SettingValue.Number) //Pokud je OriginalColor CoverSecondary nebo UncoverSecondary, bude mít toto políčko barvu UncoverSecondary
                Color = (ConsoleColor)DiffSwitcher.Colours[3].SettingValue.Number;
            Position = originalTile.Position;
            MinefieldPosition = originalTile.MinefieldPosition;
            TilesAround = originalTile.TilesAround; //Většina fieldů zůstává stejná
            if (immediatePrint) //Pokud se má políčko okamžitě vytisknout, zavolá se metoda PrintTile()
                PrintTile();
        }

        public override void PrintTile()
        {
            ///Shrnutí
            ///Vytisikne políčko
            if (((Console.LargestWindowWidth - 5) > Console.WindowWidth) || ((Console.LargestWindowHeight - 3) > Console.WindowHeight))
                Program.WaitForFix();
            Position.GoTo(GameControls.Reprint); //Přesune nás na pozici políčka
            Console.BackgroundColor = Color; //Barva pozadí se nastaví na barvu políčka
            Console.ForegroundColor = GameControls.PlayedGame.Text; //Barva textu se nastaví podle fieldu Program.DefaultTextColour
            Console.Write(' '); //Napíše se na první pozici čtvereku mezera 
            if (MinesAround != 0) //Pokud není počet min okolo nula, napíše se na druhou polovinu čtverečku počet min
                Console.Write(MinesAround);
            else //Pokud je počet min okolo nula, napíše se i na druhou polovinu čtverečku mezera, čili bude políčko celé prazdné
                Console.Write(' ');
        }
        public override int FlagTile(bool immediatePrint = true) //Odkrytá políčka nemohou být označena vlaječkou
        {
            throw new NotImplementedException();
        }
    }
}
