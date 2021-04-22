using System;
using System.Collections.Generic;
using System.IO;

namespace GloriousMinesweeper
{
    
    static class Program
    {
    ///Shrnutí
    ///Ve statické třídě Program jsou uloženy některé důležité globální hodnoty a metody. Například barva textu nebo metoda, která požaduje opravu velikosti okna neo metoda, která zobrazuje tabulku nejlepších výsledků.
        static void Main()
        {///Shrnutí
         ///Základní metoda Main() nejprve donutí uživatele zvětšit okno na celou obrazovku a následně zobrazí první menu, které má tři tlačítka
         ///Play Minesweeper: Otevřít menu, kterým se vytváří nastavení hry
         ///Show Highscores: Zobrazit tabulku nejlepších výsledků
         ///Quit: Ukončit program
            Console.BackgroundColor = ConsoleColor.Black; //Restartuje barvu pozadí v konzoli na černou
            Console.ForegroundColor = ConsoleColor.White;//Nastaví barvu v konzoli na bílou
            TakenColours = new List<ConsoleColor>(); //Ve fieldu TakenColors se vytvoří nový prázdný seznam, do kterého budou později přidávány a odebírany barvy. Pokud je nějaká barva v seznamu, není možné nějaké nastavení na tuto barvu změnit, aby nemohly mít dva artikly stejnou barvu.
            Console.CursorVisible = false; //Skryje kurzor, neboť působí rušivě
            Console.WriteLine("Please fullscreen"); //Objeví se zpráva uživateli, aby zvětšil okno na celou obrazovku
            Console.WriteLine("Alt+Enter is highly recommended"); //Uživateli je doporučeno, aby okno zvětšil stiskutím kláves Alt+Enter, a ne pouze maximilizováním okna.
            Console.WriteLine("It is not recommended to Alt+Tab during the game or to un-fullscreen the game"); //Na závěr je uživateli doporučeno, aby hru neschovával na lištu stisknutím kláves Alt+Tab nebo zmenšoval okno
            while (((Console.LargestWindowWidth - 5) > Console.WindowWidth) || ((Console.LargestWindowHeight - 3) > Console.WindowHeight))
            { } //Prázdný while cyklus čekající na zvětšení okna donutí uživatele zvětšit hru na celou obrazovku.
            while (Console.KeyAvailable)
                Console.ReadKey(true); //Tento while cyklus odstraní ze streamu kláves všechny stisknuté klávesy. Dokud není stream kláves prázdný čte klávesy a nechává je být.
            Console.CursorVisible = false; //Znovu skryje kurzor, který se může při zvětšení okna znovu objevit
            DefaultTextColour = ConsoleColor.Gray; //Výchozí barva textu se nastaví na šedou, která je stále velmi dobře čitelná na černém podkladu. Bílá barva se bude používat na zvýrazňování vybraného tlačítka.
            FirstStart = true; //Field FirstStart, který indikuje zda uživatel zapíná nastavení hry poprvé, se nastaví na true
            PositionedText PlayGame = new PositionedText("Play Minesweeper", ConsoleColor.Black, Console.WindowWidth / 2 - 8, 10); //Vytvoří se tlačítko, kterým se zapíná hra
            PositionedText ShowHighscores = new PositionedText("See Highscores", ConsoleColor.Black, Console.WindowWidth / 2 - 7, 12);//Vytvoří se tlačítko, kterým se zobrazují nejlepší výsledky
            PositionedText Quit = new PositionedText("Quit", ConsoleColor.Black, Console.WindowWidth / 2 - 2, 14);//Vytvoří se tlačítko, kterým se dá hra ukončit
            Border MainMenuSmallBorder = new Border(Console.WindowWidth / 2 - 10, 8, 10, 20, ConsoleColor.Black, ConsoleColor.White, false); //Vytvoří se obdélník, který ohraničuje všechna tři tlačítka
            Border MainMenuBigBorder = new Border(0, 1, Console.WindowHeight - 1, Console.WindowWidth, ConsoleColor.Black, ConsoleColor.Gray, false); //Vytvoří se velký obdélník, který ohraničuje celou obrazovku
            ChosenLabel = 0; //Field ChosenLabel, který označuje vybrané tlačítko se nastaví na 0
            Labels = new List<IGraphic> { PlayGame, ShowHighscores, Quit, MainMenuSmallBorder, MainMenuBigBorder };//Field Labels je seznam všech grafických artiklů — tlačítek a obdélníků
            ConsoleKey keypressed = 0; //Stisknuté tlačítko je nastaveno na nulu a bude se číst od uživatele
            Console.Clear(); //Vyčistí obrazovku a připraví ji na vytisknutí prvního menu
            for (int x = 3; x < 5; x++)
            {
                Labels[x].Print(x == 4, Reprint); //Vytiskne první menu. Obdelník na pozici 4, tedy velký obdélník ohraničující celou obrazovku se vytiskne jako solid – jeho svislé čáry mají tloušťku dvou charů. Pokud při tištění není hra nastavena na celou obrazovku, počká se na opravu od uživatele, vymaže se Console a vytiskne se toto menu znovu od začátku
            }
            while (true) //Následuje nekonečný while cyklus, který se dá ukončit pouze vypnutím programu přes tlačítko Quit
            {
                for (int x = 0; x < 3; x++)
                {
                    Labels[x].Print(x == ChosenLabel, Reprint); //Nejprve se znovu vytisknou všechna tlačítka. Pokud se pozice tlačítka shoduje s intem ve fieldu ChosenLabel vytiskne se zvýrazněně, tedy bílou barvou. Pokud při tištění není hra nastavena na celou obrazovku, počká se na opravu od uživatele, vymaže se Console a vytiskne se toto menu znovu od začátku
                }
                keypressed = Console.ReadKey(true).Key; //Přečte klávesu, kterou stiskne uživatel
                switch (keypressed)
                {
                    case ConsoleKey.UpArrow: //Pokud uživatel stiskne šipku nahoru
                        if (ChosenLabel != 0) //A ChosenLabel už není na svém minimu tedy na nule
                            ChosenLabel--; //Sníží se ChosenLabel o jedna, tedy se zvýrazní a vybere tlačítko o jedna výše
                        break;
                    case ConsoleKey.DownArrow: //Pokud uživatel stiskne šipku dolů
                        if (ChosenLabel != 2) //A ChosenLabel už není na svém maximu tedy na dvojce
                            ChosenLabel++; //Zvýší se ChosenLabel o jedna, tedy se zvýrazní a vybere tlačítko o jedna níže
                        break;
                    case ConsoleKey.Enter: //Pokud uživatel zmáčkne Enter, tak se potvrdí aktuální výběr
                        switch (ChosenLabel) //Co se bude dít dál rozhodne to, které tlačítko je vybráno
                        {
                            case 0: //Pokud je ChosenLabel 0, tedy vybraným tlačítkem je Play Minesweeper
                                DiffSwitcher.StartMenu(FirstStart); //Přejde se do statické třídy DiffSwitcher, která vytiskne menu s nastaveními hry a umožní je měnit, pokud se jedná o první start, nastaví se výchozí hodnoty. Jinak se zůstávají předchozí hodnoty
                                FirstStart = false; //Když se uživatel vrátí z herního menu zpátky do prvního menu, tak se nastaví field FirstStart na false
                                Console.BackgroundColor = ConsoleColor.Black; //Následně se obrazovka vyčistí od předchozího menu
                                Console.Clear();
                                for (int x = 0; x < 5; x++) //A znovu se vytisknou všechny grafické artikly
                                {
                                    Labels[x].Print(x == 4, Reprint); //Artikl s pozicí 4, kterým je velký obdélník okolo celé obrazovky, se vytiskne s širokými svislými liniemi. Pokud při tištění není hra nastavena na celou obrazovku, počká se na opravu od uživatele, vymaže se Console a vytiskne se toto menu znovu od začátku
                                }
                                break;
                            case 1: //Pokud je ChosenLabel 1, tedy vybraným tlačítkem je ShowLeaderboards
                                ShowLeaderboards(); //Zavolá místní statickou metodu ShowLeaderboards, která zobrazuje nejlepší výsledky
                                for (int x = 0; x < 5; x++) //Poté, co si je uživatel prohlídne a vrátí se zpátky do menu se vytisknou všechny grafické artikly
                                {
                                    Labels[x].Print(x == 4, Reprint); //Artikl s pozicí 4, kterým je velký obdélník okolo celé obrazovky, se vytiskne s širokými svislými liniemi. Pokud při tištění není hra nastavena na celou obrazovku, počká se na opravu od uživatele, vymaže se Console a vytiskne se toto menu znovu od začátku
                                }
                                break;
                            case 2: //Pokud je ChosenLabel 2, tedy vybraným tlačítkem je Quit
                                Environment.Exit(0);//Ukončí se program s výstupním kódem 0
                                break;
                        }
                        break;
                    case ConsoleKey.R: //Pokud uživatel zmáčkne R zavolá se metoda Reprint
                        try
                        {
                            Reprint(); //Pokusí se vymazat menu a vytisknout jej znovu
                        }
                        catch (ArgumentOutOfRangeException) //Pokud nastane situace, že něco by se mělo tisknout mimo obrazovku
                        {
                            WaitForFix(); //Vyzve uživatele, aby zvětšil hru na celou obrazovku a nepustí ho dál dokud tak neučiní
                            Reprint(); //Znovu se pokusí vymazat menu a vytisknout jej znovu
                        }
                        break;
                }
            }
        }
        private static int ChosenLabel { get; set; } //Field, který určuje které tlačítko je vybráno v hlavním menu
        public static List<ConsoleColor> TakenColours { get; set; } //Field, do kterého se uklájí zabrané barvy, pokud je barva v tomto seznamu nepůjde na ni nastavit žádný artikl
        public static ConsoleColor DefaultTextColour { get; set; } //Barva, kterou se tiskne veškerý text, který není vyznačen. Vyznačený text se tiskne bíle.
        private static bool FirstStart { get; set; } //Field, který určuje, zda uživatel poprvé zapíná startovní menu, určuje se podle toho, zda se nastaví defaultní nastavení
        private static List<IGraphic> Labels { get; set; } //Field, ve kterém jsou uloženy všechny grafické objekty a odkud se tisknou

        
        public static void ShowLeaderboards()
        {
            ///Shrnutí
            ///Metoda, která načte ze souboru AppData/Roaming/Minesweeper/highscores.txt uložené nejlepší výkony
            if (((Console.LargestWindowWidth - 5) > Console.WindowWidth) || ((Console.LargestWindowHeight - 3) > Console.WindowHeight)) //Pokud není okno na celou obrazovku
                WaitForFix(); //Počká se dokud to uživatel nenapraví
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear(); //Smaže se vytištěné menu
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Minesweeper", "highscores.txt"); //do stringu path se uloží cesta k souború
            if (!File.Exists(path)) //Pokud soubor neexistuje
            {
                (new PositionedText("No saved scores yet.", ConsoleColor.Black, Console.WindowWidth / 2 - 10, 10)).Print(false, Reprint); //Vytvoří se oznámení, že nenjsou žádná skóre uložena a okamžitě se vytiskne. Pokud při tištění není hra nastavena na celou obrazovku, počká se na opravu od uživatele, vymaže se Console a vytiskne se toto menu znovu od začátku
                (new Border(0, 1, Console.WindowHeight - 1, Console.WindowWidth, ConsoleColor.Black, ConsoleColor.Gray, false)).Print(true, Reprint); //Vytvoří se také velký obdélník, který ohraničuje celou obrazovku a okamžitě se vytiskne. Pokud při tištění není hra nastavena na celou obrazovku, počká se na opravu od uživatele, vymaže se Console a vytiskne se toto menu znovu od začátku
                Console.ReadKey(true); //Dokud uživatel nezmáčkne klávesu zůstává zpráva zobrazená na obrazovce
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Clear(); //Poté se smaží vytištěné výsledky a zase se vracíme zpátky do předchozí metody
                return;
            }
            string[] leaderboards = File.ReadAllLines(path); //Pokud soubor existuje, program vytiskne všechny uložené výkony, tedy nejlepších deset
            Console.ForegroundColor = DefaultTextColour; //Barva textu se nastaví na výchozí
            Console.CursorVisible = false; //Znovu se pro jistotu skryje kurzor
            (new Border(0, 1, Console.WindowHeight - 1, Console.WindowWidth, ConsoleColor.Black, ConsoleColor.Gray, false)).Print(true, Reprint); //Vytvoří se a vytiskne se obdélník, který ohraničuje obrazovku. Pokud při tištění není hra nastavena na celou obrazovku, počká se na opravu od uživatele, vymaže se Console a vytiskne se toto menu znovu od začátku 
            (new Border(Console.WindowWidth / 2 - 50, 9, leaderboards.Length + 4, 100, ConsoleColor.Black, ConsoleColor.Gray, false)).Print(true, Reprint); //Vytvoří se a vytiskne se menší obdélník, který ohraničuje nejlepší výkony. Jeho výška se odvíjí od počtu uložených výkonů. Pokud při tištění není hra nastavena na celou obrazovku, počká se na opravu od uživatele, vymaže se Console a vytiskne se toto menu znovu od začátku 
            for (int x = 1; x <= leaderboards.Length; x++) //For cyklus, který tiskne všechny výsledky. Jde od jedničky až do počtu výkonů - 1.
            {
                string toPrint = x.ToString() + ".   " + leaderboards[x - 1]; //Do stringu tpPront se zapíše číslo iterace for cyklu a následně zápis v tabulce (tedy přezdívka hráče a body)
                new PositionedText(toPrint, ConsoleColor.Black, (Console.WindowWidth - toPrint.Length) / 2, 10 + x).Print(false, Reprint); //Vytvoří se a vytiskne se tento zápis. Umístění se odvíjí od délky zápisu a pořadí v tabulce
            }
            Console.ReadKey(true); //Nejlepší výkony zůstanou zobrazené dokud uživatel nestiskne klávesu
            Console.Clear(); //Poté se Console vyčistí a vracíme se do předchozí metody
            return;
        }
        private static void Reprint()
        {
            ///Shrnutí
            ///Metoda, která smaže a znovu vytiskne celé menu. Používá se v případě, že uživatel zmenšil nedopatřením okno, aby se mu povedlo srovnat vytištěné objekty na svá místa.
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear(); //Smaže se vše, co je v dané chvíli vytištěno
            for (int x = 0; x < 5; x++) //Znovu se vytisknou všehny grafické objekty
            {
                Labels[x].Print(x == 4 || x == ChosenLabel, Reprint); //Objekt na pozici, tedy velký obdélník okolo obrazovky, se vytiskne se silnými svislými čárami. Pokud při tištění není hra nastavena na celou obrazovku, počká se na opravu od uživatele, vymaže se Console a začne se tato metoda znovu od začátku 
            }
        }
        public static void WaitForFix()
        {
            ///Shrnutí
            ///Globálně používaná metoda. Pokud se zjistí, že není hra na celou obrazovku zavolá se tato metoda, která zajistí, aby to uživatel napravil
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear(); //Nejprve se vymaže vše, co je v danou chvíli vytištěno
            Console.SetCursorPosition(0, 0); //Kurzor se nastaví na pozici 0, 0; která by měla být prakticky vždy dostupná
            Console.WriteLine("Please fullscreen using Alt+Enter"); //Uživatel dostane zprávu aby hru zvětšil na celou obrazovku
            while (((Console.LargestWindowWidth - 5) > Console.WindowWidth) || ((Console.LargestWindowHeight - 3) > Console.WindowHeight))
            { } //Tento while cyklus jej nepustí dál, dokud tak neučiní
            while (Console.KeyAvailable) //Tento while cyklus odstraní všechny klávesy ze streamu kláves, dokud se něco nachází ve streamu
                Console.ReadKey(true); //Čte klávesy a zahazuje je
            System.Threading.Thread.Sleep(50); //Pak ještě chvilku počkáme, aby se stihly aktualizovat hodnoty Console.WindowHeight a Console.WindowWidth
        }
    }
}
