using System;

namespace GloriousMinesweeper
{
    class PositionedNumber : PositionedObject
    {
        ///Shrnutí
        ///Objekt typu PositionedNumber je číslo, které má svoji pozici v Consoli a nastavenou barvu pozadí
        public int Number { get; private set; } //Kromě základních hodnot PositionedObjectu má ještě svoji číselnou hodnotu
        public PositionedNumber(int number, ConsoleColor background, int horizontal, int vertical) : base(background, horizontal, vertical) //Základní konstruktor, který přijme hodnotu čísla, barvu pozadí a pozici na kterou se má tisknout
        {
            Number = number;
        }
        public void ChangeBy(int value, Action Reprint)
        {
            ///Shrnutí
            ///Číslo se změní o danou hodnotu a znovu se vytiskne
            base.Print(false, Reprint); //metoda base.Print() nás dostane na správnou pozici
            Console.BackgroundColor = ConsoleColor.Black; //Nejprve se smaže původsní číslo, nastaví se barva pozadí na černou
            int numberOfDigits; //Nyní se spočítá počet číslic
            if (Number == 0) //Pokud se číslo rovná nule délka, počet číslic se rovná 1
                numberOfDigits = 1;
            else
                numberOfDigits = (int)Math.Floor(Math.Log10(Number)) + 1; //Jinak se počet číslic rovná logaritmu čísla o základu deset zaokrouhleného dolů + 1
            Console.Write(new string(' ', numberOfDigits)); //Přemaže se tedy číslo počtem mezer, který odpovídá počtu číslic
            Number += value; //Změní se hodnota o zadanou value
            Print(false, Reprint); //A číslo se vytiskne znovu
        }
        public void ChangeTo(int number, Action Reprint)
        {
            ///Shrnutí
            ///Číslo se změní na danou hodnotu a znovu se vytiskne
            base.Print(false, Reprint); //Opět se provede to samé
            Console.BackgroundColor = ConsoleColor.Black;
            int numberOfDigits;
            if (Number == 0)
                numberOfDigits = 0;
            else
                numberOfDigits = (int)Math.Floor(Math.Log10(Number)) + 1;
            Console.Write(new string(' ', numberOfDigits));
            Number = number; //Tentokrát se však použije = a ne +=
            Print(false, Reprint);
        }
        public override void Print(bool highlight, Action Reprint)
        {
            ///Shrnutí
            ///Číslo se vytiskne na své pozici
            base.Print(highlight, Reprint); //Metoda base.Print() nás dostane na správné místo
            Console.Write(Number); //A napíše se naše číslo
        }
        public void PrintWithConsoleColourEnum(bool highlight, Action Reprint)
        {
            ///Shrnutí
            ///Speciální tisknutí PositionedNumberu
            ///Místo běžného čísla se vytiskne barva, která je na pozici v ConsoleColor Enumu daného čísla
            base.Print(highlight, Reprint); //Metoda base.Print() nás dostane na správné místo
            Console.Write((ConsoleColor)Number); //Vypíše se zvolená barva
            Console.BackgroundColor = ConsoleColor.Black; //Nyní se nastaví jako barva pozadí černá
            Console.Write("       "); //A smaží se přebývající znaky za
        }
        public override string ToString()
        {
            ///Shrnutí
            ///Metoda ToString vrací číselnou hodnotu čísla přes Int.ToString()
            return Number.ToString();
        }
    }
}
