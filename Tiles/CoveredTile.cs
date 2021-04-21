using System;
using System.Collections.Generic;

namespace GloriousMinesweeper
{
    class CoveredTile : Tile
    {
        ///Shrnutí
        ///Neotočené políčko -> To které je na začátku hry na každé pozici
        ///Nemá na sobě žádný text, lze jej označit vlaječkou nebo otočit
        ///Při tvorbě hry se generují políčka tohoto typu
        public CoveredTile(bool mine, ConsoleColor color, int horizontal, int vertical, int x, int y) //Konstruktor, který se používa při tvoření hry. Obdrží se hodnoty všech fieldů, kromě TilesAround a MinesAround, které se počítají až když jsou všechna políčka vytvořena
        {
            Mine = mine;
            MinesAround = 0;
            Covered = true;
            Flag = false;
            Questionmark = false;
            Color = color;
            OriginalColor = color;
            TilesAround = new List<Tile>();
            Position = new Coordinates(horizontal, vertical);
            MinefieldPosition = new Coordinates(x, y);
        }
        public CoveredTile(Tile originalTile) //Konstruktor, který se používá při tvorbě neotočeného políčka z vyznačeného políčka
        {
            Mine = originalTile.Mine;
            MinesAround = originalTile.MinesAround;
            Covered = true;
            Flag = originalTile.Flag;
            Questionmark = originalTile.Questionmark;
            Color = originalTile.OriginalColor; //Všechny fieldy se přepisují z originálního políčka. Kromě barvy, která se získá z originální barvy originálního políčka
            TilesAround = originalTile.TilesAround;
            Position = originalTile.Position;
            MinefieldPosition = originalTile.MinefieldPosition;
            PrintTile(); //Na závěr se políčko znovu vytiskne, aby hráč poznal změnu
        }

        public override void PrintTile()
        {
            ///Shrnutí
            ///Vytiskne toto políčko na jeho pozici
            if (((Console.LargestWindowWidth - 5) > Console.WindowWidth) || ((Console.LargestWindowHeight - 3) > Console.WindowHeight))
                Program.WaitForFix();
            Position.GoTo(GameControls.Reprint); //Přesuneme se na pozici tohoto políčka
            if (Flag) //Pokud má políčko vlaječku, vytiskne se barvou vlaečky
                Console.BackgroundColor = GameControls.PlayedGame.Flag;
            else //Jinak se vytiskne svojí barvou
                Console.BackgroundColor = Color;
            if (Questionmark)
                Console.Write(" ?");
            else
                Console.Write("  "); //Vytiskne se čtvereček (2 mezery)
        } 
        public override int FlagTile(bool immediatePrint = true)
        {
            ///Shrnutí
            ///Metoda odstraní nebo umístí vlaječku nebo otazník (podle toho, jestli na tomto tilu je vlaječka nebo otazník nebo nic)
            ///Dostává vstupní boolean, který určuje zda se má okamžitě políčko přetisknou s vyznačenou vlaječkou nebo otazníkem nebo naopak bez nich
            ///Vrací int
            ///-1: Vlaječka byla odstraněna
            ///0: Počet vlaječek se vůbec nemění. Jedná se o přechod z otazníku na prázdné políčko
            ///+1: Vlaječka byla umístěnas
            /*Flag = !Flag; //Otočí se boolean
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
            }*/
            if (Flag) //Pokud na políčku je vlaječka, tak se vlaječka odstraní a nahradí se otazníkem
            {
                Flag = false;
                Questionmark = true;
                if (immediatePrint)
                    PrintTile();
                return -1;
            }
            else if (Questionmark) //Pokud je na políčku otazník, tak se otazník odstraní a políčkose nechá prázdné
            {
                Questionmark = false;
                if (immediatePrint)
                    PrintTile();
                return 0;
            }
            else //Pokud je políčko prázdne (tedy není na něm ani otazník, ani vlaječka, tak se označí vlaječkou
            {
                Flag = true;
                if (immediatePrint)
                    PrintTile();
                return +1;
            }
        }
    }
}

