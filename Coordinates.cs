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
        public bool CompareCoordinates(Coordinates compareWith)
        {
            return (Horizontal == compareWith.Horizontal && Vertical == compareWith.Vertical);
        }
    }
}
