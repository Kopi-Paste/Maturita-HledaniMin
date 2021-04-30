using System;
using System.Collections.Generic;

namespace GloriousMinesweeper
{
    static class GameControls
    {
        ///Shrnutí
        ///Hlavní statická třída celého programu. Zde se odehrává samotná hra
        public static Game PlayedGame { get; set; } //Hra, kterou hráč hraje
        public static ChangeableCoordinates CurrentMinefieldPosition { get; private set; } //Současná pozice, na které se hráč v Minefieldu nachází
        public static Tile CurrentTile { get; private set; } //Současné vybrané políčko
        private static bool EndGame { get; set; } //Bool, který udává, zda je již hra dokončena (ať už výhrou nebo prohrou)
        public static PositionedNumber UncoveredTiles { get; private set; } //Počet otočených políček, který je graficky zobrazován uživateli
        public static PositionedNumber NumberOfFlags { get; private set; } //Počet umístěných vlaječek, který je graficky zobrazený uživateli
        private static List<IGraphic> Labels {get; set;} //Seznam grafických obejktů
        public static int IncorrectFlags { get; private set; } //Počet chybně umístěných vlaječek, hráč nemůže zvítězit, pokud se toto číslo nerovná nule. Toto číslo samozřejmě uživatel nevidí
        public static SpecialisedStopwatch CompletionTime { get; set; } //Zde se měří čas, jak dlouho trvá hráči dohrát tuto hru
        public static decimal ScoreMultiplier { get; private set; } //Činitel skóre. Začíná na hodnotě 1, při každém použití Hintu, který nebyl nutný, vydělí se dvěma
        private static bool GameAborted { get; set; } //Bool, který udává zda hráč opustil hru v průběhu
        public static void SetDefault()
        {
            ///Shrnutí
            ///Nastaví původní hodnoty a také grafiky k vytisknutí
            if (((Console.LargestWindowWidth - 5) > Console.WindowWidth) || ((Console.LargestWindowHeight - 3) > Console.WindowHeight)) //Nejprve se ověří, zda je hra na celou obrazovku, protože jinak by se mohly vytvořit grafiky na chybných pozicích
                Program.WaitForFix(); //Počká se tedy, než hráč dá hru na celou obrazovku
            CurrentMinefieldPosition = new ChangeableCoordinates(0, 0, PlayedGame.HorizontalTiles - 1, PlayedGame.VerticalTiles - 1); //CurrentMinefieldPosition se nastaví na 0, 0 a vrchní hranice se nastaví na počet políček - 1
            CurrentTile = PlayedGame.Minefield[0, 0];
            IncorrectFlags = 0;
            UncoveredTiles = new PositionedNumber(0, ConsoleColor.Black, Console.WindowWidth - 34, 6);
            NumberOfFlags = new PositionedNumber(0, ConsoleColor.Black, Console.WindowWidth - 34, 7); 
            CompletionTime = new SpecialisedStopwatch("0", "0"); //Všechny hodnoty se nastaví na nula
            EndGame = false;
            GameAborted = false; //Booleany se nasatví na false
            ScoreMultiplier = 1; //ScoreMultiplier se nastaví na výchozí hodnotu jedna
            Labels = new List<IGraphic>(); //Nyní se vytvoří všechny grafiky
            Labels.Add(new Border(0, 1, Console.WindowHeight - 1, Console.WindowWidth, ConsoleColor.Black, ConsoleColor.Gray, false));
            Labels.Add(new Border(new Coordinates(PlayedGame.Minefield[0, 0].Position, -2, -1), PlayedGame.VerticalTiles + 2, 2 * (PlayedGame.HorizontalTiles + 2), ConsoleColor.Black, ConsoleColor.White, false));
            Labels.Add(new Border(Console.WindowWidth - 54, 5, 4, 23 + (int)Math.Floor(Math.Log10((PlayedGame.HorizontalTiles * PlayedGame.VerticalTiles - PlayedGame.Mines))), ConsoleColor.Black, ConsoleColor.Gray, false));
            Labels.Add(new PositionedText("Uncovered Tiles:", ConsoleColor.Black, Console.WindowWidth - 52, 6));
            Labels.Add(new PositionedText("Placed Flags:", ConsoleColor.Black, Console.WindowWidth - 52, 7));
            Labels.Add(new Border(6, 16, 10, 22, PlayedGame.Uncover, PlayedGame.Cover, true));
            Labels.Add(new Border(6, 28, 10, 22, PlayedGame.UncoverSecondary, PlayedGame.CoverSecondary, true));
            Labels.Add(new Border(31, 16, 10, 22, PlayedGame.Uncover, PlayedGame.Cover, true));
            Labels.Add(new Border(31, 28, 10, 22, PlayedGame.UncoverSecondary, PlayedGame.CoverSecondary, true));
            Labels.Add(new Border(Console.WindowWidth - 54, 16, 28, 50, PlayedGame.Uncover, PlayedGame.Cover, true));
            Labels.Add(new PositionedText("Uncover all tiles", PlayedGame.Uncover, 8, 18));
            Labels.Add(new PositionedText("without mines and", PlayedGame.Uncover, 8, 19));
            Labels.Add(new PositionedText("flag all tiles", PlayedGame.Uncover, 8, 20));
            Labels.Add(new PositionedText("with them!", PlayedGame.Uncover, 8, 21));
            Labels.Add(new PositionedText("Numbers on tiles", PlayedGame.UncoverSecondary, 8, 30));
            Labels.Add(new PositionedText("indicate how many", PlayedGame.UncoverSecondary, 8, 31));
            Labels.Add(new PositionedText("mines are around.", PlayedGame.UncoverSecondary, 8, 32));
            Labels.Add(new PositionedText("Beaware, if you", PlayedGame.Uncover, 33, 18));
            Labels.Add(new PositionedText("try to uncover", PlayedGame.Uncover, 33, 19));
            Labels.Add(new PositionedText("tile with mine", PlayedGame.Uncover, 33, 20));
            Labels.Add(new PositionedText("you will lose.", PlayedGame.Uncover, 33, 21));
            Labels.Add(new PositionedText("If you have a tile", PlayedGame.UncoverSecondary, 33, 30));
            Labels.Add(new PositionedText("marked by a flag", PlayedGame.UncoverSecondary, 33, 31));
            Labels.Add(new PositionedText("you will not be", PlayedGame.UncoverSecondary, 33, 32));
            Labels.Add(new PositionedText("able to uncover", PlayedGame.UncoverSecondary, 33, 33));
            Labels.Add(new PositionedText("the tile.", PlayedGame.UncoverSecondary, 33, 34));
            Labels.Add(new PositionedText("Game controls:", PlayedGame.Uncover, Console.WindowWidth - 52, 18));
            Labels.Add(new PositionedText("Use arrow keys to move around", PlayedGame.Uncover, Console.WindowWidth - 52, 20));
            Labels.Add(new PositionedText("Use Enter to uncover tile", PlayedGame.Uncover, Console.WindowWidth - 52, 22));
            Labels.Add(new PositionedText("First uncover is 100 % safe", PlayedGame.Uncover, Console.WindowWidth - 52, 23));
            Labels.Add(new PositionedText("Use Spacebar to mark tile with flag", PlayedGame.Uncover, Console.WindowWidth - 52, 25));
            Labels.Add(new PositionedText("or questionmark", PlayedGame.Uncover, Console.WindowWidth - 52, 26));
            Labels.Add(new PositionedText("Need help? Use H to get a hint", PlayedGame.Uncover, Console.WindowWidth - 52, 28));
            Labels.Add(new PositionedText("Warning: Doing so in cases when it", PlayedGame.Uncover, Console.WindowWidth - 52, 29));
            Labels.Add(new PositionedText("is not needed will lower your score", PlayedGame.Uncover, Console.WindowWidth - 52, 30));
            Labels.Add(new PositionedText("Or you can use S to let the game solve itself", PlayedGame.Uncover, Console.WindowWidth - 52, 32));
            Labels.Add(new PositionedText("Or Q to solve itself very very quickly", PlayedGame.Uncover, Console.WindowWidth - 52, 33));
            Labels.Add(new PositionedText("You can also use Escape to pause the game", PlayedGame.Uncover, Console.WindowWidth - 52, 35));
            Labels.Add(new PositionedText("or to stop auto-solve", PlayedGame.Uncover, Console.WindowWidth - 52, 36));
            Labels.Add(new PositionedText("Try to solve the game as quickly as possible", PlayedGame.Uncover, Console.WindowWidth - 52, 38));
            Labels.Add(new PositionedText("to achieve the highest score.", PlayedGame.Uncover, Console.WindowWidth - 52, 39));
            Labels.Add(new PositionedText("Good Luck!", PlayedGame.Uncover, Console.WindowWidth - 52, 41));
        }
        
