using System;

namespace GloriousMinesweeper
{
    class PositionedNumber : PositionedObject
    {


        public int Number { get; private set; }
        public PositionedNumber(int number, ConsoleColor background, int horizontal, int vertical) : base(background, horizontal, vertical)
        {
            Number = number;
        }
        public void ChangeBy(int value, Action Reprint)
        {
            base.Print(false, Reprint);
            Console.BackgroundColor = ConsoleColor.Black;
            int numberOfDigits;
            if (Number == 0)
                numberOfDigits = 0;
            else
                numberOfDigits = (int)Math.Floor(Math.Log10(Number)) + 1;
            Console.Write(new string(' ', numberOfDigits));
            Number += value;
        }
        public void ChangeTo(int number, Action Reprint)
        {
            base.Print(false, Reprint);
            Console.BackgroundColor = ConsoleColor.Black;
            int numberOfDigits;
            if (Number == 0)
                numberOfDigits = 0;
            else
                numberOfDigits = (int)Math.Floor(Math.Log10(Number)) + 1;
            Console.Write(new string(' ', numberOfDigits));
            Number = number;
        }
        public override void Print(bool highlight, Action Reprint)
        {
            base.Print(highlight, Reprint);
            Console.Write(Number);
        }
        public void PrintWithConsoleColourEnum(bool highlight, Action Reprint)
        {
            base.Print(highlight, Reprint);
            Console.Write((ConsoleColor)Number);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("       ");
        }
        public override string ToString()
        {
            return Number.ToString();
        }
    }
}
