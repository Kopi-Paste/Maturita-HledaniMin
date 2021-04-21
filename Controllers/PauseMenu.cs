using System;
using System.Collections.Generic;
using System.IO;

namespace GloriousMinesweeper
{
    static class PauseMenu
    {
        ///Shrnutí
        ///Statická třída PauseMenu reprezentuje menu pozastavené hry. Toto menu se zobrazí uživateli pokud stiskne Escape v průběhu hry. Během zobrazení menu je hrací plocha skrytá a neběží čas
        ///Uživatel zde má několik možností: Pokračovat ve hře, uložit tuto hru, načíst předchozí uloženou hru, vrátit se zpět do menu nebo ukončit program
        private static int ChosenLabel { get; set; } //Toto číslo opět reprezentuje vybrané tlačítko
        private static int ChosenFile { get; set; } //Toto číslo reprezentuje vybraný soubor, v momentě, kdy uživatel vybírá z uložených souborů
        private static SpecialisedStopwatch CurrentTime { get; set; } //Tento field reprezentuje čas, který již byl hráči v této hře naměřen
        private static IGraphic[] Labels { get; set; } //Array grafických objektů bez tlačítek
        private static PositionedText[] SwitchableLabels { get; set; } //Array tlačítek
        private static List<PositionedText> FilesAsText { get; set; } //Seznam názvů uložených souborů, které se dají do typu PositionedText
        public static bool PauseGameMenu()
        {
            ///Shrnutí
            ///Základní metoda PauseMenu, která vrátí boolean podle toho, zda hráč chce pokračovat ve hře nebo zda chce hru ukončit
            if (((Console.LargestWindowWidth - 5) > Console.WindowWidth) || ((Console.LargestWindowHeight - 3) > Console.WindowHeight)) //Nejprve se ověří, zda je hra na celou obrazovku, protože jinak by se mohly vytvořit grafiky na chybných pozicích
                Program.WaitForFix(); //Počká se tedy, než hráč dá hru na celou obrazovku
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear(); //Obrazovka se smaže
            CurrentTime = GameControls.CompletionTime; //Načte se uběhlý čas ze hry
            Border PauseBigBorder = new Border(0, 1, Console.WindowHeight - 1, Console.WindowWidth, ConsoleColor.Black, ConsoleColor.Gray, false); 
            Border PauseSmallBorder = new Border(Console.WindowWidth / 2 - 30, 18, 9, 60, ConsoleColor.Black, ConsoleColor.Gray, false);
            PositionedText PausedGame = new PositionedText("Game is paused", ConsoleColor.Black, Console.WindowWidth / 2 - 7, 20);
            PositionedText Unpause = new PositionedText("Continue", ConsoleColor.Black, Console.WindowWidth / 2 - 27, 24);
            PositionedText SaveGame = new PositionedText("Save game", ConsoleColor.Black, Console.WindowWidth / 2 - 16, 24);
            PositionedText LoadGame = new PositionedText("Load game", ConsoleColor.Black, Console.WindowWidth / 2 - 4, 24);
            PositionedText BackToMenu = new PositionedText("Back to Menu", ConsoleColor.Black, Console.WindowWidth / 2 + 8, 24);
            PositionedText Quit = new PositionedText("Quit", ConsoleColor.Black, Console.WindowWidth / 2 + 23, 24);
            string time = "Current time: " + CurrentTime.ToString(); 
            PositionedText Time = new PositionedText(time, ConsoleColor.Black, (Console.WindowWidth - time.Length) / 2, 22); //Vytvoří se všechny grafiky
            Labels = new IGraphic[4];
            Labels[0] = PauseBigBorder;
            Labels[1] = PauseSmallBorder;
            Labels[2] = PausedGame;
            Labels[3] = Time;
            SwitchableLabels = new PositionedText[5];
            SwitchableLabels[0] = Unpause;
            SwitchableLabels[1] = SaveGame; 
            SwitchableLabels[2] = LoadGame; 
            SwitchableLabels[3] = BackToMenu; 
            SwitchableLabels[4] = Quit; //A grafiky se uloží do svých arrayů. Tlačítka do SwitchableLabels, zbytek do Labels
            ChosenLabel = 0; //Nastaví se vybrané tlačítko na nulu (Continue – Pokračovat)
            for (int x = 0; x < 4; x++) //Vytisknou se grafické objekty
                Labels[x].Print(x < 2, Reprint); //Oba obdélníky se vytisnou se silnými svislými liniemi. Pokud při tištění není hra nastavena na celou obrazovku, počká se na opravu od uživatele, vymaže se Console a toto menu se vytiskne znovu
            ConsoleKey keypressed = 0; //Nyní budeme opět číst klávesu
            while (true) //Začíná nekonečný while cyklus, který se dá opustit pouze tím, že hráč vrátí true (pokračovat ve hře), false (vrátit se do menu) nebo ukončí program
            {
                for (int x = 0; x < 5; x++) //Před stisknutím klávesy se vytisknou všechna tlačítka
                {
                    SwitchableLabels[x].Print(x == ChosenLabel, Reprint); //Pokud je tlačítko na vybrané pozici, tak se vytiskne bílou barvou (zvýrazněně). Pokud při tištění není hra nastavena na celou obrazovku, počká se na opravu od uživatele, vymaže se Console a toto menu se vytiskne znovu
                }
                keypressed = Console.ReadKey(true).Key; //Nyní se přečte klávesa od uživatele
                switch (keypressed)
                {
                    case ConsoleKey.RightArrow: //Pokud uživatel zmáčkne šipku doprava tak se pokusíme posunout o jedno doprava (tedy zvýšit číslo o jedna)
                        if (ChosenLabel != 4) //Stane se tak pokud již nejsme na maximální hodnotě 4 (tedy Quit – Ukončit)
                            ChosenLabel++;
                        break;
                    case ConsoleKey.LeftArrow: //Pokud uživatel zmáčkne šipku doleva tak se pokusíme posunout o jedno doleva (tedy snížit číslo o jedna)
                        if (ChosenLabel != 0) //Stane se tak pokud již nejsme na svém minimu 0 (tedy Continue – pokračovat)
                            ChosenLabel--;
                        break;
                    case ConsoleKey.Escape: //Pokud uživatel zmáčkne Escape, tak se okamžitě vrátí do hry (Escapem pauzu zapne i ukončí)
                        return true;
                    case ConsoleKey.Enter: //Enterem hráč potvrdí svoji volbu
                        switch (ChosenLabel)
                        {
                            case 0: //Pokud je vybráno tlačítko Continue (Pokračovat), tak se vrátí true a pokračuje se ve hře
                                return true;
                            case 1: //Pokud je vybráno tlačítko Save game (Uložit hru), tak se hra uloží pomocí metody SaveTheGame()
                                SaveTheGame(); 
                                Console.BackgroundColor = ConsoleColor.Black; //Když se hráč vrátí zpěrt
                                Console.Clear(); //Vymaží se grafiky ukládání
                                for (int x = 0; x < 4; x++) //A znovu se vytisknou grafické objekty kromě tlačítek (tlačítka se vytisknou poté na začátku další fáze while cyklu)
                                    Labels[x].Print(x < 2, Reprint); //Oba obdélníky se vytisnou se silnými svislými liniemi. Pokud při tištění není hra nastavena na celou obrazovku, počká se na opravu od uživatele, vymaže se Console a toto menu se vytiskne znovu
                                break;
                            case 2: //Pokud je vybráno tlačítko Load game (Načíst hru), tak se zavolá metoda LoadTheGame(), která vrací bool. Tento bool určuje zda hráč skutečně hru načetl nebo si to v průběhu rozmyslel/nebyly žádné hry k načtení
                                if (LoadTheGame()) //Pokud se hra úspěšně načetla
                                { 
                                    Console.BackgroundColor = ConsoleColor.Black; //Smaže se současné vytisklé menu
                                    Console.Clear();
                                    return true; //Vrátí se true, protože chceme pokračovat ve hře. Nicméně metoda LoadTheGame() ji změnila právě na hru ze souborui
                                }
                                else //Pokud hra nebyla úspěšně načtena
                                {
                                    Console.BackgroundColor = ConsoleColor.Black;
                                    Console.Clear(); //Smaží se grafiky načítání hry 
                                    for (int x = 0; x < 4; x++) //A znovu se vytisknou grafické objekty kromě tlačítek (tlačítka se vytisknou poté na začátku další fáze while cyklu)
                                        Labels[x].Print(x < 2, Reprint); //Oba obdélníky se vytisnou se silnými svislými liniemi. Pokud při tištění není hra nastavena na celou obrazovku, počká se na opravu od uživatele, vymaže se Console a toto menu se vytiskne znovu
                                    break;
                                }
                            case 3: //Pokud je vybráno tlačítko Back to menu
                                {
                                    Console.BackgroundColor = ConsoleColor.Black; 
                                    Console.Clear(); //Smaže se současné menu
                                    return false; //Vrátí se false, což v GameControls nastaví GameAbported na true a vrátí nás do prvního menu
                                }
                            case 4: //Pokud je vybráno tlačítko Quit
                                Environment.Exit(0); //Program se ukončí s číselným kódem 0
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
                            Program.WaitForFix(); //Vyzve uživatele, aby zvětšil hru na celou obrazovku a nepustí ho dál dokud tak neučiní
                            Reprint(); //Znovu se pokusí vymazat menu a vytisknout jej znovu
                        }
                        break;
                }
            }
        }
        public static void SaveTheGame()
        {
            ///Shrnutí
            ///Tato metoda ukládá hru do souboru .txt
            ///Tento soubor bude mít 5 řádků
            ///První řádek: Pozice umístěných vlaječek
            ///Druhý řádek: Pozice umístěných otazníků
            ///Třetí řádek: Pozice min
            ///Čtvrtý řádek: Pozice již otočených políček
            ///Pátý řádek: Parametry hry
            ///Šestý řádek: Současný čas
            string Flags = ""; //První řádek, do kterého se budou zapisovat umístěné vlajky
            string Questionmarks = ""; //Druhý řádek, do kterého se budou zapisovat umístěné otazníky
            string Mines = ""; //Teřtí řádek, kam se budou zapisovat miny
            string Uncovered = ""; //Čtvrtý řádek, kam se budou zapisovat odkrytá políčka
            string Time = (CurrentTime.ElapsedMilliseconds.ToString() + ';' + CurrentTime.ElapsedTicks.ToString()); //Šestý řádek, kam se zapíše čas přes metodu SpecialisedStopwatch.ToString()
            foreach (Tile tile in GameControls.PlayedGame.Minefield) //Projede se celý minefield
            {
                if (tile.Flag) //Pokud má políčko valjku, zapíš se do prvního řádku
                {
                    Flags += tile.MinefieldPosition; //Zapíše se pozice políčka přes metodu Coordinates.ToString() (Ta vrací horizontální pozici a vertikální pozici oddělené čárkou)
                    Flags += ';'; //Mezi jednotlivá políčka se zapíše středník
                }
                if (tile.Questionmark) //To samé proběhne i pro políčka s otazníky
                {
                    Questionmarks += tile.MinefieldPosition;
                    Questionmarks += ';';
                }
                if (tile.Mine) //To samé proběhne i pro políčka s minami
                {
                    Mines += tile.MinefieldPosition;
                    Mines += ';';
                }
                if (!tile.Covered) //A pro políčka, která jsou odkryta
                {
                    Uncovered += tile.MinefieldPosition;
                    Uncovered += ';';
                }
            }
            string Parameters = GameControls.PlayedGame.ToString(); //Do pátého řádku se zapíší herní parametry přes metodu Game.ToString(), která vrátí všechny parametry oddělené čárkou
            string[] ToSave = { Flags, Questionmarks, Mines, Uncovered, Parameters, Time }; //Všechny řádky se připraví do arraye
            string Path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Minesweeper"); //Nyní začneme hledat v AppData složku Minsweeper/
            try //Pokud neexistuje pokusíme se ji vytvořit
            {
                if (!Directory.Exists(Path))
                    Directory.CreateDirectory(Path);
            }
            catch //Pokud se to nepodaří, informujeme uživatele
            {
                Console.WriteLine($"Floder {Path} does not exist and can not be created, please check your acces-rights.");
                Console.WriteLine("Your game can not be saved.");
                return;
            }
            Path = System.IO.Path.Combine(Path, "SavedGames"); //Nyní začneme ve složce Minesweeper heldat složku SavedGames/
            try //Pokud neexistuje pokusíme se ji vytvořit
            {
                if (!Directory.Exists(Path))
                    Directory.CreateDirectory(Path);
            }
            catch //Pokud se to nepodaří, informujeme uživatele
            {
                Console.WriteLine($"Floder {Path} does not exist and can not be created, please check your acces-rights.");
                Console.WriteLine("Your game can not be saved.");
                return;
            }
            string SaveName; //Do tohoto stringu budeme ukládat název hry
            do //Začíná do while cyklus, který trvá dokud hráč nezadá validní název souboru
            {
                Console.SetCursorPosition(Console.WindowWidth / 2 + 8, 15); //Umistí kurzor na pozici, kam se bude brzy psát název
                Console.Write(new string(' ', 50)); //Smaže následujících 50 charů, aby mohl uživatel psát na čistý podklad
                Console.SetCursorPosition(Console.WindowWidth / 2 + 8, 15); //Vrátí kurzor na startovní pozici
                if (((Console.LargestWindowWidth - 5) > Console.WindowWidth) || ((Console.LargestWindowHeight - 3) > Console.WindowHeight)) //Zkontroluje jestli je hra na celou obrazovku, jinak by se mohl vytisknout grafiky špatně
                {
                    Program.WaitForFix(); //Pokud ano zajistí se aby uživatel hru zvětšil
                    Reprint(); //A menu se smaže a znovu vytiskne
                }
                (new PositionedText("Name your save: ", ConsoleColor.Black, Console.WindowWidth / 2 - 8, 15)).Print(false, Reprint); //Nyní vyzveme hráče, aby pojmenoval svoji uloženou hru
                Console.CursorVisible = true; //Zobrazíme kurzor, aby uživatel věděl, že může psát
                SaveName = Console.ReadLine(); //Čteme jméno
                if (SaveName != "") //Pokud byl zadán alespoň jeden char, připojíme na konec příponu .txt
                    SaveName += ".txt";
                if (File.Exists(System.IO.Path.Combine(Path, SaveName))) //Zkontroluje se jestli daný doubor již neexistuje
                {
                    (new PositionedText("Save with this name already exists", ConsoleColor.Black, Console.WindowWidth / 2 - 17, 16)).Print(false, Reprint); //Pokud ano uživatel je o tom informován
                    SaveName = ""; //SaveName se vrátí zpátky na prázdný string, aby tento do while cyklus pokračoval dále
                }
            } while (SaveName == "");
            Path = System.IO.Path.Combine(Path, SaveName); //Když dostaneme validní název tak si zapíšeme již kompletní umístění
            try //Pokusíme se soubor vytvořit a zavřít
            {
                File.Create(Path).Dispose();
            }
            catch //Pokud se nepodaří soubor vytvořit, informujeme uživatele
            {
                Console.WriteLine($"File {Path} can not be created, please check your acces-rights and make sure that your save-name doesn't include forbidden characters. (Like '/')");
                Console.WriteLine("Your game can not be saved.");
            }
            try //Nyní se do něj pokusíme zapsat naše řádky
            {
                File.WriteAllLines(Path, ToSave); //Zapíšeme všechny řádky do souboru
                (new PositionedText(" Your game was saved succesfully. ", ConsoleColor.Black, Console.WindowWidth / 2 - 16, 16)).Print(false, Reprint); //Informujeme uživatele, že vše proběhlo v pořádku. Tato zpráva má stejnou délku jako zprává Save with this name already exists, čili ji v případě potřeby přemaže
            }
            catch (Exception e) //Pokud se zápis nepodaří, informujeme uživatele
            {
                Console.WriteLine($"Your game couldn't be saved in File {Path}");
                Console.WriteLine(e.Message);
            }
            Console.CursorVisible = false; //Když je hra uložena, znovu skyrjeme kurzor, protože již se nic psát nebude
            Console.ReadKey(); //Počkáme než hráč zmáčkne jakoukoliv klávesu, aby se vrátil zpátky do menu pozastavené hry
        }
        public static bool LoadTheGame()
        {
            ///Shrnutí
            ///Tato metoda dá hráči na výběr z uložených souborů ve složce AppData/Roaming/Minesweeper/SavedGames/ a načte vybraný soubor
            ///Vrací bool, podle toho zda byla hra úspěšně načtena nebo ne
            if (((Console.LargestWindowWidth - 5) > Console.WindowWidth) || ((Console.LargestWindowHeight - 3) > Console.WindowHeight)) //Nejprve se zkontroluje velikost okna a případně se počká na opravu od uživatele
            {
                Program.WaitForFix();
                Reprint();
            }
            string Path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Minesweeper");
            Path = System.IO.Path.Combine(Path, "SavedGames"); //Dáme dohromady celou cestu ke složce
            if (!Directory.Exists(Path)) //Zkontrolujeme jestli složka existuje
            {
                (new PositionedText("There are no saved games to be loaded.", ConsoleColor.Black, Console.WindowWidth / 2 - 19, 15)).Print(false, Reprint); //Pokud ne informujeme uživatele
                Console.ReadKey(true);
                return false; //A vrátí se false, protože hra nebyla načtena
            }
            FilesAsText = new List<PositionedText>(); //Pokud složka existuje vytvoří se seznam, do kterého se vloží grafické interpretace souborů
            string[] saveGameFiles = Directory.GetFiles(Path); //Nejprve získáme názvy souborů ze složky jako array stringů
            for (int x = 0; x < saveGameFiles.Length; x++)
                FilesAsText.Add(new PositionedText(System.IO.Path.GetFileNameWithoutExtension(saveGameFiles[x]), ConsoleColor.Black, Console.WindowWidth / 2 - 10, 29 + x)); //Poté z každho stringu vytvoříme PositionedText, pozice textu záleží na jeho pořadí ve složce a délce názvu
            if (saveGameFiles.Length == 0) //Pokud nezískáme žádné soubory, opět informujeme uživatele, že neexistují žádné hry k načtení
            {
                (new PositionedText("There are no saved games to be loaded.", ConsoleColor.Black, Console.WindowWidth / 2 - 19, 15)).Print(false, Reprint);
                Console.ReadKey(true);
                return false; //A vrátí se false, protože hra nebyla načtena
            }
            ConsoleKey keypressed = 0; //Nyní už uživatel může vybírat soubor. Budeme tedy číst klávesu
            ChosenFile = 0; //A přednastaví se vybraný soubor na nulu (tedy ten úplně horní)
            while (keypressed != ConsoleKey.Enter) //While cyklus výběru souborů probíhá až dokud není potvrzen Enterem
            {
                for (int x = 0; x < FilesAsText.Count; x++)
                    FilesAsText[x].Print(x == ChosenFile, LoadFilesReprint); //Nejprve se všechny soubory vytisknou, aktuálně zvolený soubor se vytiskne zvýrazněně (bílou barvou). Pokud při tištění není hra nastavena na celou obrazovku, počká se na opravu od uživatele, vymaže se Console a toto menu a soubory se vytisknou znovu
                keypressed = Console.ReadKey(true).Key; //Přečte klávesu od uživatele
                switch (keypressed)
                {
                    case ConsoleKey.UpArrow: //Pokud je zmáčnuté tlačítko šipka nahoru
                        if (ChosenFile != 0) //A zároveň nejsme na souboru na pozici nula (úplně nahoře), posuneme se o jedna nahoru
                            ChosenFile--;
                        break;
                    case ConsoleKey.DownArrow: //Pokud je zmáčknuté tlačítko šipka dolů
                        if (ChosenFile != FilesAsText.Count - 1) //A nejsme na pozici počtu souborů mínus jedna (tedy úplně dole), posuneme se o jedna dolů
                            ChosenFile++;
                        break;
                    case ConsoleKey.Escape: //Pokud hráč zmáčkne Escape vrátí se zpátky do PauseMenu
                        return false; //Nebyla načtena žádná hra, tedy se vrátí false
                    case ConsoleKey.R: //Pokud uživatel zmáčkne R zavolá se metoda LoadFilesReprint
                        try
                        {
                            LoadFilesReprint(); //Tato metoda smaže a znovu vypíše menu i se soubory
                        }
                        catch (ArgumentOutOfRangeException) //Pokud nastane situace, že něco by se mělo tisknout mimo obrazovku
                        {
                            Program.WaitForFix(); //Program počká na opravu
                            LoadFilesReprint(); //A znovu zavolá metodu LoadFilesReprint()
                        }
                        break;
                }
            }
            GameControls.PlayedGame = new Game(File.ReadAllLines(saveGameFiles[ChosenFile])); //Nyní se z vybraného souboru vytvoří hra pomocí konstruktoru
            GameControls.SetLoaded(File.ReadAllLines(saveGameFiles[ChosenFile])); //Zároveň se nastavení nahrají i do GameControls (jako Počet otočených políček nebo nové barvy)
            int[] Parameters = GameControls.PlayedGame.GetParameters(); //Získají se parametry nové hry a pošlou se do Diffswitcheru
            if (Parameters[0] == 10 && Parameters[1] == 10 && Parameters[2] == 10) //Pokud se shodují s nastavením pro obtížnost Easy (Lehkou) tak se DiffSwitcher přepne na Easy
                DiffSwitcher.SwitchTo(0, false, false);
            else if (Parameters[0] == 20 && Parameters[1] == 20 && Parameters[2] == 60) //Pokud se shodují s nastavením pro Medium (Střední) tak se DiffSwitcher přepne na Medium
                DiffSwitcher.SwitchTo(1, false, false);
            else if (Parameters[0] == 30 && Parameters[1] == 30 && Parameters[2] == 180) //Pokud se shodují s nastavením pro Hard (Těžkou) tak se DiffSwitcher přepne na Hard
                DiffSwitcher.SwitchTo(2, false, false);
            else //Pokud se neshodují s žádnou výchozí obtížností
            {
                DiffSwitcher.GameMenus[3] = new CustomGameMenu(Parameters); //Vytvoří se nové CustomGameMenu podle těchto parametrů, zabere třetí pozici v Arrayi obtížností
                DiffSwitcher.SwitchTo(3, false, false); //A DiffSwitcher se na něj přepne
            }
            GameControls.PlayedGame.TilesAndMinesAroundCalculator(); //Proběhne přepočítání políček a min okolo
            return true; //Vrátí se true, protože hra byla úspěšně načtena

        }
        private static void Reprint()
        {
            ///Shrnutí
            ///Tato metoda umožní v případě potřeby vymazat a znovu vytsknout toto menu
            Console.BackgroundColor = ConsoleColor.Black; 
            Console.Clear(); //Současné menu se vymaže
            for (int x = 0; x < 4; x++) //Vytisknou se znovu grafické obkjekty. Pokud při tištění není hra nastavena na celou obrazovku, počká se na opravu od uživatele, vymaže se Console a toto menu se vytiskne znovu
                Labels[x].Print(x < 2, Reprint); //Oba obdélníky se vytisknou se silnými svislými čarami
            for (int x = 0; x < 5; x++) //Znovu se vytisknou měnitelná tlačítka
            {
                SwitchableLabels[x].Print(x == ChosenLabel, Reprint); //Zvýrazní se vybrané tlačítko. Pokud při tištění není hra nastavena na celou obrazovku, počká se na opravu od uživatele, vymaže se Console a toto menu se vytiskne znovu
            }
        }
        private static void LoadFilesReprint()
        {
            ///Shrnutí
            ///Tato metoda také vymaže a znovu vytiskne toto menu, přidá k tomu však také ještě seznam souborů
            Reprint(); //Zavolá se Reprint
            for (int x = 0; x < FilesAsText.Count; x++) //Znovu se vytisknou soubory
                FilesAsText[x].Print(x == ChosenFile, LoadFilesReprint); //Zvýrazní se vybraný soubor. Pokud při tištění není hra nastavena na celou obrazovku, počká se na opravu od uživatele, vymaže se Console a toto menu a soubory se vytisknou znovu
        }
    }
}
