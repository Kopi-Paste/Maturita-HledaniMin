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
        public void ChangeBy(int value)
        {
            base.Print(false);
            Console.BackgroundColor = ConsoleColor.Black;
            int numberOfDigits;
            if (Number == 0)
                numberOfDigits = 0;
            else
                numberOfDigits = (int)Math.Floor(Math.Log10(Number)) + 1;
            Console.Write(new string(' ', numberOfDigits));
            Number += value;
        }
        public void ChangeTo(int number)
        {
            base.Print(false);
            Console.BackgroundColor = ConsoleColor.Black;
            int numberOfDigits;
            if (Number == 0)
                numberOfDigits = 0;
            else
                numberOfDigits = (int)Math.Floor(Math.Log10(Number)) + 1;
            Console.Write(new string(' ', numberOfDigits));
            Number = number;
        }
        public override void Print(bool highlight)
        {
            base.Print(highlight);
            Console.Write(Number);
        }
        public void PrintWithConsoleColourEnum(bool highlight)
        {
            base.Print(highlight);
            Console.Write((ConsoleColor)Number);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("       ");
        }
    }
}
