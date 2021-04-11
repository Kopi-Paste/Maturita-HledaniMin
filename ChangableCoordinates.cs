namespace GloriousMinesweeper
{
    class ChangableCoordinates : Coordinates
    {
        ///Shrnutí
        ///Tato třída dědí z Coordinates
        ///Opět se jedná o dvě čísla, ale tato třída má metody, jak je změnit
        ///Zároveň má dva fieldy: horizontální a vertikální maximum, za které se čísla nemohou dostat. V obou rozměrech je minimum nula
        private int HorizontalMax { get; }
        private int VerticalMax { get; }


        public ChangableCoordinates(int horizontal, int vertical, int horizontalMax, int verticalMax) : base(horizontal, vertical) //Konstruktor funguje stejně jako ten pro Coordinates, pouze vyžaduje čísla udávající maxima
        {
            HorizontalMax = horizontalMax;
            VerticalMax = verticalMax;
        }
        //Jsou zde čtyři metody, každá ověří možnost pohybu ve svém směru a poté pohybuje souřadnicemi svým směrem
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
