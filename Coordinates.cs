using System;

namespace GloriousMinesweeper
{
    class Coordinates
    {
        ///Shrnutí
        ///Jednoduchá třída, která obshauje dvě čísla – horizontální a vertikální souřadnici
        public int Horizontal { get; protected set; }
        public int Vertical { get; protected set; }

        public Coordinates(int horizontal, int vertical) //Konstruktor, který přijme souřadnice
        {
            Horizontal = horizontal;
            Vertical = vertical;
        }
        public Coordinates(Coordinates original, int HorizontalChange, int VerticalChange) //Konstruktor, který přijme jiné Coordinates a posune je o zvolené hodnoty
        {
            Horizontal = original.Horizontal + HorizontalChange;
            Vertical = original.Vertical + VerticalChange;
        }
        public void GoTo(Action Reprint)
        {
            ///Shrnutí
            ///Metoda, která se pokusí přesunout kurzor na pozici danou těmito souřadnicemi
            if (Horizontal < 0 || Horizontal > Console.LargestWindowWidth || Vertical < 0 || Vertical > Console.LargestWindowHeight) //Pokud jsou souřadnice mimo maximální rozměry obrazovky, tak hra nemůže být spuštěna na tomto zařízení
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Clear();
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("This application can not run on your device");
                Console.ReadKey();
                Environment.Exit(0);
            }
            if (((Console.LargestWindowWidth - 5) > Console.WindowWidth) || ((Console.LargestWindowHeight - 3) > Console.WindowHeight)) //Pokud hra není na celou obrazovku, tak se počká než to uživatel napraví a pak se znovu vytiskne to, co má být na obrazovce (to je uloženo v Action Reprint)
            {
                Program.WaitForFix();
                Reprint();
            }
            Console.SetCursorPosition(Horizontal, Vertical); //Nakonec se tedy přesuneme na cílovou pozici
        }
        public override string ToString() 
        {
            ///Shrnutí
            ///Metoda ToString() vrátí obě dvě čísla oddělená čárkou
            ///Tato metoda je důležitá při ukládání hry do souboru
            string toString = Horizontal.ToString() + ',' + Vertical.ToString();
            return toString;
        }
        
    }
}
