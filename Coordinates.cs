using System;

namespace GloriousMinesweeper
{
    class Coordinates
    {
        public int Horizontal { get; protected set; }
        public int Vertical { get; protected set; }

        public Coordinates(int horizontal, int vertical)
        {
            Horizontal = horizontal;
            Vertical = vertical;
        }

        public void GoTo()
        {
            Console.SetCursorPosition(Horizontal, Vertical);
        }
        public override string ToString()
        {
            string toString = Horizontal.ToString() + ',' + Vertical.ToString();
            return toString;
        }
        public Coordinates(Coordinates original, int HorizontalChange, int VerticalChange)
        {
            Horizontal = original.Horizontal + HorizontalChange;
            Vertical = original.Vertical + VerticalChange;
        }
    }
}