        public static bool Gameplay(out decimal score, out SpecialisedStopwatch playTime)
        {
            ///Shrnutí
            ///Hlavní metoda, která vrací bool: výhra/prohra, skóre a čas, jak dlouho hráč hru hrál
            for (int x = 0; x < Labels.Count; x++) //Vytisknou se všechny grafiky
                Labels[x].Print(x < 2, Reprint); //První dva obdélníky (velký obdélník okolo celé obrazovky a obdélník okolo herní plochy) se vytisknou se silnými svislými liniemi. Pokud při tištění není hra nastavena na celou obrazovku, počká se na opravu od uživatele, vymaže se Console a vytisknou se grafiky a hrací plocha
            CompletionTime.Start(); //Začne se počítat čas
            while (!EndGame) //Dokud hra není skončena volá se metoda GameTurn(), která vrací bool, podle toho zda se má hra ukončit (buď bylo otočeno políčko s minou nebo byla splněna podmínka pro výhru nebo se uživatel chce vrátit zpět do menu)
            {
                EndGame = GameTurn();
            }
            if ((UncoveredTiles.Number == (PlayedGame.HorizontalTiles * PlayedGame.VerticalTiles - PlayedGame.Mines))) //Nyní se zjišťuje proč byla hra skončena. Pokud se počet otočených políček rovná celkovému počtu políček bez min, tak byla hra vyhrána
                return GameWin(out score, out playTime); //zavolá se metoda GameWin, kde se spočítá skóre
            else if (GameAborted) //Pokud byla hra přerušena, protože se hráč chce vrátit zpět do menu, tak se skóre vrátí -1, podle čehož se indetifikuje že se máme vrátit až do prvního menu
            {
                score = -1;
                playTime = CompletionTime; //Hodnota je přiřazena jen proto, aby se vrátila out hodnota. Jinak to není v podstatě nutné
                return false;
            }
            else
                return GameLose(out score, out playTime); //V opačných případeh hráč prohral a zavolá se metoda GameLose()
        }
        public static bool GameWin(out decimal score, out SpecialisedStopwatch playTime)
        {
            ///Shrnutí
            ///Metoda, která se zavolá, když hráč zvítězí
            CompletionTime.Stop(); //Časomíra se ukončí
            score = Math.Round(1000 * ScoreMultiplier * PlayedGame.HorizontalTiles * PlayedGame.VerticalTiles * (int)Math.Pow(PlayedGame.Mines, 3) / CompletionTime.ElapsedMilliseconds, 5); //Výpočet skóre, zohledněn je počet políček, min, čas a samozřejmě ScoreMultiplier
            playTime = CompletionTime; //Vrátí zpátky true jakožto hlavní hodnotu, neboť hra je vyhrána, vypočítá se skóre a to se vrátí přes out
            return true;
        }
        public static bool GameLose(out decimal score, out SpecialisedStopwatch playTime)
        {
            ///Shrnutí
            ///Metoda, kterou se zavolá, když hráč prohraje
            CompletionTime.Stop(); //Časomíra se ukončí
            score = 0; //Skóre se tentokrát nepočítá, vzhledem k tomu, že hráč nezvítězil
            playTime = CompletionTime; //Každopádně se vrátí čas, jak dlouho se hráč snažil a vše se vrátí zpátky
            return false;
        }
        public static bool GameTurn()
        {
            ///Shrnutí
            ///Jeden herní tah, který vrací bool. Pokud vrátí true, tak to znamená, že se má hra ukončit. Jinak má hra pokračovat. True se vrátí pokud bylo otočeno políčko s minou nebo byla splněna podmínka pro výhru nebo se uživatel chce vrátit zpět do menu
            ///Toto přetížení nepřijímá vstupní hodnotu a zmáčknutou klávesu určí až sám uživatel
            if (((Console.LargestWindowWidth - 5) > Console.WindowWidth) || ((Console.LargestWindowHeight - 3) > Console.WindowHeight)) //Na začátku každého tahu se zajistí, že hra je na celou obrazovku
                Program.WaitForFix(); //Případně se počká, než to uživatel napraví
            if (UncoveredTiles.Number == (PlayedGame.HorizontalTiles * PlayedGame.VerticalTiles - PlayedGame.Mines))
                return true; //Nejprve se ověří zda není splněna podmínka pro výhru
            CurrentTile = new HighlightedTile(PlayedGame.Minefield[CurrentMinefieldPosition.Horizontal, CurrentMinefieldPosition.Vertical]); //Současné vybrané políčko se znovu vytvoří, aby získalo typ třídy HighlightedTile
            UncoveredTiles.Print(false, Reprint); //Aktualizuje se (znovu se vytiskne a tím pádem přepíše) číslo, udávající počet již otočených políček
            NumberOfFlags.Print(false, Reprint); //A také se aktualizuje číslo, udávající počet umístěných vlaječek
            ConsoleKey keypressed = Console.ReadKey(true).Key; //Nyní se počká, kterou klávesu zmáčkne uživatel
            return GameAction(keypressed); //A tato klávesa se následně oedšle do metody GameAction
        }
        public static bool GameTurn(ConsoleKey keypressed)
        {
            ///Shrnutí
            ///Jeden herní tah, který vrací bool. Pokud vrátí true, tak to znamená, že se má hra ukončit. Jinak má hra pokračovat. True se vrátí pokud bylo otočeno políčko s minou nebo byla splněna podmínka pro výhru nebo se uživatel chce vrátit zpět do menu
            ///Toto přetížení přijímá vstupní klávesu, je určeno pro používání Hintu, kdy o stisknuté kjlávese rozhoduje program sám
            if (((Console.LargestWindowWidth - 5) > Console.WindowWidth) || ((Console.LargestWindowHeight - 3) > Console.WindowHeight)) //Na začátku každého tahu se zajistí, že hra je na celou obrazovku
            {
                Program.WaitForFix(); //Případně se počká, než to uživatel napraví
                Reprint(); //A v případě potřeby se přetiskne herní plocha a ostatní grafiky
            }
            if (UncoveredTiles.Number == (PlayedGame.HorizontalTiles * PlayedGame.VerticalTiles - PlayedGame.Mines))
                return true; //Nejprve se ověří zda není splněna podmínka pro výhru
            CurrentTile = new HighlightedTile(PlayedGame.Minefield[CurrentMinefieldPosition.Horizontal, CurrentMinefieldPosition.Vertical]); //Současné vybrané políčko se znovu vytvoří, aby získalo typ třídy HighlightedTile
            UncoveredTiles.Print(false, Reprint); //Aktualizuje se (znovu se vytiskne a tím pádem přepíše) číslo, udávající počet již otočených políček
            NumberOfFlags.Print(false, Reprint); //A také se aktualizuje číslo, udávající počet umístěných vlaječek
            return GameAction(keypressed); //Klávesa, kterou zvolil program se odeše do metody GameAction
        }
        public static bool GameAction(ConsoleKey keypressed)
        {
            ///Shrnutí
            ///Jedna herní akce, která také vrací bool. Přijatá klávesa se přetaví v něco, co se uděje na herní ploše. Pokud se bude jednat o otočení políčka s minou nebo návrat zpět do menu, vrátí se true, jinak se vrátí false a hra může pokračovat
            if (CurrentTile.Covered) //Pokud CurrentTile před změnou na HighlightedTile byl Covered tile zůstal mu field bool Tile.Covered jako true, v takovém případě se nahradí nazpoět Covered tilem. Činí se tak proto, aby uživatel vždy viděl, které políčko má vybrané
                CurrentTile = new CoveredTile(CurrentTile);
            else //Pokud byl CurrentTile Uncovered změní se nazpět na Uncovered, opět ze stejného důvodu
                CurrentTile = new UncoveredTile(CurrentTile, true);
            if (keypressed == ConsoleKey.LeftArrow || keypressed == ConsoleKey.RightArrow || keypressed == ConsoleKey.UpArrow || keypressed == ConsoleKey.DownArrow) //Pokud bylo zmáčknuto tlačítko některé z šipek, tak nám tato skutečnost indikuje pohyb po hrací ploše
            {
                
                switch (keypressed) //Podle toho, která šipka byla stalčena se zavolá metoda na ChangableCoordinates
                {
                    case ConsoleKey.LeftArrow:
                        CurrentMinefieldPosition.GoLeft();
                        break;
                    case ConsoleKey.RightArrow:
                        CurrentMinefieldPosition.GoRight();
                        break;
                    case ConsoleKey.UpArrow:
                        CurrentMinefieldPosition.GoUp();
                        break;
                    case ConsoleKey.DownArrow:
                        CurrentMinefieldPosition.GoDown();
                        break;
                }
                CurrentTile = new HighlightedTile(PlayedGame.Minefield[CurrentMinefieldPosition.Horizontal, CurrentMinefieldPosition.Vertical]); //A na závěr se aktualizuje CurrentTile, který je nyní již na jiné pozici
            }
            else
            {
                switch (keypressed) //Pokus je stlačena jiná klávesa, tak se zavolá některá ze speciálních metod
                {
                    case ConsoleKey.H: //Stlačení H zavolá metodu Hint (nápověda), která učiní tah za hráče a vrátí bool podle toho, jestli byl hint nutný nebo nes 
                        if (Hint(false))
                        { }
                        else
                        {
                            ScoreMultiplier /= 2; //Pokud hint nebyl nutný vydělí se hráčův ScoreMultiplier dvěma
                        }
                        break;
                    case ConsoleKey.S: //Stlačení S zavolá metodu Solve (vyřešit), která opakuje hinty až dokud hru nedořeší nebo ji hráč nepřeruší
                        Solve(false);
                        break;
                    case ConsoleKey.Q: //Stlačení Q také zavolá metodu Solve, ale její zrychlenou verzi QuickSolve (rychle vyřešit). tato metoda narozdíl od klasického solvu nedělá přestávky mezi jedntlivými tahy, čili je mnohem rychlejší
                        Solve(true);
                        break;
                    case ConsoleKey.Escape: //Stlačení Escape pozastaví hru vyvoláním několika metod
                        CompletionTime.Stop(); //Nejprve se hráči pozastaví čas
                        bool unpause = PauseGame(); //Poté se hráči zobrazí menu pozastavené hry pomocí metody PauseGame(). Tato metoda vrací bool, podle toho, jestli hráč chce ve hře pokračovat
                        if (unpause) //Pokud chce pokračovat
                        {
                            PlayedGame.PrintMinefield(); //Vytiskne se znova hrací plocha
                            Labels[2] = (new Border(Console.WindowWidth - 54, 5, 4, 23 + (int)Math.Floor(Math.Log10((PlayedGame.HorizontalTiles * PlayedGame.VerticalTiles - PlayedGame.Mines))), ConsoleColor.Black, ConsoleColor.Gray, false)); //Je důležité si uvědomit, že PauseGame() může načíst jinou dříve uložnou hru, a proto je potřeba aktualizovat grafiky. Barvy a velikosti se musí přizpůsobit nově hrané hře
                            Labels[5] = new Border(6, 16, 10, 22, PlayedGame.Uncover, PlayedGame.Cover, true);
                            Labels[6] = new Border(6, 28, 10, 22, PlayedGame.UncoverSecondary, PlayedGame.CoverSecondary, true);
                            Labels[7] = new Border(31, 16, 10, 22, PlayedGame.Uncover, PlayedGame.Cover, true);
                            Labels[8] = new Border(31, 28, 10, 22, PlayedGame.UncoverSecondary, PlayedGame.CoverSecondary, true);
                            Labels[9] = new Border(Console.WindowWidth - 54, 16, 28, 50, PlayedGame.Uncover, PlayedGame.Cover, true);
                            for (int x = 0; x < Labels.Count; x++)
                            {
                                if ((x > 9 && x < 14) || (x > 16 && x < 21) || (x > 25))
                                    Labels[x].ChangeColour((int)PlayedGame.Uncover);
                                else if (x > 10)
                                    Labels[x].ChangeColour((int)PlayedGame.UncoverSecondary);
                                Labels[x].Print(x < 2, Reprint); //Na závěr všech změn barev se vše samozžejmě znovu vytiskne
                            }
                            CompletionTime.Start(); //A rozeběhne se čas
                        }
                        else
                        {
                            GameAborted = true; //Pokud hráč nechce pokračovat ve hře, nastaví se field GameAborted na true a vrátí se true, protože je hra ukončena
                            return true;
                        }
                        break;
                    case ConsoleKey.Spacebar: //Stlačení mezerníku znamená označení/odznačení vlaječkou
                        if (CurrentTile.Covered) //Celý tento blok se může odehrát pouze pokud není políčko již odkryté. Vlaječkou mohou být označena pouze políčka, která ještě nebyla odkryta
                        {
                            NumberOfFlags.ChangeBy(CurrentTile.FlagTile(), Reprint); //Zavolá se metoda FlagTile() na daném políčku, ta vrací buď 1 (políčko bylo nově označeno vlaječkou), nebo -1 (políčko bylo odznačeno) Počet umístěných vlajek je okamžitě o tuto hodnotu změněn a vytisknut. Pokud při tištění není hra nastavena na celou obrazovku, počká se na opravu od uživatele, vymaže se Console a vytisknou se grafiky a hrací plocha
                            if (!CurrentTile.Mine && CurrentTile.Flag) //Pokud na daném políčku není mina a políčko bylo označeno zvýší se počet chybně umístěných vlajek o jedna
                                IncorrectFlags += 1;
                            else if (!CurrentTile.Mine && !CurrentTile.Flag) //Pokud na daném políčku není mina a políčko bylo odznačeno sníží se počet chybně umístěných vlajek o jedna
                                IncorrectFlags -= 1;
                        }
                        PlayedGame.Minefield[CurrentMinefieldPosition.Horizontal, CurrentMinefieldPosition.Vertical] = CurrentTile; //Nakonec se zapíší změny na políčko v Arrayi, který field CurrentTile reprezentuje
                        break;
                    case ConsoleKey.Enter: //Stlačení Enteru znamená pokus o otočení políčka
                        if (CurrentTile.Covered && !CurrentTile.Flag) //Pokud políčko ještě není odkryté a není označeno vlaječkou. Jednou z funkcí vlaječky je, že zabraňuje neúmyslnému otočení políčka
                        {
                            if (UncoveredTiles.Number == 0) //Pokud zatím nebyla otočena žádná políčka (tedy pokud se jedná o první otočení), tak je zajištěno, aby bylo bezpečné
                            {
                                PlayedGame.MoveMinesOut(CurrentTile, CurrentTile.TilesAround); //Zavolá se metoda MoveMinesOut, která odstraní miny z vybraného políčka a jeho sousedních políček
                                CurrentTile = PlayedGame.Minefield[CurrentMinefieldPosition.Horizontal, CurrentMinefieldPosition.Vertical]; //Nakonec se zapíší změny na políčko v Arrayi, který field CurrentTile reprezentuje
                            }
                            if (CurrentTile.Mine) //Pokud je na políčku mina, vrací se true a hra končí prohrou
                                return true;
                            PlayedGame.Minefield[CurrentTile.MinefieldPosition.Horizontal, CurrentTile.MinefieldPosition.Vertical] = new UncoveredTile(CurrentTile, true); //V opačném případě se dané políčko změní na typ UncoveredTile
                            CurrentTile = PlayedGame.Minefield[CurrentTile.MinefieldPosition.Horizontal, CurrentTile.MinefieldPosition.Vertical];
                            if (CurrentTile.MinesAround == 0) //Pokud dané políčko má okolo sebe nula min, tak se automaticky otočí všechna okolní políčka
                            {
                                UncoverAllTilesAround(CurrentTile);
                                int uncoveredTiles = 0;
                                foreach (Tile tile in PlayedGame.Minefield)
                                    if (!tile.Covered)
                                        uncoveredTiles++;
                                UncoveredTiles.ChangeTo(uncoveredTiles, Reprint);
                            }
                            else
                                UncoveredTiles.ChangeBy(1, Reprint); //V ostatních případech se field UncoveredTiles změní o jedna a znovu vytiskne. Pokud při tištění není hra nastavena na celou obrazovku, počká se na opravu od uživatele, vymaže se Console a vytisknou se grafiky a hrací plocha
                        }
                        else if (!CurrentTile.Covered && CountFlagsAround(CurrentTile) == CurrentTile.MinesAround) //Pokud políčko je odkryté, může se zde uplatnit automatické otočení okolních políček. To může proběhnout pokud se shoduje počet min okolo s počtem vlajek okolo
                        {
                            if (UncoverTilesAroundWithoutFlags(CurrentTile))
                                return true;
                            int uncoveredTiles = 0;
                            foreach (Tile tile in PlayedGame.Minefield)
                                if (!tile.Covered)
                                    uncoveredTiles++;
                            UncoveredTiles.ChangeTo(uncoveredTiles, Reprint);
                        }
                        PlayedGame.Minefield[CurrentMinefieldPosition.Horizontal, CurrentMinefieldPosition.Vertical] = CurrentTile; //Nakonec se zapíší změny na políčko v Arrayi, který field CurrentTile reprezentuje
                        break;
                    case ConsoleKey.R: //Pokud uživatel zmáčkne R zavolá se metoda Reprint
                        try
                        {
                            Reprint(); //Pokusí se vymazat vše a vytisknout herní ovládání znovu
                        }
                        catch (ArgumentOutOfRangeException) //Pokud nastane situace, že něco by se mělo tisknout mimo obrazovku
                        {
                            Program.WaitForFix(); //Vyzve uživatele, aby zvětšil hru na celou obrazovku a nepustí ho dál dokud tak neučiní
                            Reprint(); //Znovu se pokusí vymazat vše a vytisknout herní ovládání znovu
                        }
                        break;
                }
            }
            return false;
        }
        
