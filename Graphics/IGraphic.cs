using System;
namespace GloriousMinesweeper
{
    interface IGraphic
    {
        ///Shrnutí
        ///Rozhraní, ze kterého dědí grafické objekty
        public void Print(bool highlight, Action Reprint); //Vytištění grafického objektu: U PositionedObject bool udává zvýraznění (vytištění bílou barvou) a u Border udává zda se mají svislé linie okraje vytisknout se šířkou dva. Action udává, co se má stát pokud se nepodaří objekt vytisknout. Nejčastěji se jedná o přetisk menu.
        public void ChangeColour(int Colour); //Změna barvy grafického objektu na zvolené číslo z ConsoleColor Enum
    }
}