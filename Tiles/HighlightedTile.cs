using System;

namespace GloriousMinesweeper
{
    class HighlightedTile : Tile
    {
        ///Shrnutí
        ///HighlightedTile je zvýrazněné políčko, to může být jako otočené, tak i neotočené
        ///Dá se otočit a označit vlaječkou
        ///Má svoji speciální barvu, určenou hráčem
        public HighlightedTile(Tile originalTile) //Konstruktor vyjde z původního Tilu, který se má označit
        {
            if (originalTile.Color == (ConsoleColor)DiffSwitcher.Colours[5].SettingValue.Number) //Pokud políčko ze kterého vycházíme má jako barvu Higlighted
                OriginalColor = originalTile.OriginalColor; //Tak se jako originální barva zapíše originální barva originálního políčka
            else
                OriginalColor = originalTile.Color; //Jinak se jako originální barva zapíše současná barva políčka, ze kterého vycházíme
            MinesAround = originalTile.MinesAround;
            Covered = originalTile.Covered;
            Flag = originalTile.Flag;
            Mine = originalTile.Mine; 
            Color = (ConsoleColor)DiffSwitcher.Colours[5].SettingValue.Number; //Změní se ale současná barva políčka na Highlighted
            TilesAround = originalTile.TilesAround;
            Position = originalTile.Position;
            MinefieldPosition = originalTile.MinefieldPosition; //Ostatní fieldy zůstavají stejné
            PrintTile(); //Políčko se vytiskne, aby byla vidět změna barvy
            foreach (Tile tile in TilesAround) //Okolní políčka si vymění políčko ve svém seznamu
            {
                tile.TilesAroundCalculator();
            }
        }
        

        public override void PrintTile()
        {
            ///Shrnutí
            ///Metoda vytiskne políčko
            Position.GoTo(GameControls.Reprint); //Přesuneme se na pozici, na které je v Consoli políčko
            Console.ForegroundColor = ConsoleColor.White; //Políčko je zvýrazněno, a proto jeho text bude napsán bílou barvou
            if (Flag) //Pokud je políčko označeno vlajkou, tak se vytiskne políčko barvou vlajky
                Console.BackgroundColor = GameControls.PlayedGame.Flag;
            else //Jinak se vytiskne svojí Highlighted barvou
                Console.BackgroundColor = GameControls.PlayedGame.Highlight;
            if (Covered || MinesAround == 0) //Pokud se jedná o neotočené políčko nebo o políčko otočené, které má okolo sebe nula min, nevypíše se žádný text, pouze prázdný čtvereček (2 mezery)
                Console.Write("  ");
            else //Jinak se do levé poloviny čtverečku napíše mezera a do pravé poloviny se napíše počet min okolo
            {
                Console.Write(' ');
                Console.Write(MinesAround);
            }
        }
        public override int FlagTile(bool immediatePrint = true)
        {
            ///Shrnutí
            ///Metoda odstraní nebo umístí vlaječku (podle toho, jetsli na tomto tilu vlaječka je, nebo není
            ///Dostává vstupní boolean, který určuje zda se má okamžitě políčko přetisknou s vyznačenou vlaječkou nebo naopak bez ní
            ///Vrací int
            ///-1: Vlaječka byla odstraněna
            ///+1: Vlaječka byla umístěna
            Flag = !Flag; //Otočí se boolean
            if (Flag) //Pokud je nyní na políčku vlaječka, tak se vrátí 1
            {
                if (immediatePrint) //Pokud se má zároveň i políčko okamžitě vytisknout,
                {
                    Position.GoTo(GameControls.Reprint); //tak se nastaví pozice pro vytištění
                    Console.BackgroundColor = GameControls.PlayedGame.Flag; //Změní se barva na barvu vlaječky
                    Console.Write("  "); //A vytiskne se čtvereček (2 mezery)
                }
                return 1;
            }
            else //Pokud nyní na políčku vlaječka, není tak se vrátí -1
            {
                if (immediatePrint) //Opět, pokud se má políčko hned vytisknout provedou se stejné kroky, ale tentokrát se nastaví barva zvýrazněného tlačítka
                {
                    Position.GoTo(GameControls.Reprint);
                    Console.BackgroundColor = GameControls.PlayedGame.Highlight;
                    Console.Write("  ");
                }
                return -1;
            }
        }
    }
}
