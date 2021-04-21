using System;
using System.Collections.Generic;

namespace GloriousMinesweeper
{
    class Game
    {
        ///Shrnutí
        ///Třída Game vytváří hru. Jsou zde uloženy důležité parametry o hře
        public int HorizontalTiles { get; } //Počet políček zleva doprava
        public int VerticalTiles { get; } //Počet políček shora dolů

        public int Mines { get; } //Počet min

        public ConsoleColor Cover { get; } //První barva neotočených políček
        public ConsoleColor CoverSecondary { get; } //Druhá barva neotočených políček
        public ConsoleColor Uncover { get; } //První barva otočených políček
        public ConsoleColor UncoverSecondary { get; } //Druhá barva otočených políček
        public ConsoleColor Highlight { get; } //Barva zvýrazněného políčka
        public ConsoleColor Flag { get; } //Barva políček označených vlaječkou
        public ConsoleColor Text { get; } //Barva textu
        public Tile[,] Minefield { get; set; } //Herní plocha

        public Game(int[] parameters) //Konstruktor, který vytvoří z parametrů z menu nastavení hry samotnou hru
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
            Text = (ConsoleColor)parameters[9]; //Nahrají se veškeré fieldy
            Minefield = new Tile[HorizontalTiles, VerticalTiles]; //Vytvoří se prázdné minové pole
            int remainingMines = Mines; //Nyní se pomocí třídy Random určí, která políčka budou mít miny
            int remainingTiles = HorizontalTiles * VerticalTiles; //Máme dvě proměnné: Zbývající miny a zbývající políčka
            ConsoleColor currentColour; //Také se každému políčku přiřadí jeho první barva
            Random rng = new Random();
            bool mine;
            for (int x = 0; x != HorizontalTiles; x++)
            {
                if (x % 2 == 0)
                    currentColour = Cover; //Pokud jsme v lichém řádku, políčko úplně vlevo bude mít první barvu (Cover)
                else
                    currentColour = CoverSecondary; //Pokud jsme v sudém, políčko bude mít druhou barvu (Cover Secondary)

                    for (int y = 0; y != VerticalTiles; y++)
                    {
                        int mineDeterminator = rng.Next(remainingTiles); //Nyní se vygeneruje náhodné číslo od nuly do počtu zbývajících políček
                        if (mineDeterminator < remainingMines) //Pokud je náhodné číslo nižší než počet zbývajících min,
                        {
                            mine = true; //tak následující políčko bude mít minu,
                            remainingMines--; //Samozřejmě se sníží počet zbývajících min a políček
                            remainingTiles--;
                        }
                        else
                        {
                            mine = false; //jinak následující políčko minu mít nebude
                            remainingTiles--; //A sníží se jen počet zbývajících políček
                        }
                        Minefield[x, y] = new CoveredTile(mine, currentColour, (Console.WindowWidth/2 - HorizontalTiles+2*x), y + ((Console.WindowHeight - VerticalTiles) / 2), x, y); //Na pozici x, y se vytvoří nové políčko s minou určenou náhodným generátorem, souřadnicemi x, y a souřadnicemi v Consoli, které jsou propočítany tak, aby se Minefield nacházel ve středu obrazovky
                        if (currentColour == Cover) //nyní se změní currentColour, aby následující políčko mělo jinou barvu a tvořila se „šachovnice“
                            currentColour = CoverSecondary;
                        else
                            currentColour = Cover;
                    }
            }
        }
        public Game(string[] savedGame) //Konstruktor, který slouží k vytvoření hry z hry uložené v souboru .txt
        {
            ///První řádek: Pozice umístěných vlaječek
            ///Druhý řádek: Pozice umístěných otazníků
            ///Třetí řádek: Pozice min
            ///Čtvrtý řádek: Pozice již otočených políček
            ///Pátý řádek: Parametry hry
            ///Šestý řádek: Současný čas
            string[] parameters = savedGame[4].Split(','); //Z pátého řádku se přečtou parametry hry a zapíší se do fieldů
            HorizontalTiles = int.Parse(parameters[0]);
            VerticalTiles = int.Parse(parameters[1]);
            Mines = int.Parse(parameters[2]);
            Cover = (ConsoleColor)int.Parse(parameters[3]);
            CoverSecondary = (ConsoleColor)int.Parse(parameters[4]);
            Uncover = (ConsoleColor)int.Parse(parameters[5]);
            UncoverSecondary = (ConsoleColor)int.Parse(parameters[6]);
            Flag = (ConsoleColor)int.Parse(parameters[7]);
            Highlight = (ConsoleColor)int.Parse(parameters[8]);
            Text = (ConsoleColor)int.Parse(parameters[9]);
            Program.DefaultTextColour = Text; //Barva textu v programu se také změní podle uložené hry 
            Minefield = new Tile[HorizontalTiles, VerticalTiles]; //Vytvoří se prázdný Minefield
            ConsoleColor currentColour;
            DiffSwitcher.SetLoaded(parameters); //Parametry se odešlou také do DiffSwitcheru, aby hráč, pokud se vrátí zpátky do nastavení, zde viděl tato nastavení

            for (int x = 0; x != HorizontalTiles; x++)
            {
                if (x % 2 == 0)
                    currentColour = Cover;
                else
                    currentColour = CoverSecondary; //Nastavení barev funguje naprosto stejně jako u prvního konstruktoru

                for (int y = 0; y != VerticalTiles; y++)
                {
                    Minefield[x, y] = new CoveredTile(false, currentColour, (Console.WindowWidth / 2 - HorizontalTiles + 2 * x), y + ((Console.WindowHeight - VerticalTiles) / 2), x, y); //Nejprve se pokryje celý Minefield neotočenými políčkami
                    if (currentColour == Cover)
                        currentColour = CoverSecondary;
                    else
                        currentColour = Cover;
                }
            }
            string[] uncovered = savedGame[3].Split(';'); //Poté se podle čtvrtého řádku otočí políčka
            foreach (string coordinates in uncovered)
            {
                if (coordinates != "")
                {
                    string[] position = coordinates.Split(',');
                    Minefield[int.Parse(position[0]), int.Parse(position[1])] = new UncoveredTile(Minefield[int.Parse(position[0]), int.Parse(position[1])], false);
                }
            }
            string[] flagged = savedGame[0].Split(';'); //Podle prvního řádku se označí políčka vlaječkou
            foreach (string coordinates in flagged)
            {
                if (coordinates != "")
                {
                    string[] position = coordinates.Split(',');
                    Minefield[int.Parse(position[0]), int.Parse(position[1])].SetTileToFlag();
                }
            }
            string[] questioned = savedGame[1].Split(';');
            foreach (string coordinates in questioned) //Podle druhého řádku se umístí ozazníčky
            {
                if (coordinates != "")
                {
                    string[] position = coordinates.Split(',');
                    Minefield[int.Parse(position[0]), int.Parse(position[1])].SetTileToQuestionMark();
                }
            }
            string[] mines = savedGame[2].Split(';'); //A podle třetího řádku se umístí miny
            foreach (string coordinates in mines)
            {
                if (coordinates != "")
                {
                    string[] position = coordinates.Split(',');
                    Minefield[int.Parse(position[0]), int.Parse(position[1])].PlantMine();
                }
            }
        }
        public void TilesAndMinesAroundCalculator()
        {
            ///Shrnutí
            ///U všech políček se spočítají okolní políčka a následně se spočítá, kolik má které políčko okolo sebe min
            foreach (Tile tile in Minefield)
                tile.TilesAroundCalculator();
            foreach (Tile tile in Minefield)
                tile.MinesAroundCalculator();
        }

