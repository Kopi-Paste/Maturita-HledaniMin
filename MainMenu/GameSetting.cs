using System;

namespace GloriousMinesweeper
{
    class GameSetting
    {
        ///Shruntí
        ///Třída herních nastavení, která se dají měnit
        public PositionedText Setting { get; private set; } //Název nastavení
        public PositionedNumber SettingValue { get; private set; } //Hodnota nastavení
        public bool Colour { get; } //Boolean, který určuje zda se jedná o nastavení barvy
        public bool TextColour { get; } //Boolean, který určuje zda se jedná o nastavení barvy textu

        public GameSetting(string text, int defaultValue, bool colourable, bool textColour, int line) //Konstruktor obdrží název nastavení, jeho výchozí hodnotu, booleany týkající se barev a pozici
        {
            text += ": "; //K názvu se přidá dvojtečka a mezera
            ConsoleColor background;
            if (colourable)
                background = (ConsoleColor)defaultValue;
            else
                background = ConsoleColor.Black;
            Colour = colourable;
            TextColour = textColour;
            Setting = new PositionedText(text, background, ((Console.WindowWidth - (text.Length + 8)) / 2), line); //Název a hodnota se převedou do grafické podoby
            SettingValue = new PositionedNumber(defaultValue, background, ((Console.WindowWidth + text.Length - 8) / 2), line);
        }


        public void ChangeValue(int change, int chosenLine, int tiles, int mines, Action Reprint)
        {
            ///Shrnutí
            ///Metoda, která změní hodnotu o dané číslo (na začátku vždy -1 nebo +1)
            ///Dostává další argumenty, podle kterých určuje zda je změna možná
            if (Colour || TextColour) //Pokud se jedná o barvu
            {
                while (Program.TakenColours.Contains((ConsoleColor)SettingValue.Number + change)) //Tak se nejprve ověří že barva, na kterou by se přešlo již není zabraná Pokud ano zvýší se změna o 1 v daném směru (Pokud je -1 tak na -2, pokud je 1 tak na 2). A tak dále až dokud se nedostaneme k barvě, která ještě není zabraná
                {
                    if (change < 0) 
                        change--;
                    else
                        change++;
                }
                if (SettingValue.Number + change >= 15) //Pokud barva dosáhne 15 (tedy se dosatne na bílou, která je rezervovaná pro zvýrazněnou grafiku), vrátí se zpátky na 0 a zavolá se znovu tato metoda se vstupní změnou +1
                {
                    SettingValue.ChangeTo(0, Reprint);
                    ChangeValue(1, chosenLine, tiles, mines, Reprint);
                    return;
                }
                else if (SettingValue.Number + change <= 0) //To samé pokud barva dosáhne 0 (tedy se dostane na černou, která je používané pro prázdné prostory), skočí na 15 a zavolá se znovu tato metoda se vstupní změnou -1
                {
                    SettingValue.ChangeTo(15, Reprint);
                    ChangeValue(-1, chosenLine, tiles, mines, Reprint);
                    return;
                }
                SettingValue.ChangeBy(change, Reprint); //Když se dostaneme na barvu, která není zabraná, tak se změní toto nastavení na nově zvolenou barvu
                if (Colour) //Pokud se jedná o klasickou barvu změní se zobrazení tohoto GameSettingu, Název i hodnota teď budou mít pozadí této nové barvy
                {
                    Setting.ChangeColour(SettingValue.Number);
                    SettingValue.ChangeColour(SettingValue.Number);
                }
                else if (TextColour) //Pokud se jedná o barvu textu, tak se změní hodnota fieldu Program.DefaultTextColour a vytiskne se toto menu znovu bez Borders, které tato změna nijak neovlivní
                {
                    Program.DefaultTextColour = (ConsoleColor)SettingValue.Number;
                    DiffSwitcher.GameMenus[DiffSwitcher.ChosenMenu].PrintMenu(false);
                }
                Print(true, Reprint); //Na závěr se toto nastavení vytiskne, vzhledem k tomu, že jej měníme, tak je určitě zgvýrazněné
            }
            else if (Setting.Text.EndsWith("tiles: ")) //Pokud se jedná o nastavení políček
            {
                int otherValue = tiles / SettingValue.Number; //Spočítá se hodnota druhého nastavení z celkového počtu políček, který je vstupní hodnotou
                if (((SettingValue.Number + change) < 4 || (SettingValue.Number + change) > 50) || (((SettingValue.Number + change) * otherValue) < (mines + 20))) //Políček nesmí být ani v jednom rozměru méně než čtyři, více než padesát nebo dohromady tolik, že by se rozdíl mezi počtem políček a počtem min dostal pod dvacet
                { }
                else if ((Setting.Text == "Number of horizontal tiles: ") &&  (2 *(SettingValue.Number + change)) > (Console.WindowWidth - 115)) //Zároveň nesmí být horizontálních políček tolik, že by se hrací plocha s okolními grafikami nevešla na obrazovku
                { }
                else if ((Setting.Text == "Number of vertical tiles: ") && (SettingValue.Number + change) > (Console.WindowHeight - 4)) //To stejné platí i pro vertikální políčka
                { }
                else
                    SettingValue.ChangeBy(change, Reprint); //Pokud jsou všechny podmínky splněny, může se počet políček změnit
            }
            else //else platí pro případy, kdy se jedná o nastavení počtu min
            {
                if ((SettingValue.Number + change) < 2 || (SettingValue.Number + change) > (tiles - 20)) //Počet min nesmí klesnout pod dvě a zároveň nesmí přesáhnout počet políček - 20
                { }
                else
                    SettingValue.ChangeBy(change, Reprint); //Pokud je podmínka splněna, může se počet min změnit
            }
        }
        public void ChangeValueTo(int newValue, Action Reprint, bool immediatePrint = true)
        {
            ///Shruntí
            ///Tato metoda nastaví hodnoty barev na dané číslo
            if (!Colour && !TextColour) //Metoda mění pouze nastavení barev
                return;
            SettingValue.ChangeTo(newValue, Reprint, immediatePrint); //Provede se změna čísla
            if (Colour) //Pokud se jedná o klasickou barvu,
            {
                Setting.ChangeColour(newValue); //tak se nastaví nová barva do pozadí pro název i hodnotu nastavení
                SettingValue.ChangeColour(newValue);
            }
            else //Pokud se jedná o barvu Textu
            {
                Program.DefaultTextColour = (ConsoleColor)newValue; //Tak se přenastaví field Program.DefaultTextColour
            }

        }
        public void Print(bool highlight, Action Reprint)
        {
            ///Shrnutí
            ///Tato metoda vytiskne název i hodnotu nastavení
            ///Pomocí vstupní hodnoty highlight je možné oba dva PositionedObjecty zvýraznit
            ///Pokud při tištění není hra nastavena na celou obrazovku, počká se na opravu od uživatele, vymaže se Console a vytiskne se menu znovu
            Setting.Print(highlight, Reprint); //Vytiskne název
            if (Colour || TextColour) //Pokud se jedná o barvu textu nebo pozadí, tak se převede číslo na svoji hodnotu v ConsoleColor Enumu
                SettingValue.PrintWithConsoleColourEnum(highlight, Reprint);
            else //Jinak se vytiskne normální číslo
                SettingValue.Print(highlight, Reprint);
        }


    }
}
