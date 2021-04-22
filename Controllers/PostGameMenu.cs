using System;
using System.Collections.Generic;
using System.IO;

namespace GloriousMinesweeper
{
    class PostGameMenu
    {
        ///Shrnutí
        ///Menu, které se zobrazí po dokonnčení hry
        ///Hráči se zed objeví jestli vyhrál, jeho čas, případně skóre a možnosti kam dále pokračovat
        ///Může hrát znovu, hrát znovu se stejnými parametry (a přeskočit nastavování hry) nebo program ukončit
        ///Pak má ještě možnost, pokud zvítězil a jedná se o dostatečně dobrý výkon uložit skóre do tabulky
        ///Pokud prohrál má možnost podívat se na správné řešení
        private static bool GameWon { get; set; } //Boolean, který jednoduše určuje výhra/prohra
        private static decimal Score { get; set; } //Skóre dosžené hráčem
        private static bool IsHighscore { get; set; } //Boolean, který nám udává jestli je dané skóre dostateně vysoké na to, aby mohlo být zapsáno do tabulky
        private static string Nickname { get; set; } //Přezdívka, se kterou bude případně výkon do tabulky uložen
        private static List<IGraphic> Labels { get; set; } //Seznam grafických objektů
        private static List<PositionedText> SwitchableLabels { get; set; } //Seznam tlačítek
        private static int ChosenLabel { get; set; } //Číslo označující právě vybrané tlačítko
        private static int Position { get; set; } //Pozice, na kterou daný výkon patří
        private static string Path { get; set; } //Cesta ke souboru s uloženými výkony
        public static bool ShowMenu(decimal score, bool won, SpecialisedStopwatch PlayTime) 
        {
            ///Shrnutí
            ///Základní metoda tohoto menu
            ///Vrací bool podle toho, jestli hráč chce hrát znovu nebo ne
            ///Dale se zde dají spustit i další metody (zobrazení řešení nebo uložení skóre)
            if (((Console.LargestWindowWidth - 5) > Console.WindowWidth) || ((Console.LargestWindowHeight - 3) > Console.WindowHeight)) //Nejprve zkontroluje zda je hra zobrazena na celou obrazovku, jinak by se mohly grafiky špatně vytvořit
                Program.WaitForFix(); //Případně počká na opravu od uživatele
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear(); //Vyčistí obrazovku
            GameWon = won; 
            Score = score; //Hodnoty score, won a PlayTime dostane metoda a zapíší se do fieldů
            Labels = new List<IGraphic>(); //Do Labels se ukládají zcela všechny grafické objekty
            SwitchableLabels = new List<PositionedText>(); //Do SwitchableLabels se ukládají tlačítka
            string firstMessage; //První zpráva se liší podle toho zda hráč vyhrál nebo prohrál
            if (won)
                firstMessage = "Congratulations, you've swept the mines.";
            else
                firstMessage = "Mines are victorious";
            Labels.Add(new PositionedText(firstMessage, ConsoleColor.Black, (Console.WindowWidth - firstMessage.Length) / 2, 5)); //První zpráva se přemění do grafického objektu PositionedText
            if (won) //Pokud hráč zvítězil
            {
                string secondMessage = "Your score is " + Score; //Objeví se mu na obrazovce také jeho skóre
                Labels.Add(new PositionedText(secondMessage, ConsoleColor.Black, (Console.WindowWidth - secondMessage.Length) / 2, 7)); //Skóre se také přemění do grafické podoby
            }
            Labels.Add(new Border(0, 1, Console.WindowHeight - 1, Console.WindowWidth, ConsoleColor.Black, ConsoleColor.Gray, false)); //Vytvoří se opět velký obdélnik okolo celé obrazovky
            if (won)
                Labels.Add(new Border((Console.WindowWidth - firstMessage.Length) / 2 - 3, 3, 17, 46, ConsoleColor.Black, ConsoleColor.Gray, false)); //A poté se vytvoří také malý obdelník okolo samotného menu
            else
                Labels.Add(new Border((Console.WindowWidth - 35) / 2 - 3, 3, 17, 41, ConsoleColor.Black, ConsoleColor.Gray, false)); //Jeho šířka se odvijí od toho zda hráč vyhrál či prohrál. V případě výhry je totiž první zpráva pro uživatele delší
            string Time = "Your time: " + PlayTime.ToString(); //Ať už hráč vyhrál či prohrál zobrazí se mu také jeho čas
            Labels.Add(new PositionedText(Time, ConsoleColor.Black, (Console.WindowWidth - Time.Length) / 2, 9)); //Převedený do grafické podoby
            Position = 1; //Nyní začíná výpočet pozice v tabulce, začne se na jedničce
            if (won) //Pokud hráč vyhrál projede se celá tabulka metodou CheckHighscores. Pokud je skóre výkonu v tabulce vyšší přičte se jednička. Pokud se hráč dostane na jedenáct nejedná se o highscore. Pokud je ale v první desítce může si skóre uložit
                IsHighscore = CheckHighscores();
            else //Pokud nevyhrál, tak se samozřejmě nemůže jednat o highscore
                IsHighscore = false;
            if (IsHighscore) //Pokud tedy vyhrál a dostane se do první desítky
            {
                Labels.Add(new PositionedText("Save Highscore", ConsoleColor.Black, (Console.WindowWidth - 14) / 2, 11)); //Přidá se tlačítko Save Highscore (Uložit skóre)
                SwitchableLabels.Add(new PositionedText("Save Highscore", ConsoleColor.Black, (Console.WindowWidth - 14) / 2, 11)); //Přidá se jak mezi všechny grafické objekty, tak i mezi tlačítka
            }
            else if (!GameWon) //Pokud hráč prohrál, má možnost podívat se na správné řešení
            {
                Labels.Add(new PositionedText("View minefield", ConsoleColor.Black, (Console.WindowWidth - 14) / 2, 11)); //Přidá se tlačítko View Minefield (Zobrazit minové pole)
                SwitchableLabels.Add(new PositionedText("View minefield", ConsoleColor.Black, (Console.WindowWidth - 14) / 2, 11)); //Přidá se jak mezi všechny grafické objekty, tak i mezi tlačítka
            }
            else //Pokud hráč sice vyhrál, ale nejedná se o zápis do tabulky, nezobrazí se mu na této pozici nic a tato pozice v seznamech se vyplní null
            {
                Labels.Add(null);
                SwitchableLabels.Add(null);
            }
            Labels.Add(new PositionedText("Play again", ConsoleColor.Black, (Console.WindowWidth - 10) / 2, 13));
            SwitchableLabels.Add(new PositionedText("Play again", ConsoleColor.Black, (Console.WindowWidth - 10) / 2, 13));
            Labels.Add(new PositionedText("Play again with the same parameters", ConsoleColor.Black, (Console.WindowWidth - 35) / 2, 15));
            SwitchableLabels.Add(new PositionedText("Play again with the same parameters", ConsoleColor.Black, (Console.WindowWidth - 35) / 2, 15));
            Labels.Add(new PositionedText("Quit", ConsoleColor.Black, (Console.WindowWidth - 4) / 2, 17));
            SwitchableLabels.Add(new PositionedText("Quit", ConsoleColor.Black, (Console.WindowWidth - 4) / 2, 17)); //Nyní se do obou seznamů přidají ostatní tlačítka
            
            ChosenLabel = 1; //Výchozí zvolené tlačítko je tlačítko na pozici jedna (Play Again – Hrát Znovu). Je to proto, že na pozici nula se může nacházet null, pokud hráč zvítězil, ale nejedná se o nejlepší výkon
            return EnableSwitch(); //Nyní se zavolá metoda EnableSwitch() která umožní přepínat mezi tlačítky. Tato metoda také vrací bool, jestli hráč chce hrát znovu, či ne
        }
        private static bool CheckHighscores()
        {
            ///Shrnutí
            ///Tato metoda projede celou tabulku nejlepších výkonů a spočítá, pozici na kterou by se tento výkon zařadil
            ///Pokud jsou všehny výkony v tabulce lepší než tento a pořadí by tedy bylo 11 nebo pokud je skóre nižší než 1 (čili hráč pravděpodobně použil solve) vrátí se false 
            ///Jinak se vrátí true, což znamená že se jedná o výkon, který může být do tabulky zařazen
            if (Score < 1) //Pokud je skóre velice nízké (způsobeno pravděpodobně auto-solvem), tak nemůže být výkon zapsán do tabulky a vrátí se false
                return false;
            Path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Minesweeper"); //Nyní se dostaneme do složky AppData/Romaing/Minesweeper/
            try //Pokud složka neexistuje pokusíme se jí vytvořit
            {
                if (!Directory.Exists(Path))
                    Directory.CreateDirectory(Path);
            }
            catch //Pokud se to nepodaří informujeme uživatele
            {
                Console.WriteLine($"Floder {Path} does not exist and can not be created, please check your acces-rights.");
                Console.WriteLine("Your score can not be saved.");
                Console.ReadKey(true);
                return false; //A vrátí se false, protože nemůžeme skóre uložit
            }
            Path = System.IO.Path.Combine(Path, "highscores.txt"); //Nyní se pokusíme dostat přímo do souboru, kde jsou uloženy nejlepší výkony
            try //Pokud soubor neexistuje, tak se jej pokusíme vytvořit
            {
                if (!File.Exists(Path))
                {
                    File.Create(Path).Dispose();
                    return true; //Pokud dosud soubor neexistoval, tak je jisté, že skóre se má uložit na první pozici
                }
            }
            catch //Pokud se soubor nepodaří vytvořit informujeme uživatele
            {
                Console.WriteLine($"File {Path} does not exist and can not be created, please check your acces-rights.");
                Console.WriteLine("Your score can not be saved.");
                Console.ReadKey(true);
                return false; //A vrátí se false, protože nemůžeme skóre uložit
            }
            try //Pokusíme se nyní přečíst řádky uložené v souboru
            {
                string[] allLines = File.ReadAllLines(Path); //Uložíme všechny rádky do arraye stringů
                foreach (string line in allLines) //Projedeme všechny stringy v arrayi
                {
                    string[] parts = line.Split("   "); //Rozdělíme podle tří mezer (čímž oddělujeme přezdívku od skóre)
                    if (Score < Decimal.Parse(parts[1])) //Nyní porovnáme skóre této hry s Decimal.Parse ze substringu skóre z řádku
                        Position++; //Pokud je skóre této hry nižší, tak se zvýší pozice o jedna
                    else
                        return true; //Pokud je skóre této hry vyšší, tak se tato metoda vrátí s true, protože tento výkon může být zařazen do tabulky
                }
                if (Position == 11) //Pokud se Position dostane až na 11 (všechny výkony v tabulce jsou lepší a je jich již 10), tak se vrátí false (skóre nemůže být zapsáno do tabulky, protože není dostatsečně vysoké
                    return false;
                else //Jinak se vrátí true
                    return true;
            }
            catch (Exception e) //Pokud se nepodaří soubor přečíst
            {
                Console.WriteLine($"File {Path} can not be reached.");
                Console.WriteLine(e.Message);
                Console.WriteLine("Your score can not be saved."); //Informujeme uživatele
                return false; //A vrátí se false, protože nemůžeme skóre uložit
            }
        }
        private static bool EnableSwitch()
        {
            ///Shrnutí
            ///Tato metoda umožní hráči dělat akce v tomto menu
            ///Vrací bool, podle toho zda má hráč zájem hrát znovu (true: hrát znovu/false: konec hry)
            foreach (IGraphic label in Labels) //Nejprve se vytisknou všechny grafické objekty,
            {
                if (label == null) //které nejsou null
                    continue;
                label.Print(label.GetType() == (typeof(Border)), Reprint); //Oba bordery se vytisknou se silnými svislými liniemi. Pokud při tištění není hra nastavena na celou obrazovku, počká se na opravu od uživatele, vymaže se Console a vytiskne se menu znovu
            }
            ConsoleKey keypressed; //Budeme opět číst klávesu od uživatele
            do //Začne do while cyklus, který trvá dokud uživatel nezmáčkne Enter
            {
                for (int x = 0; x < 4; x++) //Nejprve se vytisknou všechny grafické objekty,
                {
                    if (SwitchableLabels[x] == null) //které nejsou null
                        continue;
                    SwitchableLabels[x].Print(x == ChosenLabel, Reprint); //Vybrané tlačítko se vytiskne zvýrazněně (bílou barvou). Pokud při tištění není hra nastavena na celou obrazovku, počká se na opravu od uživatele, vymaže se Console a vytiskne se menu znovu
                }
                keypressed = Console.ReadKey(true).Key; //Nyní přečteme klávesu od uživatele
                switch (keypressed)
                {
                    case ConsoleKey.UpArrow: //Pokud uživatel zmáčkne šipku nahoru
                        if (ChosenLabel != 0 && SwitchableLabels[ChosenLabel - 1] != null) //Ověří se, zda již nejsme úplně nahoře (Buď se ChosenLabel rovná nule nebo se tlačítko o jedna výš rovná null)
                            ChosenLabel--; //Pokud nejsme, sníží se ChosenLabel o jedna
                        break;
                    case ConsoleKey.DownArrow: //Pokud uživatel zmáčkne šipku dolů
                        if (ChosenLabel != 3) //Pokud se nenacházíme již úplně dole (na pozici tři), zvýší se ChosenLabel o jedna
                            ChosenLabel++;
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
            } while (keypressed != ConsoleKey.Enter); //Když hráč zmáčkne Enter tak se pokračuje podle toho, které tlačítko bylo vybráno
            switch (ChosenLabel)
            {
                case 0: //Na pozici nula může být (v případě výhry) SaveHighscore (Uložit skóre), nebo (v případě prohry) View Minefield (Zobrazit minové pole)
                    if (GameWon) //Pokud byla hra vyhraná, tak se jde uložit skóre
                    {
                        SwitchableLabels[0].Print(false, Reprint); //Přetiskne toto tlačítko bez zvýraznění
                        SaveHighscore(); //Zavolá se metoda SaveHighscore(), která uloží hru
                        SwitchableLabels[0] = null; //Toto tlačítko se změní na null, aby nebylo možné toto skóre ukládat znovu
                        Labels[5] = null;
                        ChosenLabel = 1; //Chosen Label se automaticky změní na 1, aby nebylo vybrané tlačítko nula
                    }
                    else //Pokud byla hra prohraná, tak se zobrazí znovu Hrací plocha s vyznačenými minami vlaječkou
                    { 
                        if (((Console.LargestWindowWidth - 5) > Console.WindowWidth) || ((Console.LargestWindowHeight - 3) > Console.WindowHeight)) //Nejprve zkontroluje zda je hra zobrazena na celou obrazovku, jinak by se mohly grafiky špatně vytvořit
                            Program.WaitForFix(); //Případně počká na opravu od uživatele
                        GameControls.PlayedGame.PrintMinefield(true); //Vytiskne se znovu hrací plocha, ale všechny políčka s minou se označí vlaječkou
                        (new Border(new Coordinates(GameControls.PlayedGame.Minefield[0, 0].Position, -2, -1), GameControls.PlayedGame.VerticalTiles + 2, 2 * (GameControls.PlayedGame.HorizontalTiles + 2), ConsoleColor.Black, ConsoleColor.White, false)).Print(true, Reprint); //Vytvoří se rámeček okolo hrací plochy a okamžitě se vytiskne. Pokud při tištění není hra nastavena na celou obrazovku, počká se na opravu od uživatele, vymaže se Console a vytiskne se menu znovu
                        (new Border(0, 1, Console.WindowHeight - 1, Console.WindowWidth, ConsoleColor.Black, ConsoleColor.Gray, false)).Print(true, Reprint); //Vytvoří se rámeček okolo obrazovky a okamžitě se vytiskne. Pokud při tištění není hra nastavena na celou obrazovku, počká se na opravu od uživatele, vymaže se Console a vytiskne se menu znovu
                        Console.ReadKey(); //Hrací plocha zůstane zobrazená dokud uživatel nezmáčkne klávesu
                        Console.BackgroundColor = ConsoleColor.Black; //Poté se celá Console vyčistí
                        Console.Clear();
                    }
                    return EnableSwitch(); //A vrátíme se zpátky na začátek této metody
                case 1: //Pokud je zmáčknuto Play Again, tak se chceme vrátit zpátky do menu, kde nastavujeme hru
                    foreach (GameMenu menu in DiffSwitcher.GameMenus) //U všech menu nastavíme ChosenLine zpátky na nulu
                    {
                        if (menu == null) //Pokud samozřejmě není menu == null
                            continue;
                        menu.ChooseLine(0);
                    }
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Clear(); //Console se vyčistí a přejdeme zpátky do DiffSwitcheru
                    DiffSwitcher.PrintGraphics(true); //DiffSwitcher vytiskne všechny své grafiky
                    DiffSwitcher.SwitchTo(DiffSwitcher.ChosenMenu, true); //Přejdeme na vybrané menu
                    GameControls.PlayedGame = DiffSwitcher.EnableSwitch(); //Zavoláme metodu EnableSwitch() pomocí, které vytvoříme hru, kterou vložíme do GameControls.PlayedGame()
                    return true; //Vrátí se true a hráč poté může hru rovnou začít hrát
                case 2: //Pokud je zmáčknuto Play Again with the same parameters, tak se okamžitě vytvoří hra se stejnými parametry
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Clear(); //Vyčistí se Console
                    (new PositionedText("Loading...", ConsoleColor.Black, (Console.WindowWidth - 10) / 2, 12)).Print(false, Reprint); //Vytvoří a vytiskne text Loading... U větších minefieldů může tvorba hry chvilku trvat, čili aby bylo jasné, že se nejdná o chybu a skutečně se něco děje
                    int[] parameters = GameControls.PlayedGame.GetParameters(); //Získají se parametry ze hry, která byla právě dohrána
                    GameControls.PlayedGame = new Game(parameters); //A vytvoří se pomocí nich nová hra
                    return true; //Vrátí se true a hráč poté může hru rovnou začít hrát
                case 3: //Pokud je zmáčknuto Quit, tak se program ukončí
                    Environment.Exit(0); //Program se ukončí s kódem 0
                    return false;
            }
            return false;
        }
        private static void SaveHighscore()
        {
            ///Shrnutí
            ///Tato metoda uloží Highscore do tabulky se zvolenou přezdívkou
            PositionedText nickname = new PositionedText("Enter your Nickname: ", ConsoleColor.Black, (Console.WindowWidth - 40) / 2, 21); //Grafika vyzve hráče, aby zapsal přezdívku, pod kterou bude skóre v tabulce uloženo
            nickname.Print(true, Reprint);
            do
            {
                Console.SetCursorPosition(Console.WindowWidth / 2 + 1, 21); //Umistí kurzor na pozici, kam se bude brzy psát název
                Console.Write(new string(' ', 50)); //Vymaže následujících 50 charů, aby hráč mohl psát na volné místo
                Console.SetCursorPosition(Console.WindowWidth / 2 + 1, 21); //Znovu umistí kurzor na pozici
                Console.CursorVisible = true; //Zviditelní kurzor, aby bylo jasné, že se očekává zadávání textu
                Console.ForegroundColor = ConsoleColor.White; //Nastaví se barva textu na bílou bvarvou
                Nickname = Console.ReadLine(); //Přečte zadaný text a uloží jej do fieldu Nickname
                if (Nickname.Contains("   ") || Nickname.Length > 20 || Nickname == "") //Pokud je přezdívka příliš dlouhá nebo příliš krátká a nebo obsahuje tři mezery (kterými se odděluje přezdívka od skóre), tak je uživateli zobrazeno, že přezdívka je neplatná a pokusí se jii zadat znovu
                {
                    (new PositionedText("Invalid or too long nickname!", ConsoleColor.Black, Console.WindowWidth / 2 - 9, 23)).Print(false, Reprint);
                    Nickname = "";
                }
            } while (Nickname == ""); //Tato metoda trvá dokud není zadána validní přezdívka
            Console.CursorVisible = false; //Znovu se skryje text, protože už se žádný text zadávat nebude
            try //Pokusíme se upravit seznam nejlepší výkonů
            {
                int[] gameParameters = GameControls.PlayedGame.GetParameters(); //Získáme parametry dohrané hry
                string entry = $"{Nickname}   {Score}   {gameParameters[0]}×{gameParameters[1]} minefield with {gameParameters[2]} mines in {GameControls.CompletionTime}"; //Ty se společně s Přezdívkou a skóre zapíší do stringu, který se bude zapisovat do tabulky
                List<string> currentLeaderboard = new List<string>(File.ReadAllLines(Path)); //Nejprve se získá současný seznam
                currentLeaderboard.Insert(Position - 1, entry); //Na správnou pozici se vloží tento string
                if (currentLeaderboard.Count == 11) //Pokud přesáhne počet zapsaných výkonů 10
                    currentLeaderboard = currentLeaderboard.GetRange(0, 10); //Tak se odstraní poslední výkon
                File.WriteAllLines(Path, currentLeaderboard); //Nový seznam se zapíše zpátky do souboru
                Program.ShowLeaderboards(); //Po zapsání do tabulky zobrazíme hráči nově vypadající tabulku nejlepších výkonů
            }
            catch (Exception e) //Pokud dojde k chybě, informujeme uživatele
            {
                Console.WriteLine($"The file {Path} couldn't be changed. Please check your acces-rights");
                Console.WriteLine("Your score couldn't be saved");
                Console.WriteLine(e.Message);
            }
            return;
        }
        private static void Reprint()
        {
            ///Shrnutí
            ///Metoda, která smaže a znovu vytiskne menu. Používá se v případě, že uživatel zmenšil nedopatřením okno, aby se mu povedlo srovnat vytištěné objekty na svá místa.
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear(); //Nejprve smaže celou Console
            foreach (IGraphic label in Labels) //Poté vytiskne všechny grafické objekty
            {
                if (label == null) //které nejsou null
                    continue;
                label.Print(label.GetType() == (typeof(Border)), Reprint); //Oba obdélníky s vytisknou se silnými svislými liniemi
            }
            for (int x = 0; x < 4; x++) //Vytisknou se všechna tlačítka
            {
                if (SwitchableLabels[x] == null) //která nejsou null
                    continue;
                SwitchableLabels[x].Print(x == ChosenLabel, Reprint); //Pokud se jedná o vybrané tlačítko, tak se zvýrazní (vytiskne bíle)
            }
        }
    }
}
