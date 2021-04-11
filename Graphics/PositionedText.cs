using System;

namespace GloriousMinesweeper
{
    class PositionedText : PositionedObject
    {
        ///Shrnutí
        ///Objekt typu PositionedText je string, který má svoji pozici v Consoli a nastavenou barvu pozadí
        public string Text { get; } //Kromě základních hodnot PositionedObjectu má ještě svůj text

        public PositionedText(string text, ConsoleColor background, int horizontal, int vertical) : base(background, horizontal, vertical)
        {
            Text = text; //Základní konstruktor, který přijme daný text, barvu pozadí a pozici na kterou se má tisknout
        }
        public override void Print(bool highlight, Action Reprint)
        {
            ///Shrnutí
            ///Text se vytiskne na své pozici
            base.Print(highlight, Reprint); //Metoda base.Print() nás dostane na správnou pozici
            Console.Write(Text); //Následně se vypíše text
        }
        public override string ToString()
        {
            ///Shrnutí
            ///Metoda ToString vrátí text, který je zde uložen
            return Text;
        }
    }
}
