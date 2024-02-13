using System;
using System.Diagnostics;

namespace MTSim.Utils
{
    public static class StopwatchHelper
    {
        public static double ElapsedSeconds(long started, long finished)
        {
            return (double)(finished - started) / Stopwatch.Frequency;
        }

        public static TimeSpan Elapsed(long started, long finished)
        {
            return TimeSpan.FromSeconds(ElapsedSeconds(started, finished));
        }
    }
}
