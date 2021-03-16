﻿using System;

namespace GloriousMinesweeper
{
    class PositionedText : PositionedObject
    {

        public string Text { get; private set; }
        
        public PositionedText(string text, ConsoleColor background, int horizontal, int vertical) : base(background, horizontal, vertical)
        {
            Text = text;
        }
        public void ChangeBy(string value)
        {
            Text += value;
        }
        public void ChangeTo(string text)
        {
            Text = text;
        }
        public override void Print(bool highlight)
        {
            base.Print(highlight);
            Console.Write(Text);
        }
    }
}