        private static void UncoverAllTilesAround(Tile UncoverAround)
        {
            ///Shrnutí
            ///Tato metoda otočí všechna políčka okolo daného políčka. Pokud některé z těchto políček okolo sebe také nemá žádnou minu, zavolá se rekurzivně tato metoda znovu.
            for (int x = 0; x < UncoverAround.TilesAround.Count; x++) //Projede se celý seznam UncoverAround.TilesAround (kde jsou všechna sousední políčka)
            {
                int currentHorizontal = UncoverAround.TilesAround[x].MinefieldPosition.Horizontal; 
                int currentVertical = UncoverAround.TilesAround[x].MinefieldPosition.Vertical; //Z políčka se vybere jeho pozice v hrací ploše
                if (PlayedGame.Minefield[currentHorizontal, currentVertical].Covered) //Nyní se zkontroluje zda políčka na této pozici v hrací ploše je otočené (Je to uděláno takto složitě protože políčko v seznamu nemusí být aktualizované)
                {
                    if (PlayedGame.Minefield[currentHorizontal, currentVertical].Flag) //Pokud má náhodou toto políčko vlaječku, tak ta bude automaticky odstraněna v konstruktoru UncoveredTileu (který automaticky nastaví Flag na false
                    {
                        NumberOfFlags.ChangeBy(-1, Reprint); //Musí se ale zajistit, že se změní počet umístěných vlaječek o jedna
                        IncorrectFlags--; //Zároveň je jasné, že se jednalo o špatně umístěnou vlajku, když byla umístěna na políčku, kde není mina. Z toho důvodu se sníží i počet chybně umístěných vlajek o jedna
                    }
                    PlayedGame.Minefield[currentHorizontal, currentVertical] =  new UncoveredTile(PlayedGame.Minefield[currentHorizontal, currentVertical], true); //Zde dochází pomocí konstruktoru k samotnému otočení
                    if (PlayedGame.Minefield[currentHorizontal, currentVertical].MinesAround == 0) //Pokud toto políčko okolo sebe nemá žádnou minu znovu se zavolá tato metoda
                    {
                        UncoverAllTilesAround(PlayedGame.Minefield[currentHorizontal, currentVertical]); //Tentokrát se jako Tile UncoverAround vloží políčko se současnými souřadnicemi Počet otočených políček se zvýší o počet políček, který se otočí okolo nového políčka
                    }
                }
            }
        }
        private static bool UncoverTilesAroundWithoutFlags(Tile UncoverAround)
        {
            ///Shrnutí
            ///Tato metoda otočí všechna políčka okolo daného políčka, které nemají vlajku. Pokud některé z těchto políček má okolo sebe nula min, zavolá se na něm také metoda UncoverAllTilesAround
            ///Tato metoda vrací bool jestli byla odkryta mina. To se může stát, pokud byly vlajky špatně umístěny
            for (int x = 0; x < UncoverAround.TilesAround.Count; x++) //Projede se celý seznam UncoverAround.TilesAround (kde jsou všechna sousední políčka), stejně jako u metody UncoverAllTilesAround
            {
                int currentHorizontal = UncoverAround.TilesAround[x].MinefieldPosition.Horizontal;
                int currentVertical = UncoverAround.TilesAround[x].MinefieldPosition.Vertical;
                if (PlayedGame.Minefield[currentHorizontal, currentVertical].Covered && !PlayedGame.Minefield[currentHorizontal, currentVertical].Flag) //Nyní, kromě toho, jestli jsou políčka neotočená ověřujeme také jestli nemají vlajku
                {
                    if (PlayedGame.Minefield[currentHorizontal, currentVertical].Mine) //Pokud má některé z těchto políček minu a my bychom ji vlastně chtěli otočit, vrátí se bool, že byla odkryta mina a ukončí se hra prohrou
                    {
                        return true;
                    }
                    else //Jinak se políčko normálně otočí pomocí konstruktoru
                    {
                        PlayedGame.Minefield[currentHorizontal, currentVertical] = new UncoveredTile(PlayedGame.Minefield[currentHorizontal, currentVertical], true); //Zde dochází pomocí konstruktoru k samotnému otočení
                        if (PlayedGame.Minefield[currentHorizontal, currentVertical].MinesAround == 0) //Pokud některé z těchto políček nemá žádnou minu, zavolá se mmetoda UncoverAllTilesAround, aby otočila políčka okolo něj. Int, který vrací tato metoda se přičte k tilesUncovered, který budeme brzy vracet
                        {
                            UncoverAllTilesAround(PlayedGame.Minefield[currentHorizontal, currentVertical]);
                        }
                    }
                }
            }
            return false;
        }
        public static int CountCoveredAround(Tile tile)
        {
            ///Shrnutí
            ///Metoda, kterou využívá metoda Hint(). Tato metoda spočítá pro vložené políčko počet okolních otočených políček
            tile.TilesAroundCalculator(); //Nově se přepočítají políčka okolo
            int coveredAround = 0; //V této proměnné se počítá počet otočených políček
            foreach (Tile tileAround in tile.TilesAround) //Projede se celý seznam políček okolo tohoto
            {
                if (tileAround.Covered)
                    coveredAround++; //Pokud je políčko Covered proměnná se zvýší
            }
            return coveredAround; //Když dojede foreach vrátí se naše proměnná
        }
        public static int CountFlagsAround(Tile tile)
        {
            ///Shrnutí
            ///Metoda, kterou využívá metoda Hint(). Tato metoda spočítá pro vložené políčko počet okolních políček s vlaječkou
            tile.TilesAroundCalculator(); //Nově se přepočítají políčka okolo
            int flagsAround = 0; //V této proměnné se počítá počet políček s vlaječkou
            foreach (Tile tileAround in tile.TilesAround) //Projede se celý seznam políček okolo tohoto
            {
                if (tileAround.Flag)
                    flagsAround++; //Pokud je políčko Flag proměnná se zvýší
            }
            if (flagsAround == 0) //Pokud není okolo ani jedna vlaječka, tak se vrátí minus jedna, jinak by totiž bylo započítáno každé políčko, které má okolo sebe nula min, že se jeho počet min okolo rovná počtu vlaječek okolo
                return -1;
            else
                return flagsAround; //Když dojede foreach a nalezne se alespoň jedna vlaječka, tak se vrátí naše proměnná
        }
        public static void NavigateToTile(Tile tile, bool quick)
        {
            ///Shrnutí
            ///Další metoda, kterou využívá metoda Hint(). Tato metoda vloží do GameTurn(ConsoleKey) správné množství a směry šipek, aby se program dostal na žádaný Tile tile.
            ///Další vstupní hodnota je bool quick, která ovlivňuje, zda po každém tahu bude krátká přestávka, aby mohl hráč sledovat, co se děje
            if (tile.MinefieldPosition.Horizontal > CurrentMinefieldPosition.Horizontal) //         Pokud je současná pozice více vlevo než cílová pozice
            {
                while (tile.MinefieldPosition.Horizontal != CurrentMinefieldPosition.Horizontal) // Tak dokud se současná pozice nerovná cílové pozici
                {
                    EndGame = GameTurn(ConsoleKey.RightArrow); //                                   Tak se vkládá do metody GameTurn() šipka doprava
                    if (!quick)
                        System.Threading.Thread.Sleep(50); //Pokud se nejedná o rychlou navigaci, tak se 5 setin vteřiny nic neděje
                }
            }
            else
            {
                while (tile.MinefieldPosition.Horizontal != CurrentMinefieldPosition.Horizontal)
                {
                    EndGame = GameTurn(ConsoleKey.LeftArrow);
                    if (!quick)
                        System.Threading.Thread.Sleep(50);
                }
            }
            if (tile.MinefieldPosition.Vertical > CurrentMinefieldPosition.Vertical)
            {
                while (tile.MinefieldPosition.Vertical != CurrentMinefieldPosition.Vertical)
                {
                    EndGame = GameTurn(ConsoleKey.DownArrow);
                    if (!quick)
                        System.Threading.Thread.Sleep(50);
                }
            }
            else
            {
                while (tile.MinefieldPosition.Vertical != CurrentMinefieldPosition.Vertical)
                {
                    EndGame = GameTurn(ConsoleKey.UpArrow);
                    if (!quick)
                        System.Threading.Thread.Sleep(50);
                }
            }

            return; //To samé funguje i pro všehny ostatní směry
        }
        
