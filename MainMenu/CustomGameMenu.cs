using System;

namespace GloriousMinesweeper
{
    class CustomGameMenu : GameMenu
    {
        ///Shrnutí
        ///Herní menu, které má nastavení hry
        ///Lze v něm upravovat všechna nastavení
        ///Dědí ze třídy GameMenu, ve které jde upravovat pouze nastavení barev
        public CustomGameMenu(ConsoleKey keypressed, int chosenLine) : base("Custom", DiffSwitcher.GameMenus[DiffSwitcher.ChosenMenu].Difficulty) //První konstruktor, který vyjde z GameMenu v momentě, kdy se hráč pokusí upravit počet políček nebo min v GameMenu.
        {
            ChosenLine = chosenLine; //Je naprosto identické s menu, ze kterého vzešlo
            if (keypressed == ConsoleKey.LeftArrow) //Pouze se změní hodnota u daného řádku podle toho, jaká klávesnice byla stisknuta
                GameSettings[chosenLine].ChangeValue(-1, ChosenLine, GameSettings[0].SettingValue.Number * GameSettings[1].SettingValue.Number, GameSettings[2].SettingValue.Number, Reprint);
            else if (keypressed == ConsoleKey.RightArrow)
                GameSettings[chosenLine].ChangeValue(1, ChosenLine, GameSettings[0].SettingValue.Number * GameSettings[1].SettingValue.Number, GameSettings[2].SettingValue.Number, Reprint);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear(); //Vymaže se menu předchozí
            PrintMenu(false); //A vytiskne se toto menu
        }
        public CustomGameMenu(int[] Parameters) : base("Custom", Difficulties.Easy)
        {
            //Druhý konstruktor, který dostane přímo zadané parametry a nastaví toto menu v souladu s nimi. Používá se při načítání hry ze souboru
            for (int x = 0; x < 3; x++)
                GameSettings[x].SettingValue.ChangeTo(Parameters[x], Reprint); //Nastavení min a políček se nastaví na dané hodnoty
        }

        public override int MenuAction()
        {
            ///Shrnutí
            ///Metoda CustomGameMenu.MenuAction() je dosti podobná jako GameMenu.MenuAction()
            ///Pouze s tím rozdílem, že zde jdou upravovat i počty min a políček a už se přes ní nevytváří nové CustomGameMenu
            ///Vrací stejné hodnoty jako GameMenu.MenuAction(), ale již nemůže vrátit -1 z důvodu zmíněného na předchozím řádku
            ///Vrací pouze:
            ///1: uživatel vyjel ven nahoru na měnění obtížností (na měnění jednotlvých menu)
            ///0: uživatel potvrdil nastavení Enterem
            ///-2: uživatel zrušil tvorbu hry stisknutím Escape
            ConsoleKey keypressed; //Opět budeme číst klávesu od uživatele
            do //Začíná while cyklus, který trvá až dokud se nevrátí jedna z výše zmiňovaných hodnot
            {
                if (ChosenLine <= 2) //Zvýrazní se řádek, podobně jako v GameMenu se nejprve musí zjistit jesli se pohybujeme mezi Colours nebo mezi nastaveními 
                    GameSettings[ChosenLine].Print(true, Reprint);
                else
                    DiffSwitcher.Colours[ChosenLine - 3].Print(true, Reprint);
                keypressed = Console.ReadKey(true).Key; //Zde již čteme klávesu od uživatele
                switch (keypressed)
                {
                    case ConsoleKey.UpArrow: //Pro šipky nahoru a dolů se děje to samé jako u GameMenu.MenuAction()
                        if (ChosenLine == 0)
                        {
                            GameSettings[ChosenLine].Print(false, Reprint);
                            return 1;
                        }
                        else
                        {
                            if (ChosenLine <= 2)
                                GameSettings[ChosenLine].Print(false, Reprint);
                            else
                                DiffSwitcher.Colours[ChosenLine - 3].Print(false, Reprint);
                            ChosenLine--; 
                            break;
                        }
                    case ConsoleKey.DownArrow:
                        if (ChosenLine != 9)
                        {
                            if (ChosenLine <= 2)
                                GameSettings[ChosenLine].Print(false, Reprint);
                            else
                                DiffSwitcher.Colours[ChosenLine - 3].Print(false, Reprint);
                            ChosenLine++;
                            break;
                        }
                        break;
                    case ConsoleKey.LeftArrow: //Změna nastane až pro šipky doleva a doprava
                        if (ChosenLine > 2) //Pro barvy přichází ta stejná prcoedura
                        {
                            Program.TakenColours.Remove((ConsoleColor)DiffSwitcher.Colours[ChosenLine - 3].SettingValue.Number); //Odstranění ze seznamu
                            DiffSwitcher.Colours[ChosenLine - 3].ChangeValue(-1, ChosenLine, GameSettings[0].SettingValue.Number * GameSettings[1].SettingValue.Number, GameSettings[2].SettingValue.Number, Reprint); //Změna
                            Program.TakenColours.Add((ConsoleColor)DiffSwitcher.Colours[ChosenLine - 3].SettingValue.Number); //Vrácení do seznamu
                        }
                        else //Tentokrát však pro ostatní nastavení už funguje normální ChangeValue, pro levou šipku s -1
                        {
                            GameSettings[ChosenLine].ChangeValue(-1, ChosenLine, GameSettings[0].SettingValue.Number * GameSettings[1].SettingValue.Number, GameSettings[2].SettingValue.Number, Reprint);
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        if (ChosenLine > 2)
                        {
                            Program.TakenColours.Remove((ConsoleColor)DiffSwitcher.Colours[ChosenLine - 3].SettingValue.Number);
                            DiffSwitcher.Colours[ChosenLine - 3].ChangeValue(1, ChosenLine, GameSettings[0].SettingValue.Number * GameSettings[1].SettingValue.Number, GameSettings[2].SettingValue.Number, Reprint);
                            Program.TakenColours.Add((ConsoleColor)DiffSwitcher.Colours[ChosenLine - 3].SettingValue.Number);
                        }
                        else //A pro pravou šipku s +1
                        {
                            GameSettings[ChosenLine].ChangeValue(1, ChosenLine, GameSettings[0].SettingValue.Number * GameSettings[1].SettingValue.Number, GameSettings[2].SettingValue.Number, Reprint);
                        }
                        break;
                    case ConsoleKey.Enter: //Opět je možné potvrdit Enterem a vrátit 0
                        return 0;
                    case ConsoleKey.Escape: //Nebo zrušit Escapem a vrátit -2
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
    }
}
