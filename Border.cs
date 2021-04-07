using System;

namespace GloriousMinesweeper
{
    class Border : IGraphic
    {
        private Coordinates StartPoint { get; }
        private int Heigth { get; }
        private int Width { get; }
        private ConsoleColor InsideColour { get; set; }
        private ConsoleColor BorderColour { get; }
        private bool PrintInside { get; }
        public Border(int StartPointHorizontal, int StartPointVertical, int height, int width, ConsoleColor inside, ConsoleColor border, bool filled)
        {
            StartPoint = new Coordinates(StartPointHorizontal, StartPointVertical);
            Heigth = height;
            Width = width;
            InsideColour = inside;
            BorderColour = border;
            PrintInside = filled;

        }
        public Border(Coordinates TopLeft, int height, int width, ConsoleColor inside, ConsoleColor border, bool filled)
        {
            StartPoint = TopLeft;
            Heigth = height;
            Width = width;
            InsideColour = inside;
            BorderColour = border;
            PrintInside = filled;
        }
        public void Print(bool Solid)
        {
            /*Tuple<int, int> ChangeByTuple;
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Heigth; y++)
                {
                    if (Solid)
                    {
                        if (x != 0 && x != 1 && y != 0 && x != Width - 2 && x != Width - 1 && y != Heigth - 1 && !PrintInside)
                            continue;
                        ChangeByTuple = new Tuple<int, int>(x, y);
                        Coordinates PrintThere = (StartPoint + ChangeByTuple);
                        PrintThere.GoTo();
                        if (x == 0 || x == 1 || y == 0 || x == Width - 1 || x == Width - 2 || y == Heigth - 1)
                            Console.BackgroundColor = BorderColour;
                        else if (PrintInside)
                            Console.BackgroundColor = InsideColour;
                        else
                            continue;

                    }
                    else
                    {
                        if (x != 0 && y != 0 && x != Width - 1 && y != Heigth - 1 && !PrintInside)
                            continue;
                        ChangeByTuple = new Tuple<int, int>(x, y);
                        Coordinates PrintThere = (StartPoint + ChangeByTuple);
                        PrintThere.GoTo();
                        if (x == 0 || y == 0 || x == Width - 1 || y == Heigth - 1)
                            Console.BackgroundColor = BorderColour;
                        else if (PrintInside)
                            Console.BackgroundColor = InsideColour;
                        else
                            continue;
                    }
                    Console.Write(' ');
                    Console.BackgroundColor = ConsoleColor.Black;
                }
            }*/
            Coordinates CurrentCoordinates;
            if (PrintInside)
            {
                for (int x = 0; x < Width; x++)
                {
                    for (int y = 0; y < Heigth; y++)
                    {
                        if (Solid)
                        {
                            if (x != 0 && x != 1 && y != 0 && x != Width - 2 && x != Width - 1 && y != Heigth - 1)
                                Console.BackgroundColor = InsideColour;
                            else
                                Console.BackgroundColor = BorderColour;
                        }
                        else
                        {
                            if (x != 0 && y != 0 && x != Width - 1 && y != Heigth - 1)
                                Console.BackgroundColor = InsideColour;
                            else
                                Console.BackgroundColor = BorderColour;
                        }
                        CurrentCoordinates = new Coordinates(StartPoint, x, y);
                        CurrentCoordinates.GoTo();
                        Console.Write(' ');
                    }
                }
            }
            else
            {
                Console.BackgroundColor = BorderColour;
                StartPoint.GoTo();
                Console.Write(new string(' ', Width));
                CurrentCoordinates = new Coordinates(StartPoint, 0, Heigth - 1);
                CurrentCoordinates.GoTo();
                Console.Write(new string(' ', Width));
                CurrentCoordinates = StartPoint;
                for (int x = 0; x < Heigth - 1; x++)
                {
                    CurrentCoordinates = new Coordinates(CurrentCoordinates, 0, 1);
                    CurrentCoordinates.GoTo();
                    Console.Write(' ');
                }
                CurrentCoordinates = new Coordinates(StartPoint, Width - 1, 0);
                for (int x = 0; x < Heigth - 1; x++)
                {
                    CurrentCoordinates = new Coordinates(CurrentCoordinates, 0, 1);
                    CurrentCoordinates.GoTo();
                    Console.Write(' ');
                }
                if (Solid)
                {
                    CurrentCoordinates = new Coordinates(StartPoint, 1, 0);
                    for (int x = 0; x < Heigth - 1; x++)
                    {
                        CurrentCoordinates = new Coordinates(CurrentCoordinates, 0, 1);
                        CurrentCoordinates.GoTo();
                        Console.Write(' ');
                    }
                    CurrentCoordinates = new Coordinates(StartPoint, Width - 2, 0);
                    for (int x = 0; x < Heigth - 1; x++)
                    {
                        CurrentCoordinates = new Coordinates(CurrentCoordinates, 0, 1);
                        CurrentCoordinates.GoTo();
                        Console.Write(' ');
                    }
                }
            }
            Console.BackgroundColor = ConsoleColor.Black;
        }
        public void ChangeColour(int Colour)
        {
            InsideColour = (ConsoleColor)Colour;
        }
    }
}
