using System;

namespace GloriousMinesweeper
{
    class GameMenu
    {
        ///Shrnutí
        ///Herní menu, které má nastavení hry
        ///Nelze v nich upravovat nastavení počtu políček a min
        ///Ty lze upravit pouze u objektu třídy CustomGameMenu, která dědí z této třídy
        public PositionedText Name { get; } //Název menu (Easy, Medium, Hard a Custom)
        public GameSetting[] GameSettings { get; private set; } //Array GameSettingů, kde jsou dány parametry hry
        public int ChosenLine { get; protected set; } //Vybraný GameSetting
        public Difficulties Difficulty { get; } //Obtížnost z Enumu obtížností (Easy = 1; Medium = 2; Hard = 3)

        public GameMenu(string name, Difficulties difficulty) //Konstruktor menu přijme název a obtížnost, z obtížnosti se vypočítají počty políček a min
        {
            if (((Console.LargestWindowWidth - 5) > Console.WindowWidth) || ((Console.LargestWindowHeight - 3) > Console.WindowHeight)) //Pokud není hra na celou obrazovku počká se dokud to uživatel nenapraví, jinak by se mohly grafiky špatně vytisknout
            {
                Program.WaitForFix();
            }
            Difficulty = difficulty;
            Name = new PositionedText(name, ConsoleColor.Black, (Console.WindowWidth - name.Length) / 2, 3) ; //Jméno se převede ze Stringu do PositionedTextu
            int mines = 5 * (int)Math.Pow((int)difficulty, 2) * ((int)difficulty + 1); //Počet min se vypočítá z obtížnosti (obtížnost^3 + obtížnost^2)
            GameSettings = new GameSetting[3]; //Vytvoří se Array GameSettingů
            GameSettings[0] = new GameSetting("Number of horizontal tiles", (int)difficulty * 10, false, false, 10); //Počet políček v obou směrech se rovná
            GameSettings[1] = new GameSetting("Number of vertical tiles", (int)difficulty * 10, false, false, 12);  //10*obtížnost
            GameSettings[2] = new GameSetting("Number of mines", mines, false, false, 14); //Miny byly již vypočítány, vše ale musí dostat svoji grafickou podobu přes GameSetting
            ChosenLine = 0; //Výchozím vybraným řádkem je první (nultý) řádek
        }

