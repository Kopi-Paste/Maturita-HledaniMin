

namespace GloriousMinesweeper
{
    class ChangableCoordinates : Coordinates
    {
        private int HorizontalMax { get; }
        private int VerticalMax { get; }


        public ChangableCoordinates(int horizontal, int vertical, int horizontalMax, int verticalMax) : base(horizontal, vertical)
        {
            HorizontalMax = horizontalMax;
            VerticalMax = verticalMax;
        }
        public void GoLeft()
        {
            if (Horizontal != 0)
                Horizontal--;
        }
        public void GoRight()
        {
            if (Horizontal != HorizontalMax)
                Horizontal++;
        }
        public void GoUp()
        {
            if (Vertical != 0)
                Vertical--;
        }
        public void GoDown()
        {
            if (Vertical != VerticalMax)
                Vertical++;
        }
    }
}
