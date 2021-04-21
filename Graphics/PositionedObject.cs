using System;

namespace GloriousMinesweeper
{
    abstract class PositionedObject : IGraphic
    {
        ///Shrnutí
        ///PositionedObject je abstraktní třída, ze které dědí PositionedText a PositionedNumber
        ///Jedná se o nějaký text nebo číslo, které má v sobě danou pozici (Coordinates), kde se má tisknout
        ///Dědí z rozhraní IGraphic, odkud dostává metodu Print(bool Solid, Action Reprint), kterým se daný objekt vytiskne a metodu ChangeColour(int Colour), kterým se přebarví barva pozadí.
        private Coordinates Position { get; } //Pozice na kterou je objekt umístěn
        public ConsoleColor Background { get; private set; } //Barva pozadí, na kterém má být daný objektvytištěn
        public PositionedObject(ConsoleColor background, int horizontal, int vertical) //Konstruktor vytvoří z dvou čísel Pozici pomocí konstruktoru Coordinates a zároveň uloží vloženou barvu do fieldu Background
        {
            if (((Console.LargestWindowWidth - 5) > Console.WindowWidth) || ((Console.LargestWindowHeight - 3) > Console.WindowHeight))
                Program.WaitForFix();
            Background = background;
            Position = new Coordinates(horizontal, vertical);
        }
        public virtual void Print(bool highlighted, Action Reprint)
        {
            ///Shrnutí
            ///Hlavní metoda PositionedObjectu zděděná z IGraphic
            ///Vytiskne Object na danou pozici Position
            ///Přijímá bool Highlighted, který určuje zda se má objekt vytisknout vyznačeně (bílou barvou)
            ///Dále přijímá Action Reprint, metodu, která se má udít v případě, že se nepodaří přejít na danou pozici
            Console.CursorVisible = false; //Před každým tištěním se obnoví skrytí kurzoru
            Console.BackgroundColor = Background; //Barva pozadí se nastaví na barvu fieldu Background
            if (highlighted) //Barva textu se nastaví na bílou pokud má být objekt highlighted (zvýrazněn), nebo na výchozí barvu, která je uložena ve fieldu Program.DefaultTextColour
                Console.ForegroundColor = ConsoleColor.White;
            else
                Console.ForegroundColor = Program.DefaultTextColour;
            Position.GoTo(Reprint); //Přejde se na danou pozici
            //Následují overridy jednotlivých tříd
        }
        public virtual void ChangeColour(int backgroundChangeTo)
        {
            ///Shrnutí
            ///Jednoduchá metoda, která změní nastavení barvy v pozadí na zvolené číslo
            Background = (ConsoleColor)backgroundChangeTo; //Barva dle ConsoleColor Enumu
        }
    }
}
