using System;

namespace GloriousMinesweeper
{
    abstract class PositionedObject : IGraphic
    {
        private Coordinates Position { get; }
        public ConsoleColor Background { get; private set; }
        public PositionedObject(ConsoleColor background, int horizontal, int vertical)
        {
            Background = background;
            Position = new Coordinates(horizontal, vertical);
        }
        public virtual void Print(bool highlighted, Action Reprint)
        {
            Console.CursorVisible = false;
            Console.BackgroundColor = Background;
            if (highlighted)
                Console.ForegroundColor = ConsoleColor.White;
            else
                Console.ForegroundColor = Program.DefaultTextColour;
            Position.GoTo(Reprint);
        }
        public virtual void ColourChangeBy(int foregroundChangeBy, int backgroundChangeBy)
        {
            Background += backgroundChangeBy;
        }
        public virtual void ChangeColour(int backgroundChangeTo)
        {
            Background = (ConsoleColor)backgroundChangeTo;
        }
    }
}
