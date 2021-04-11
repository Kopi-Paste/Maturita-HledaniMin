using System;
using System.Diagnostics;

namespace GloriousMinesweeper
{
    class SpecialisedStopwatch : Stopwatch
    {
        ///Shrnutí
        ///Klasická třída Stopwatch bohužel není schopná nastavit počáteční čas
        ///Proto jsem si vytvořil vlastní třídu SpecialisedStopwatch
        private long StartingMilliseconds { get; } //Sem se uloží počáteční milisekundy
        private long StartingTicks { get; } //Sem počáteční Ticks
        private TimeSpan StartTime { get; } //A sem počáteční TimeSpan
        public SpecialisedStopwatch(string startingMilliseconds, string startingTicks) : base() //Vzhledem k tomu, že se konstruktor používá především při načítání hry z .txt, jsou argumenty ve formátu string
        {
            StartingMilliseconds = long.Parse(startingMilliseconds); //Pomocí long.Parse() se načtou startovní milisekundy i Ticks
            StartingTicks = long.Parse(startingTicks);
            StartTime = new TimeSpan(StartingTicks); //Z Ticks se spočítá také startovní TimeSpan
        }
        public new long ElapsedMilliseconds //Přepíší se fieldy ElapsedMilliseconda, ElapsedTicks a Elapsed. Všechno k sobě přičte startovní hodnoty
        {
            get
            {
                return base.ElapsedMilliseconds + StartingMilliseconds;
            }
        }
        public new long ElapsedTicks
        {
            get
            {
                return base.ElapsedTicks + StartingTicks;
            }
        }
        public new TimeSpan Elapsed
        {
            get
            {
                return base.Elapsed + StartTime;
            }
        }
        public override string ToString()
        {
            ///Shrnutí
            ///Vrátí Stopwatch v hezkém formátu času
            ///HH:MM:SS.ss
            return String.Format("{0:00}:{1:00}:{2:00}.{3:00} ",
                Elapsed.Hours, Elapsed.Minutes, Elapsed.Seconds, Elapsed.Milliseconds / 10);
        }
    }
}
