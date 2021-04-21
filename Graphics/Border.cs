using System;

namespace GloriousMinesweeper
{
    class Border : IGraphic
    {
        ///Shrnutí
        ///Třída grafických objektů -> obdélníků, které ohraničují nějaký text nebo obrazovku
        ///Dědí z rozhraní IGraphic, odkud dostává metodu Print(bool Solid, Action Reprint), která vytiskne obdélník a metodu ChangeColour(int Colour), kterým se přebarví vnitřek obdélníka na zvolené číslo
        private Coordinates StartPoint { get; } //Levý horní bod obdélníka
        private int Heigth { get; } //Výška obdélníku
        private int Width { get; } //Šířka obdélníku
        private ConsoleColor InsideColour { get; set; } //Barva vnitřku obdélníku
        private ConsoleColor BorderColour { get; } //Barva ohraničení obdélníku
        private bool PrintInside { get; } //True: obdélník je vyplněný; False: Jedná se pouze o ohraničení
        public Border(int StartPointHorizontal, int StartPointVertical, int height, int width, ConsoleColor inside, ConsoleColor border, bool filled)
        {
            ///Shrnutí
            ///Konstruktor, který přijme souřadnice startovního bodu jakožto dvě čísla
            if (((Console.LargestWindowWidth - 5) > Console.WindowWidth) || ((Console.LargestWindowHeight - 3) > Console.WindowHeight))
                Program.WaitForFix();
            StartPoint = new Coordinates(StartPointHorizontal, StartPointVertical); //Zde se levý horní bod ještě vytvoří přes konstruktor Coordinates
            Heigth = height;
            Width = width;
            InsideColour = inside;
            BorderColour = border;
            PrintInside = filled;

        }
        public Border(Coordinates TopLeft, int height, int width, ConsoleColor inside, ConsoleColor border, bool filled)
        {
            ///Shrnutí
            ///Konstruktor, který přijme souřadnice startovního bodu jakožto objekt typu Coordinates
            StartPoint = TopLeft;
            Heigth = height;
            Width = width;
            InsideColour = inside;
            BorderColour = border;
            PrintInside = filled;
        }
        public void Print(bool Solid, Action Reprint)
        {
            ///Shrnutí
            ///Hlavní metoda Borderu zděděná z IGraphic
            ///Pokud je bool Solid true, tak mají všechny strany stejnou šířku (Svislé linie mají šířku dvou charů)
            ///Action Reprint, je metoda, která se má stát pokud se nepovede Border vytisknout
            Coordinates CurrentCoordinates; //Pozice, na kterou se bude nyní tisknout obdélník
            if (PrintInside) //Pokud se má tisknout vnitřek projedou se všechny x a y od StartPointu až po maxima
            {
                for (int x = 0; x < Width; x++) //Všechna x pod šířku
                {
                    for (int y = 0; y < Heigth; y++) //Všechna y pod výšku
                    {
                        if (Solid) //Pokud je obdélník solid, tak se vytiskne kromě hranic i druhý a předposlední sloupec s BorderColour
                        {
                            if (x != 0 && x != 1 && y != 0 && x != Width - 2 && x != Width - 1 && y != Heigth - 1) //Pokud se nejedná o první, druhý, předposlední nebo poslední slupec nebo první nebo poslední řádek
                                Console.BackgroundColor = InsideColour; //Tiskne se s InsideColourem
                            else
                                Console.BackgroundColor = BorderColour; //Jinak se tiskne s BorderColourem
                        }
                        else
                        {
                            if (x != 0 && y != 0 && x != Width - 1 && y != Heigth - 1) //To samé, ale bez druhého a předposledního sloupce
                                Console.BackgroundColor = InsideColour;
                            else
                                Console.BackgroundColor = BorderColour;
                        }
                        CurrentCoordinates = new Coordinates(StartPoint, x, y); //Posuneme se ze startovního bodu o současné hodnoty x a y pomocí konstruktoru Coordinates
                        CurrentCoordinates.GoTo(Reprint); //Přejdeme na nové souřadnice, pokud se na ně nedá dostat, nastane metoda Reprint
                        Console.Write(' '); //A vytiskne se mezera s barvou která se nastavila
                    }
                }
            }
            else
            {
                Console.BackgroundColor = BorderColour; //Pokud tiskneme jen okraj tak se nastaví barva na vnější 
                StartPoint.GoTo(Reprint); //Přejde se na startovní souřadnice, pokud se na ně nedá dostat, nastane metoda Reprint
                Console.Write(new string(' ', Width)); //Napíše se počet mezer, který se rovná šířce obdélníka
                CurrentCoordinates = new Coordinates(StartPoint, 0, Heigth - 1); //Spočítá se dolní levý bod
                CurrentCoordinates.GoTo(Reprint); //Přesuneme se na dolní levý bod
                Console.Write(new string(' ', Width)); //Napíše se počet mezer, který se rovná šířce obdélníka 
                CurrentCoordinates = StartPoint; //Vrátíme se na startovní bod a jdou se tisknout svislé linie
                for (int x = 0; x < Heigth - 1; x++) //Pro výšku mínus jedna
                {
                    CurrentCoordinates = new Coordinates(CurrentCoordinates, 0, 1); //Se vypočítají souřadnice o jedna níže
                    CurrentCoordinates.GoTo(Reprint); //Přejde se na nové souřadnice
                    Console.Write(' '); //Na těchto souřadnicích se znovu napíše mezera
                }
                CurrentCoordinates = new Coordinates(StartPoint, Width - 1, 0); //Nyní se přesuneme na bod vpravo nahoře
                for (int x = 0; x < Heigth - 1; x++) //Znovu pro výšku mínus jedna
                {
                    CurrentCoordinates = new Coordinates(CurrentCoordinates, 0, 1); //Se vypočítají souřadnice o jedna níže
                    CurrentCoordinates.GoTo(Reprint); //Přejede se na nové sořadnice
                    Console.Write(' '); //Na těchto souřadnicích se opět napíše mezera
                }
                if (Solid) //Pokud má být Border Solid, tak se vytisknou obdobným způsobem ještě dva sloupce. Jeden vpravo od levého, druhý velvo od pravého
                {
                    CurrentCoordinates = new Coordinates(StartPoint, 1, 0);
                    for (int x = 0; x < Heigth - 1; x++)
                    {
                        CurrentCoordinates = new Coordinates(CurrentCoordinates, 0, 1);
                        CurrentCoordinates.GoTo(Reprint);
                        Console.Write(' ');
                    }
                    CurrentCoordinates = new Coordinates(StartPoint, Width - 2, 0);
                    for (int x = 0; x < Heigth - 1; x++)
                    {
                        CurrentCoordinates = new Coordinates(CurrentCoordinates, 0, 1);
                        CurrentCoordinates.GoTo(Reprint);
                        Console.Write(' ');
                    }
                }
            }
            Console.BackgroundColor = ConsoleColor.Black;
        }
        public void ChangeColour(int Colour)
        {
            ///Shrnutí
            ///Změna barvy, která přebarví vnitřek obdélnika
            InsideColour = (ConsoleColor)Colour;
        }
    }
}
