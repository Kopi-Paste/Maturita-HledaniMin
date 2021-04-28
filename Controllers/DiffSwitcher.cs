using System;
using System.Collections.Generic;

namespace GloriousMinesweeper
{
    static class DiffSwitcher
    {
        ///Shrnutí
        ///Statická třída DiffSwitcher umožňuje přepínat obtížnosti a také vytvořit obtížnost vlastní individuálním navolením počtu políček a min. Také umožňuje nastavit barvy ve hře, nastavení barev jsou společná pro všechny obtížnosti.
        
        public static GameSetting[] Colours { get; private set; } //Field ve kterém jsou nastavení barev, která jsou společná pro všechny obtížnosti
        public static GameMenu[] GameMenus { get; private set; } //Field ve kterém jsou uloženy samotné obtížnosti
        private static List<IGraphic> Labels { get; set; } //Field ve kterém jsou uloženy grafické objekty
        public static int ChosenMenu { get; private set; } //Field který označuje zvolené menu
                
        public static void StartMenu(bool resetColours)
        {
            ///Shrnutí
            ///Počáteční metoda menu. Zde se nastaví výchozí nastavení a menu se vytiskne
             
            if (((Console.LargestWindowWidth - 5) > Console.WindowWidth) || ((Console.LargestWindowHeight - 3) > Console.WindowHeight)) //Nejprve zajistí, že je hra na celou obrazovku, jinak by mohly grafické objekty mít špatnou pozici.
                Program.WaitForFix(); //Pokud není, počká se dokud to uživatel nenapraví
            GameMenus = new GameMenu[4]; //Vytvoří se Array pro čtyři různá nastavení
            GameMenus[0] = new GameMenu("Easy", Difficulties.Easy); //První (nulté) místo zabere lehká obtížnost
            GameMenus[1] = new GameMenu("Medium", Difficulties.Medium); //Druhé (první) místo zabere střední obtížnost
            GameMenus[2] = new GameMenu("Hard", Difficulties.Hard); //Třetí (druhé) místo zabere těžká obtížnost
            GameMenus[3] = null; //Čtvrté (třetí) místo zůstane zatím prázdné, zde se později bude vytvářet vlastní nastavení (objekt třídy CustomGameMenu), které může uživatel upravovat
            ChosenMenu = 0; //Výchozí menu je jednoduchá obtížnost, a proto se nastaví field ChosenMenu na nulu
            SetDefault(resetColours); //Pokud je vstupní hodnota resetColours true, tedy pokud otevíráme menu nastavení hry poprvé, tak se zavolá metoda SetDefault, kde se nastaví prvotní nastavení pro barvy
            SwitchTo(0, true); //Následně se zavolá metoda SwitchTo, která zobrazí menu Lehké obtížnosti a zvýrazní název menu, aby uživatel věděl, že nyní může přepínat mezi obtížnostmi
            Game newGame = EnableSwitch(); //Nyní se zavolá metoda EnableSwitch, která umožní vybírat mezi obtížnostmi a vytvářet vlastní obtížnost. Tato metoda vrací objekt typu Game
            if (newGame == null) //Pokud uživatel opustí metodu, aniž by hru vytvořil tak se vrátíme zpátky do prvního menu, kde je možné toto menu opět zobrazit nebo hru ukončit
                return;
            GameControls.PlayedGame = newGame; //V opačném případě se do další statické třídy, tedy do GameControls načte hraná hra a bude možné již brzy ji začít hrát
            Console.BackgroundColor = ConsoleColor.Black; 
            Console.Clear(); //Menu se vymaže
            bool UserWantsToPlayAgain; //Tento boolean nám bude určovat zda chce hrát hráč znovu
            do //Začíná cyklus do while, který probíhá, dokud platí, že hráč chce hrát znovu
            {
                if (GameControls.PlayedGame == null) //Pokud se stane, že se nevytvoří žádná hra, protože uživatel zruší vybírání v menu, tak se metoda vrátí zpátky do prvního menu
                    return;
                GameControls.PlayedGame.PrintMinefield(); //Jinak se zavolá metoda, která vytiskne celou herní plochu a připravujeme se k samotnému hraní hry
                GameControls.PlayedGame.TilesAndMinesAroundCalculator(); //Nyní se zavolá metoda, která pro každý Tile vytvoří seznam tilů okolo a spočítá, kolik z nich má minu
                GameControls.SetDefault(); //Nakonec se ještě nastaví výchozí nastavení. Například se vytvoří a vytisknou grafiky nebo se nastaví důležité Fieldy na nulu (například počet odkrytých políček nebo počrt umístěných vlajek)
                bool gameWon = GameControls.Gameplay(out decimal score, out SpecialisedStopwatch playTime); //Nyní se už dostáváme k samotnému Gameplayi. Tato metoda nám vrátí tři hodnoty: boolean zda byla hra vyhrána, skóre hráče a čas, který hráč potřeboval na vyřešení této hry
                if (score != -1) //Pokud hráč opustí hru v jejím průběhu, vrátí se nám skóre -1, v takovém případě se vrátíme zpátky do hlavního menu.
                    UserWantsToPlayAgain = PostGameMenu.ShowMenu(score, gameWon, playTime); //Jinak se dostaneme do další statické třídy PostGameMenu, kde si může hráč zahrát znovu, uložit skóre, podívat se na hrací plochu, zahrát si znovu nebo hru ukončit
                else
                    UserWantsToPlayAgain = false;
            } while (UserWantsToPlayAgain);
        }
        public static void SwitchMenu(ConsoleKey consoleKey)
        {
            ///Shrnutí
            ///Tato metoda může změnit menu, které je zrovna vybráno a ukazuje se na obrazovce. Šipka doleva nás posune o jedno doleva (například z Medium na Easy), šipka doprava nás podune o jedno doprava (například z Medium na Hard)
            if (consoleKey == ConsoleKey.LeftArrow && ChosenMenu != 0) //Pokud je tedy zmáčknutá šipka doleva a zároveň není už vybráno nejnižší menu (Easy), tak se posuneme o jedno dolů
            {
                ChosenMenu--; //Samotné snížení hodnoty
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Clear(); //Aktuální menu se smaže
                PrintGraphics(true); //Nyní se vytisknou znovu grafiky
                GameMenus[ChosenMenu].PrintMenu(true); //A následně se vytiskne i samotné menu
            }
                
            else if (consoleKey == ConsoleKey.RightArrow) //V tomto případě se zatím nekoukáme na to, jestli nejsme na maximu a necháme číslo zvednout
            {
                ChosenMenu++; //Zvedne se tedy číslo a nyní se teprve podíváme, co se stalo
                if (ChosenMenu < GameMenus.Length) //Číslo ponecháme pokud field ChosenMenu nepřesáhne 4
                    if (GameMenus[ChosenMenu] != null) //A zároveň pokud vybrané menu není null (což by se mohlo stát, pokud by ještě nebylo vytvořeno CustomGameMenu na pozici 3)
                    {
                        Console.BackgroundColor = ConsoleColor.Black; //V takovém případě opět následuje vymazání starého menu
                        Console.Clear();
                        PrintGraphics(true); //Opět se zde vytisknou grafiky
                        GameMenus[ChosenMenu].PrintMenu(true); //A poté i samotné menu
                    }
                    else
                        ChosenMenu--; //V opačných případech
                else
                    ChosenMenu--; //    se číslo sníží zpět, na hodnotu, na které bylo původně
            }
            GameMenus[ChosenMenu].ChooseLine(0); //Ve vybraném menu se vybere řádka na pozici nula
        }
        public static void SwitchTo(int number, bool highlightName, bool immediatePrint = true)
        {
            ///Shrnutí
            ///Tato metoda nás automaticky přesune na číslo na vybrané pozici. Dále ještě dostáváme bool jestli se má nově vytisklé nenu vytisknout (to je defaultně nastaveno na false, protože tak tomu bude ve většině případů), dále ještě dostáváme bool jestli se má v tomto menu zvýraznit název jména
            ChosenMenu = number; //Vybrané menu se nastaví na zvolené číslo
            if (immediatePrint) //Následně se podle jednoho boolu vytiskne nebo nevytiskne menu
                GameMenus[ChosenMenu].PrintMenu(highlightName); //A podle druhého se buď zvýrazní jméno nebo nezvýrazní
        }