        public void PrintMinefield(bool flagMines = false)
        {
            ///Shrnutí
            ///Vytiskne celou hrací plochu
            ///Přijímá boolean flagMines, který určuje zda se mají všechny miny označit vlaječkou (tedy zobrazit správné řešení (to se používá pouze v PostGameMenu)
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear(); //Nejprve se vymaže obrazovka
            foreach (Tile tile in Minefield) //Následně se projede celý Minefield
            {
                if (flagMines && tile.Mine && !tile.Flag) //Pokud se mají vyznačit políčka s minami
                {
                    tile.SetTileToFlag();
                }
                tile.PrintTile(); //A každé políčko se vytiskne
            }
        }
        public void MoveMinesOut(Tile selectedTile, List<Tile> forbbidenTiles)
        {
            ///Shrnutí
            ///Tato metoda zajistí, že z políčka a seznamu políček budou odstraněny miny a budou přesunuty na jiná políčka, která ještě miny nemají
            ///Tato metoda se používá pro bezpečné první otočení, aby se nemohlo stát, že hráč prohraje hned prvním tahem
            int clearedMines = 0; //Do této proměnné se ukládá počet odstraněných min
            if (selectedTile.Mine) //Pokud má vybrané políčko minu, tak se z nějmina odstraní a zvýší se počet odstraněných min o jedna
            {
                Minefield[selectedTile.MinefieldPosition.Horizontal, selectedTile.MinefieldPosition.Vertical].ClearMine();
                clearedMines++;
            }
            foreach (Tile tile in forbbidenTiles) //Nyní se projedou všechna políčka okolo vybraného
            {
                if (tile.Mine) //Pokud mají minu, tak je i odsud mina odstraněna a číslo se opět zvýši
                {
                    Minefield[tile.MinefieldPosition.Horizontal, tile.MinefieldPosition.Vertical].ClearMine();
                    clearedMines++;
                }
            }
            while (clearedMines != 0) //Nyní se začnou miny znovu umisťovat. Tento while cyklus probíhá, dokud nejsou všechny miny umístěny
            {
                Random rnd = new Random();
                int horizontalPosition = 0;
                int verticalPosition = 0; 
                do
                {
                    horizontalPosition = rnd.Next(HorizontalTiles); //Nyní se budou náhodně generovat pozice
                    verticalPosition = rnd.Next(VerticalTiles);
                }
                while ((Math.Abs(verticalPosition - selectedTile.MinefieldPosition.Vertical) <= 1 && Math.Abs(horizontalPosition - selectedTile.MinefieldPosition.Horizontal) <= 1) || Minefield[horizontalPosition, verticalPosition].Mine); //Pokud byla vybrána pozice v okolí vybraného políčka nebo pozice, na kterém už mina je, tak se bude pozice vybírat znova
                Minefield[horizontalPosition, verticalPosition].PlantMine(); //Když je vybrána validní pozice, tak se na ni umístí mina
                clearedMines--; //A sníží se počet zbývajících min k umístění
            }
            TilesAndMinesAroundCalculator(); //Nakonec se přepočítají znovu okolní políčka a miny, neboť se mohly změnit
        }
        public int[] GetParameters()
        {
            ///Shrnutí
            ///Tato metoda vrátí parametry této hry v arrayi intů
            int[] Parameters = new int[10]; 
            Parameters[0] = HorizontalTiles;
            Parameters[1] = VerticalTiles;
            Parameters[2] = Mines;
            Parameters[3] = (int)Cover;
            Parameters[4] = (int)CoverSecondary;
            Parameters[5] = (int)Uncover;
            Parameters[6] = (int)UncoverSecondary;
            Parameters[7] = (int)Flag;
            Parameters[8] = (int)Highlight;
            Parameters[9] = (int)Text;
            return Parameters; //Všechny parametry se nahrají do arraye a ten se vrátí
        }
        public override string ToString()
        {
            ///Shrnutí
            ///Tato metoda dá dohromady parametry, převede je na string a vloží mezi ně čárky
            ///Používá se k zápisu parametrů do .txt při ukládání hry
            string ToString = HorizontalTiles.ToString() + ',' + VerticalTiles.ToString() + ',' + Mines.ToString() + ',' + ((int)Cover).ToString() + ',' + ((int)CoverSecondary).ToString() + ',' + ((int)Uncover).ToString() + ',' + ((int)UncoverSecondary).ToString() + ',' + ((int)Flag).ToString() + ',' + ((int)Highlight).ToString() + ',' + ((int)Text).ToString() + ',' + GameControls.UncoveredTiles.ToString() + ',' + GameControls.NumberOfFlags.ToString() + ',' + GameControls.IncorrectFlags.ToString() + ',' + GameControls.ScoreMultiplier.ToString();
            return ToString;
        }
    }
}
