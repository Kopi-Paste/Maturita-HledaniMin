using System;
using System.Diagnostics;

namespace GloriousMinesweeper
{
    class SpecialisedStopwatch : Stopwatch
    {
        private long StartingMilliseconds { get; }
        private long StartingTicks { get; }
        private TimeSpan StartTime { get; }
        public SpecialisedStopwatch(string startingMilliseconds, string startingTicks) : base()
        {
            StartingMilliseconds = long.Parse(startingMilliseconds);
            StartingTicks = long.Parse(startingTicks);
            StartTime = new TimeSpan(StartingTicks);
        }
        public new long ElapsedMilliseconds
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
            return String.Format("{0:00}:{1:00}:{2:00}.{3:00} ",
                Elapsed.Hours, Elapsed.Minutes, Elapsed.Seconds, Elapsed.Milliseconds / 10);
        }
    }
}