        public static Game EnableSwitch()
        {
            ///Shrnutí
            ///Tato metoda nám umožňuje měnit menu, měnit nastavení barev a vytvářet vlastní obtížnost
            ///Metoda nám vrátí objekt typu Game, který se načte do fieldu GameControls.PlayedGame, kde bude uživatel tuto hru hrát
            ConsoleKey keypressed; //Budeme opět číst klávesy od uživatele
            do //Začíná do while cyklus, který trvá, dokud uživatel nezmáčkne Enter, čímž potvrdí aktuální nastavení a z těchto nastavení vytvoří objekt Game
            {
                keypressed = Console.ReadKey(true).Key; //Zde již přímo čteme klávesu od uživatele
                switch (keypressed)
                {
                    case ConsoleKey.LeftArrow: //Pokud uživatel zmáčkne klávesu doleva
                        SwitchMenu(keypressed); //zavolá se metoda SwitchMenu s klávesou doleva, což nás posune o jedno menu doleva, pokud není již vybrané menu Easy (tedy to nejnižší)
                        break;
                    case ConsoleKey.RightArrow: //Pokud uživatel zmáčkne klávesu doleva
                        SwitchMenu(keypressed); //zavolá se metoda SwitchMenu s klávesou doprava, což nás posune o jedno menu doprava, pokud není již vybrané menu Hard nebo Custom (tedy to nejvyšší existující)
                        break;
                    case ConsoleKey.DownArrow: //Pokud uživatel zmáčkne klávesu dolů, tak opustíme měnění menu a začneme měnit nastavení barev a počtů políček a min 
                        PrintMenuName(false); //Nejprve se přetiskne jméno vybraného menu, tentokrát však již bez zvýraznění
                        int keypressedint = GameMenus[ChosenMenu].MenuAction(); //Zavolá se metoda MenuAction(), skrz kterou se mohou měnit právě nastavení jednoho menu. Tato metoda vrací číselné kódy: 1 znamená, že se uživatel vrátil zpátky na měnění obtížností; 0 znamená, že uživatel potvrdil Enterem; -1 znamená, že uživatel vytvořil nové CustomGameMenu a nyní se na něj musí přepnout a pokračovat dál; -2 znamená že uživatel zmáčknul Escape a tedy se chce vrátit zpátky do prvního menu
                        switch (keypressedint)
                        {
                            case 1: //Vrátí se jedna -> vracíme se zpátky na přepínání obtížností
                                PrintMenuName(true); //Zvýrazní se název menu, aby uživatel věděl, že nyní znovu mění obtížnosti
                                break; //Vrátíme se zpět do této metody
                            case 0: //Vrátí se nula -> uživatel zmáčkl Enter, chceme ukončit výběr a načíst současná nastavení
                                keypressed = ConsoleKey.Enter; //Enter se tedy „zmáčkne“ i v rámci této metody, což ukončí tento do while cyklus
                                break;
                            case -1: //Vrátí se mínus jedna -> uživatel vytvořil nové CustomGameMenu, na které se musíme přepnout
                                SwitchTo(3, false); //Samotné CustomGameMenu se vytvoří v rámci metody MenuAction, nyní na něj pouze přepneme DiffSwitcher. Víme že vybraným řádkem není určitě menu, a proto nezvýrazňujeme název menu
                                PrintGraphics(true); //Znovu se vytisknou grafiky, včetně rámečků
                                keypressedint = GameMenus[ChosenMenu].MenuAction(); //Na nově vzniklém menu se znovu spustí metoda ManuAction, která již tentokrát ale probíhá na override CustomGameMenu ne na virtual GameMenu, čili nemůže již vrátit mínus jedna
                                switch (keypressedint)
                                {
                                    case 1: //Vrátí se jedna -> vracíme se zpátky na přepínání obtížností
                                        PrintMenuName(true); //Zvýrazní se název menu, aby uživatel věděl, že nyní znovu mění obtížnosti
                                        break; // Vrátíme se zpět do této metody
                                    case 0: //Vrátí se nula -> uživatel zmáčkl Enter, chceme ukončit výběr a načíst současná nastavení
                                        keypressed = ConsoleKey.Enter; //Enter se tedy „zmáčkne“ i v rámci této metody, což ukončí tento do while cyklus
                                        break;
                                    case -2:
                                        return null; //Vrátí se mínus dva -> uživatel zmáčkl Escape, vrátí se hra null, čili se vracíme zpátky do prvního menu
                                }
                                break;
                            case -2: //Vrátí se mínus dva -> uživatel zmáčkl Escape, vrátí se hra null, čili se vracíme zpátky do prvního menu
                                return null;
                        }
                        break;
                    case ConsoleKey.Escape: //Uživatel zmáčkl Escape, vrátí se hra null, čili se vracíme zpátky do prvního menu
                        return null;
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
            } while (keypressed != ConsoleKey.Enter);
            int[] parameters = new int[10]; //Nyní se začnou sbírat vybrané parametry, pomocí kterých se vytvoří samotná hra
            for (int x = 0; x < 10; x++) //Na každé misto v Arrayi intů se umístí jeden parametr
            {
                if (x <= 2)
                    parameters[x] = GameMenus[ChosenMenu].GameSettings[x].SettingValue.Number; //První tři místa (tedy Tiles a Mines) se obsadí hodnotami z vybraného menu
                else
                    parameters[x] = Colours[x - 3].SettingValue.Number; //Zbylá místa se doplní hodnotami z globálních barev. Použije se tady „posun o tři pozice“
            }
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear(); //Vymaže se toto menu a chystá se tisk hrací plochy
            (new PositionedText("Loading...", ConsoleColor.Black, (Console.WindowWidth - 10) / 2, 12)).Print(false, Reprint); //Vytvoří a vytiskne text Loading... U větších minefieldů může tvorba hry chvilku trvat, čili aby bylo jasné, že se nejdná o chybu a skutečně se něco děje
            return new Game(parameters); //Nyní už vytvoříme samotnou hru pomocí konstruktoru a sebraných hodnot z tohoto nastavování
        }
        public static void SetDefault(bool resetColours)
        {
            ///Shrnutí
            ///Tato metoda nastaví výchozí nastavení. Pokud se jedná o první spuštění, tak se nastaví i výchozí barvy
            PositionedText secondLine = new PositionedText("Game settings:", ConsoleColor.Black, (Console.WindowWidth - 14) / 2, 4);
            PositionedText thirdLine = new PositionedText("Use arrow keys to operate and enter to confirm", ConsoleColor.Black, (Console.WindowWidth - 46) / 2, 5);
            Border GameMenuBigBorder = new Border(0, 1, Console.WindowHeight - 1, Console.WindowWidth, ConsoleColor.Black, ConsoleColor.Gray, false);
            Border GameMenuSmallBorder = new Border(Console.WindowWidth / 2 - 37, 2, 31, 74, ConsoleColor.Black, ConsoleColor.White, false);
            Labels = new List<IGraphic>() { secondLine, thirdLine, GameMenuBigBorder, GameMenuSmallBorder }; //Nejprve se vytvoří grafické objekty a uloží se do fieldu Labels
            if (resetColours) //Pokud se jedná o první spuštění, tak se vytvoří field Colours, kam se uloží výchozí nastavení
            {
                Colours = new GameSetting[7];
                Colours[0] = new GameSetting("Covered tiles colour", 10, true, false, 17); //Barva neotočených políček: Zelená
                Colours[1] = new GameSetting("Covered tiles secondary colour", 2, true, false, 19); //Druhá barva neotočených políček: Tmavá zelená
                Colours[2] = new GameSetting("Uncovered tiles colour", 9, true, false, 21); //Barva otočených políček: Modrá
                Colours[3] = new GameSetting("Uncovered tiles secondary colour", 1, true, false, 23); //Druhá barva otočených políček: Tmavě modrá
                Colours[4] = new GameSetting("Flag colour", 12, true, false, 25); //Barva vlaječek: Červená
                Colours[5] = new GameSetting("Highlighted tile colour", 13, true, false, 27); //Barva vybraného políčka Fialová (Magenta)
                Colours[6] = new GameSetting("Text colour", 14, false, true, 29); //Barva textu: Šedá
                Program.DefaultTextColour = (ConsoleColor)Colours[6].SettingValue.Number; //Šedá barva se nahraje také do fieldu Program.DefaultTextColour, čili všechny texty budou mít šedou barvu
                Program.TakenColours.Clear(); //Restartuje se field Program.TakenColours a nahrají se do něj výchozí barvy
                foreach (GameSetting gameSetting in Colours) //Projede se celý Array
                {
                    Program.TakenColours.Add((ConsoleColor)gameSetting.SettingValue.Number); //A zapíše se vybraná barva
                }
            }
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear(); //Na závěr se vymaže Console
            PrintGraphics(true); //A vytisknou se nově vytvořené grafiky
        }
        public static void SetLoaded(string[] Parameters)
        {
            ///Shrnutí
            ///Tato metoda nastaví nastavená barev, které má načtená hra. Dělá se to proto, aby se vše tisklo podle nově načtených barev.
            ///Parametry dostáváme ve formě stringů, neboť se čtou ze souboru
            for (int x = 0; x < 7; x++)
                Colours[x].ChangeValueTo(Int32.Parse(Parameters[x + 3]), GameControls.Reprint, false); //Nejprve se projedou všechny barvy ve fieldu DiffSwitcheru a nahradí se hodnotami z Arraye stringů. Opět je zde posun o 3 pozice. Hodnoty získáváme pomocí metody Int32.Parse(string)
            Program.TakenColours.Clear(); //Restartuje se field Program.TakenColours a nahrají se do něj nově načtené barvy
            foreach (GameSetting setting in Colours) //Projede se celý Array s novými hodnotami
            {
                Program.TakenColours.Add((ConsoleColor)setting.SettingValue.Number); //A zapíše se nová barva
            }
        }
        public static void PrintMenuName(bool highlight)
        {
            ///Shrnutí
            ///Tato jednoduchá metoda vytiskne název menu. Podle daného boolu se buď vytiskne normální barvou (Program.DefaultTextColour) nebo bílou (zvýraznění)
            GameMenus[ChosenMenu].Name.Print(highlight, Reprint); //Pokud při tištění není hra nastavena na celou obrazovku, počká se na opravu od uživatele, vymaže se Console a vytiskne se toto menu znovu od začátku
        }
        public static void PrintGraphics(bool printBorders)
        {
            ///Shrnutí
            ///Tato kratší metoda vytiskne grafiky z fieldu Labels
            ///Je zde možnost vytisknout jenom texty nebo vytisknout texty i rámečky
            if (printBorders)
                for (int x = 0; x < 4; x++)
                    Labels[x].Print(x == 2, Reprint); //Tiskneme vše, Border na pozici dva (velký obdélník okolo celé obrazovky) bude mít silné svislé linie. Pokud při tištění není hra nastavena na celou obrazovku, počká se na opravu od uživatele, vymaže se Console a vytiskne se toto menu znovu od začátku
            else
                for (int x = 0; x < 2; x++)
                    Labels[x].Print(false, Reprint); //Tiskneme jenom texty. Pokud při tištění není hra nastavena na celou obrazovku, počká se na opravu od uživatele, vymaže se Console a vytiskne se toto menu znovu od začátku
        }
        private static void Reprint()
        {
            ///Shrnutí
            ///Tato metoda umožní v případě potřeby vymazat a znovu vytsknout toto menu
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear(); //Celá Console se vymaže
            PrintGraphics(true); //Nejprve se vytisknou grafiky
            GameMenus[ChosenMenu].PrintMenu(true); //Následně se vytiskne současné menu
        }
    }
}
