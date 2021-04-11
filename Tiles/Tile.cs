using System;
using System.Collections.Generic;

namespace GloriousMinesweeper
{
    abstract class Tile
    {
        ///Shrnutí
        ///Abstrakrtní třída Tile reprezentuje políčko
        ///Dědí z ní tři různé typy políček: CoveredTile (neotočené políčko), HighlightedTile (zvýrazněné/vybrané políčko), UncoveredTile (otočené políčko)
        public int MinesAround { get; protected set; } //Počet min na sousedních políčkách
        public bool Covered { get; protected set; } //Boolean určující otočené/neotočené
        public bool Flag { get; protected set; } //Boolean označující označení vlaječkou
        public bool Mine { get; protected set; } //Boolean označující minu
        public ConsoleColor Color { get; protected set; } //Současná barva políčka
        public ConsoleColor OriginalColor { get; protected set; } //Barva políčka před vyznačením
        public List<Tile> TilesAround { get; set; } //Seznam políček, se kterými toto políčko sousedí
        public Coordinates Position { get; protected set; } //Pozice v Consoli (pozice kde se má políčko vytisknout)
        public Coordinates MinefieldPosition { get; protected set; } //Pozice na hrací ploše

        

        public void TilesAroundCalculator()
        {
            ///Shrnutí
            ///Vytvoří seznam políček a umístí do něj políčka, která jsou na hrací ploše sousedem tohoto políčka
            TilesAround = new List<Tile>();
            int horizontal = MinefieldPosition.Horizontal; //Horizontální pozice tohoto políčka
            int vertical = MinefieldPosition.Vertical; //Vertikální pozice tohoto políčka
            if (horizontal != 0) //Pokud není horizontální pozice nula, přidají se políčka s posunem o
            {
                TilesAround.Add(GameControls.PlayedGame.Minefield[horizontal - 1, vertical]);   //(-1, 0)
                if (vertical != 0) //A zároveň není vertikální pozice 0
                {
                    TilesAround.Add(GameControls.PlayedGame.Minefield[horizontal - 1, vertical - 1]); //(-1, -1)
                    TilesAround.Add(GameControls.PlayedGame.Minefield[horizontal, vertical - 1]); //(0, -1)
                }
                if (vertical != GameControls.PlayedGame.VerticalTiles - 1) //A pokud není vertikální pozice maximální
                { 
                    TilesAround.Add(GameControls.PlayedGame.Minefield[horizontal - 1, vertical + 1]); //(-1, +1)
                    TilesAround.Add(GameControls.PlayedGame.Minefield[horizontal, vertical + 1]); //(0, +1)
                }
            }
            if (horizontal != GameControls.PlayedGame.HorizontalTiles - 1) //Pokud není horizontální pozice maximální, přidají se políčka s posunem o 
            {
                TilesAround.Add(GameControls.PlayedGame.Minefield[horizontal + 1, vertical]); //(+1, 0)
                if (vertical != 0) //A zároveň není vertikální pozice 0
                {
                    TilesAround.Add(GameControls.PlayedGame.Minefield[horizontal + 1, vertical - 1]); //(+1, -1)
                    if (!TilesAround.Contains(GameControls.PlayedGame.Minefield[horizontal, vertical - 1])) //Pokud jej již neobsahuje, tak (0, -1)
                        TilesAround.Add(GameControls.PlayedGame.Minefield[horizontal, vertical - 1]);
                }   
                if (vertical != GameControls.PlayedGame.VerticalTiles - 1) //A pokud není vertikální pozice maximální
                {
                    TilesAround.Add(GameControls.PlayedGame.Minefield[horizontal + 1, vertical + 1]); //(+1, +1)
                    if (!TilesAround.Contains(GameControls.PlayedGame.Minefield[horizontal, vertical + 1])) //Pokud jej již neobsahuje, tak (0, +1)
                        TilesAround.Add(GameControls.PlayedGame.Minefield[horizontal, vertical + 1]);
                }
            }
            return;
        }
        public void MinesAroundCalculator()
        {
            ///Shrnutí
            ///Vyoičítá ze seznamu okolních políček pořet min
            MinesAround = 0; //Vrátí počet min na nula
            foreach (Tile tile in TilesAround) //Projede celý seznam
            {
                if (tile.Mine) //Pokud má políčko v seznamu minu, zvýší se počet o jedna
                    MinesAround++;
            }
        }
        
        public abstract void PrintTile(); //Metoda, která tiskne políčka
        public abstract int FlagTile(bool immediatePrint = true); //Metoda, která označí vlajkou dané políčko
        public void ClearMine() //Metoda, která odstraní z políčka minu, používá se k prvnímu bezpečnému otočení
        {
            Mine = false;
        }
        public void PlantMine() //Metoda, která umístí na políčko z minu, používá se k umístění min, které odstarní první bezpečné otočení
        {
            Mine = true;
        }
    }
}