        public static bool Hint(bool quick)
        {
            ///Shrnutí
            ///Tato metoda poskytne hráči nápovědu. Může se jednat o odstranění chybné vlaječky, označení miny vlaječkou nebo otočení políčka
            ///Metoda vrací bool podle toho zda byl hint nutný nebo ne
            ///Hint je brán jako nutný v momentě kdy pro žádné políčko neplatí,
                ///že počet vlaječek okolo něj se rovná počtu min okolo něj, a proto lze otočit zbylá políčka okolo něj
                ///že počet neotočených políček okolo něj se rovná počtu min okolo něj, a proto lze označit neotečená políčka vlaječkou
                ///že má na sobě vlaječku, ačkoliv se na něm nenachází mina
            ///Také není hint brán jako nutný, když není otočeno žádné políčko, protože první otočení je na 100 % bezpečné
            if (UncoveredTiles.Number == (PlayedGame.HorizontalTiles * PlayedGame.VerticalTiles) - PlayedGame.Mines) //Nejprve se zkotroluje, zda není splněna podmínka pro výhru  v takovém případě se vrátí true a ukončí se hra výhrou
            {
                EndGame = true;
                return true;
            }
            if (UncoveredTiles.Number == 0) //Nyní se zkontroluje, zda je alespoň jedno políčko otočeno, pokud ne
            {
                foreach (Tile tile in PlayedGame.Minefield) //Zkontroluje se jestli se nenachází na hrcí ploše nějaké vlaječky
                    if (tile.Flag)                          //Pokud ano, tak se odstraní
                    {
                        NavigateToTile(tile, quick);        //Naviguje se na dané políčko
                        EndGame = GameTurn(ConsoleKey.Spacebar); //A dvakrát se stiskne mezerník, čímž se vlaječka odstané
                        EndGame = GameTurn(ConsoleKey.Spacebar);
                    }
                EndGame = GameTurn(ConsoleKey.Enter); //Pak se použije Enter a provede se bezpečné první otočení
                return false; //Hint v tomto případě nebyl nutný
            }
            if (IncorrectFlags != 0) //Dále se zkontroluje zda se na hrací ploše nenacházejí chybně umístěné vlaječky
            {
                foreach (Tile tile in PlayedGame.Minefield) //Pokud ano, projede se celý Array
                {
                    if (tile.Flag && !tile.Mine)            //Najde se políčko, která má vlaječku a zároveň nemá minu 
                    {
                        NavigateToTile(tile, quick);        //Naviguje se na dané políčko
                        EndGame = GameTurn(ConsoleKey.Spacebar);    
                        EndGame = GameTurn(ConsoleKey.Spacebar);    //A poté se dvakrát stiskne mezerník, kterým se vlaječka odstraní
                        return false; //Hint v tomto případě nebyl nutný
                    }
                }
            }
            
            
            foreach (Tile tile in PlayedGame.Minefield) //Nyní se projede celá herní plocha a program se snaží najít políčko, které by umožnilo najít další tah
            {
                if (tile.Covered) //Zakrytá políčka neposkytují žádné informace, takže ty můžeme přeskočit
                    continue;
                int flagsAround = CountFlagsAround(tile); //U každého políčka nás zajímá kolik je okolo něj vlajek
                int coveredAround = CountCoveredAround(tile); //A kolik je okolo něj otočených políček
                if (coveredAround == tile.MinesAround) //Pokud se počet zakrytých políček okolo rovná počtu min všechna tato políčka budou označena vlaječkou
                {
                    for (int x = 0; x < tile.TilesAround.Count; x++) //Projedou se všechna políčka, co sousedí s tímto políčkem
                    {
                        Tile potentialFlagTile = tile.TilesAround[x];
                        if (!potentialFlagTile.Flag && potentialFlagTile.Covered)   //Pokud je políčko zakryté a nemá vlaječku
                        {
                            NavigateToTile(potentialFlagTile, quick);               //Naviguje se na dané políčko
                            if (potentialFlagTile.Questionmark)
                                EndGame = GameTurn(ConsoleKey.Spacebar);            //Pokud je na něm otazník, tak se odstraní otazník
                            EndGame = GameTurn(ConsoleKey.Spacebar);                //A poté se stiskne mezerník, čímž se umístí vlaječka

                            return false; //Hint v tomto případě nebyl nutný
                        }
                    }
                }
                if (flagsAround == tile.MinesAround && coveredAround != flagsAround) //Pokud se počet vlaječek okolo políčka rovná počtu min okolo políčka
                {
                    NavigateToTile(tile, quick); //Přesuneme se na políčko
                    EndGame = GameTurn(ConsoleKey.Enter); //Stiskneme enter, čímž vyvoláme metodu UncoverTilesAroundWithoutFlags
                    return false; //Hint v tomto případě nebyl nutný
                }
            }
            for (int i = 7; i != 0; i--)
            {
                foreach (Tile tile in PlayedGame.Minefield) //Pokud nejsme schopni určit další tah tímto způsobem, přejde se na druhý způsob, který už využívá informací, které nejsou hráči známé, ale programu samozřejmě ano. Hint v tomto případě je brán jako nutný (ačkoliv se někdy dá i v takovémto případě dá další tah určit, ovšem ne vždy)
                {
                    if (!tile.Covered)
                        continue;
                    int coveredAround = CountCoveredAround(tile);
                    int flagsAround = CountFlagsAround(tile);
                    if (tile.Mine && !tile.Flag && (flagsAround + tile.TilesAround.Count - coveredAround == i)) //Nalezne se takové políčko, které má na sobě minu a nemá vlajku a zároveň má okolo sebe co nejvíc odkrytých políček nebo políček s vlajkami (aby toto políčko mohlo poskytnout hráči co nejvíc informací)
                    {
                        NavigateToTile(tile, quick); //Naviguje se na dané políčko
                        if (tile.Questionmark)
                            EndGame = GameTurn(ConsoleKey.Spacebar); //Pokud má políčko otazník, tak se odstraní otazníkregionalOR_l
                        EndGame = GameTurn(ConsoleKey.Spacebar); //A stiskne se mezerník
                        return true; //Hint v tomto případě byl nutný
                    }
                    if (!tile.Mine && (flagsAround + tile.TilesAround.Count - coveredAround == i)) //Podobný způsob, tentokrát hledáme políčko bez miny a místo označování vlajkou jej budeme otáčet
                    {
                        NavigateToTile(tile, quick); //Naviguje se na dané políčko
                        EndGame = GameTurn(ConsoleKey.Enter); //A stiskne se Enter
                        return true; //Hint v tomto případě byl nutný
                    }
                }
            }
            return true;
        }
        public static void Solve(bool quick)
        {
            ///Shrnutí
            ///Tato metoda opakuje metodu Hint() dokud hra není dohrána nebo není stisknut Escape
            ///Je možné vložit bool quick, který řešení urychlý, neboť do všech hintů bude vložen právě tento boolean
            do //Dokud uživatel nestiskne Escape probíhá tento do while cyklus
            {
                while (!Console.KeyAvailable && !EndGame) //Dokud uživatel nestiskne klávesu nebo není hra ukončena probíhá while cyklus
                {
                    if (!Hint(quick)) //While cyklus volá metodu Hint(), pokud vrátí false sníží se ScoreMultiplier
                        ScoreMultiplier /= 2;
                }
                if (EndGame) //Pokud se while cyklus přeruší a není to kvůli tomu, že byla stisknuta klávesa končí i do while cyklus
                    break;
            } while (Console.ReadKey(true).Key != ConsoleKey.Escape); //Pokud je stisknutá klávesa Escape tak končí do while cyklus, jinak se vrací zase zpátky do while cyklu
            return;
        }