        public void PrintMenu(bool higlightName)
        {
            ///Shrnutí
            ///Vytiskne znovu menu, přijímá bool, podle kterého určuje zda má být jméno zvýrazněno
            DiffSwitcher.PrintMenuName(higlightName); //Nejprve se vytiskne jméno
            DiffSwitcher.PrintGraphics(false); //Následně grafiky bez obdélníků (tedy hlavně druhý a třetí řádek menu)
            foreach (GameSetting setting in GameSettings)
                setting.Print(false, Reprint); //Následně se vytisknou všechny GameSettingy z arraye v menu
            foreach (GameSetting colourSetting in DiffSwitcher.Colours)
                colourSetting.Print(false, Reprint); //A všechny barvy z arraye společného pro všechna menu
        }
        public void ChooseLine(int line)
        {
            ///Shrnutí
            ///Jednoduchá metoda, která nastaví zvolený řádek na dané číslo
            ChosenLine = line;
        }
        public virtual int MenuAction()
        {
            ///Shrnutí
            ///Metoda pomocí které se dají upravovat nastavení v menu
            ///Dají se upravit barvy, při pokusu o úpravu počtu políček nebo min se vytvoří nový object CustomGameMenu a přepne se na něj
            ///Metoda vrací číselné kódy podle toho co se stalo
            ///1: uživatel vyjel ven nahoru na měnění obtížností (na měnění jednotlvých menu)
            ///0: uživatel potvrdil nastavení Enterem
            ///-1: uživatel vytvořil CustomGameMenu
            ///-2: uživatel zrušil tvorbu hry stisknutím Escape
            ConsoleKey keypressed; //Budeme číst od uživatele stisklou klávesu
            do //Začíná do while cyklus, který trvá, dokud se nevrátí jedna z hodnot zmíněných výše
            {
                if (ChosenLine <= 2) //Pokud se nacházíme v Arrayi nastavení této hry tak zvýrazníme
                    GameSettings[ChosenLine].Print(true, Reprint); //Nastavení s vybraným indexem
                else
                    DiffSwitcher.Colours[ChosenLine - 3].Print(true, Reprint); //Pokud se dostaneme do nastavení barev, tak se vytiskne barva s daným indexem, ale posuneme se o tři
                keypressed = Console.ReadKey(true).Key; //Nyní již čteme klávesu od uživatele
                switch (keypressed)
                {
                    case ConsoleKey.UpArrow: //Pokud je zmáčknuta šipka nahoru
                        if (ChosenLine == 0) //Pokud jsme na vrchním řádku
                        {
                            GameSettings[ChosenLine].Print(false, Reprint); //Přetiskne se zvolené nastavení (v tomto případě vrchní řádek), tentokrát již bez vyznačení
                            return 1; //Vrací se kód 1 -> uživatel vyjel ven nahoru na měnění obtížností (na měnění jednotlvých menu)
                        }
                        else //Pokud ne tak se posune řídek o jedna nahoru
                        {
                            if (ChosenLine <= 2) //Vytiskneme předchozí řádek znovu, tentokrát bez vyznačení (opět musíme zjistit jestli se nacházíme v Colours nebo v GameMenu.GameSettings
                                GameSettings[ChosenLine].Print(false, Reprint);
                            else
                                DiffSwitcher.Colours[ChosenLine - 3].Print(false, Reprint);
                            ChosenLine--; //Nakonec se sníží číslo ChosenLine o jedna
                            break;
                        }
                    case ConsoleKey.DownArrow: //Pokud je zmáčknuta šipka dolů
                        if (ChosenLine != 9) //Tak se posuneme dolů pokud již nejsme na spodní řádce
                        {
                            if (ChosenLine <= 2) //Nejprve zvšak vytiskneme stávající řádek bez zvýraznění, s tím že opět zjistíme jestli jsme v Colours nebo ne
                                GameSettings[ChosenLine].Print(false, Reprint);
                            else
                                DiffSwitcher.Colours[ChosenLine - 3].Print(false, Reprint);
                            ChosenLine++; //A zvýšíme číslo
                            break;
                        }
                        break;
                    case ConsoleKey.LeftArrow: //Pokud je zmáčknuta šipka doleva
                        if (ChosenLine > 2) //Tak pokud jsme v Colours tak se změní barva
                        {
                            Program.TakenColours.Remove((ConsoleColor)DiffSwitcher.Colours[ChosenLine - 3].SettingValue.Number); //Nejprve odstraníme ze seznamu Program.TakenColours stávající barvu
                            DiffSwitcher.Colours[ChosenLine - 3].ChangeValue(-1, ChosenLine, GameSettings[0].SettingValue.Number * GameSettings[0].SettingValue.Number, GameSettings[2].SettingValue.Number, Reprint); //Nyní zavoláme metodu která změní barvu s původním pohybom o jedno doleva
                            Program.TakenColours.Add((ConsoleColor)DiffSwitcher.Colours[ChosenLine - 3].SettingValue.Number); //A nakonec přidáme tuto barvu do sezname Program.TakenColours
                        }
                        else
                        {
                            DiffSwitcher.GameMenus[3] = new CustomGameMenu(keypressed, ChosenLine); //Pokud se však nacházíme na nastavení min a políček, tak se vytvoří nové menu typu CustomGameMenu
                            return -1; //A vrátí se -1 -> uživatel vytvořil CustomGameMenu
                        }
                        break;
                    case ConsoleKey.RightArrow: //Pokud je zmáčknuta šipka doprava, tak se provede úplně to samé, pouze směrem doprava (s hodnotou +1)
                        if (ChosenLine > 2)
                        {
                            Program.TakenColours.Remove((ConsoleColor)DiffSwitcher.Colours[ChosenLine - 3].SettingValue.Number);
                            DiffSwitcher.Colours[ChosenLine - 3].ChangeValue(1, ChosenLine, GameSettings[0].SettingValue.Number * GameSettings[0].SettingValue.Number, GameSettings[2].SettingValue.Number, Reprint);
                            Program.TakenColours.Add((ConsoleColor)DiffSwitcher.Colours[ChosenLine - 3].SettingValue.Number);
                        }
                        else
                        {
                            DiffSwitcher.GameMenus[3] = new CustomGameMenu(keypressed, ChosenLine);
                            return -1;
                        }
                        break;
                    case ConsoleKey.Enter: //Pokud uživatel potvrdí nastavení Enterem, tak se vrátí kód 0 -> začít hru s těmito nastavenímis
                        return 0;
                    case ConsoleKey.Escape: //Pokud zmáčkne uživatel Escape, tak se vrátí kód -2 -> vrátíme se zpátky do menu
                        return -2;
                    case ConsoleKey.R: //Pokud uživatel zmáčkne R zavolá se metoda Reprint
                        try //Pokusí se smazat menu a vytisknout jej znovu
                        {
                            Reprint();
                        }
                        catch (ArgumentOutOfRangeException) //Pokud nastane situace, že něco by se mělo tisknout mimo obrazovku
                        {
                            Program.WaitForFix(); //Vyzve uživatele, aby zvětšil hru na celou obrazovku a nepustí ho dál dokud tak neučiní
                            Reprint(); //Znovu se pokusí vymazat menu a vytisknout jej znovu
                        }
                        break;
                }
            } while (true);
        }
        public void Reprint()
        {
            ///Shrnutí
            ///Metoda, která smaže a znovu vytiskne menu. Používá se v případě, že uživatel zmenšil nedopatřením okno, aby se mu povedlo srovnat vytištěné objekty na svá místa.
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear(); //Vymaže Consoli
            DiffSwitcher.PrintGraphics(true); //Vytiskne znovu grafiky
            PrintMenu(false); //Víme že se nacházíme někde mezi nastaveními, proto není jméno zvýrazněno
        }
    }
    public enum Difficulties
    {
        ///Shrnutí
        ///Výčet obtížností, ze kterých se počítá počet min a počet políček ve výchoím nastavení menu
        Easy = 1,
        Medium = 2,
        Hard = 3
    }
}
       
