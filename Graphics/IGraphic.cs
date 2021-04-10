using System;
namespace GloriousMinesweeper
{
    interface IGraphic
    {
        public void Print(bool highlight, Action Reprint);
        public void ChangeColour(int Colour);
    }
}