        public static void Reprint()
        {
            ///Shrnutí
            ///Metoda, která smaže a znovu vytiskne herní plochu a grafiky. Používá se v případě, že uživatel zmenšil nedopatřením okno, aby se mu povedlo srovnat vytištěné objekty na svá místa.
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear(); //Smaže se vše, co je v dané chvíli vytištěno
            PlayedGame.PrintMinefield(); //Znovu se vytiskne hrací plocha
            for (int x = 0; x < Labels.Count; x++) //Znovu se vytisknou grafické objekty
                Labels[x].Print(x < 2, Reprint);
        }
        public static bool PauseGame()
        {
            ///Shrnutí
            ///Metoda, která pozastaví hru a zobrazí menu pozastavené hry
            ///Vrací bool v závislosti na tom, zda chce uživatel pokračovat ve hře
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear(); //Vymaže vytisklou hru a grafiky
            return PauseMenu.PauseGameMenu(); //Zavolá se hlavní metoda statické třídy PauseMenu, která vrátí žádaný boolean
        }
        public static void SetLoaded(string[] SavedGame)
        {
            ///Shrnutí
            ///Metoda, která do fieldů GameControls vloží načtené hodnoty
            string[] parameters = SavedGame[4].Split(','); //Parametry jsou umístěny v pátém řádku uloženého souboru, ten je následně rozděleny podle čárek na jednotlivé parametry
            string[] time = SavedGame[5].Split(';'); //Uběhlý čas je umístěn v šestém řádku uloženého souboru, dvě čísla jsou rozdělena středníkem
            CurrentMinefieldPosition = new ChangeableCoordinates(0, 0, PlayedGame.HorizontalTiles - 1, PlayedGame.VerticalTiles - 1); //CurrentMinefieldPosition se restartuje a především se nově spočtou jeho maximální hodnoty
            CurrentTile = PlayedGame.Minefield[0, 0]; //CurrentTile se restartuje
            UncoveredTiles.ChangeTo(Int32.Parse(parameters[10]), Reprint, false); //Z parametrů se načte počet odkrytých políček a ten se také změní a přetiskne. Pokud při tištění není hra nastavena na celou obrazovku, počká se na opravu od uživatele, vymaže se Console a vytiskne se toto menu znovu od začátku
            NumberOfFlags.ChangeTo(Int32.Parse(parameters[11]), Reprint, false); //To stejné čeká i počet vlaječek
            IncorrectFlags = Int32.Parse(parameters[12]); //Zároveň se načtou i uložené fieldy IncorrectFlags
            ScoreMultiplier = Decimal.Parse(parameters[13]); //A ScoreMultiplier
            CompletionTime = new SpecialisedStopwatch(time[0], time[1]); //Z času se nahraje uložený Completion Time (toto je halvní důvod existence třídy SpecialisedStopwatch, neboť bylo potřeba mít možnost přičítat uložený čas)
            Labels[1] = new Border(new Coordinates(PlayedGame.Minefield[0, 0].Position, -2, -1), PlayedGame.VerticalTiles + 2, 2 * (PlayedGame.HorizontalTiles + 2), ConsoleColor.Black, ConsoleColor.White, false); //na závěr se změní velikost rámečku ohraničujícího hrací plochu, která se mohla zvětšit/zmenšit
        }
    }
}  