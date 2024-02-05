using System.Threading.Tasks;
using System;
using System.Threading;
using System.Diagnostics;

namespace MTSim.Utils.Java
{
    /// <summary>
    /// Emulate Java's ScheduledThreadPoolExecutor
    /// </summary>
    public sealed class ScheduledThreadPoolExecutor
    {
        public static Task Schedule(Action action, TimeSpan delay)
        {
            return Task.Delay(delay).ContinueWith(_ =>
            {
                action();
            });
        }

        public static Task<T> Schedule<T>(Func<T> callable, TimeSpan delay)
        {
            return Task.Delay(delay).ContinueWith(_ =>
            {
                return callable();
            });
        }

        public static Task ScheduleAtFixedRate(Func<CancellationToken, Task> runnable, TimeSpan initialDelay, TimeSpan period, CancellationToken cancellationToken)
        {
            // don't like two awaits
            return Task.Run(async () =>
            {
                await Task.Delay(initialDelay, cancellationToken);
                await ExecutionLoop(runnable, period, cancellationToken);
            }, cancellationToken);
        }

        private static async Task ExecutionLoop(Func<CancellationToken, Task> runnable, TimeSpan period, CancellationToken cancellationToken)
        {
            var periodSeconds = period.TotalSeconds;

            while (!cancellationToken.IsCancellationRequested)
            {
                var started = Stopwatch.GetTimestamp();

                await runnable(cancellationToken);

                await WaitAsync(started, Stopwatch.GetTimestamp(), periodSeconds, cancellationToken);
            }
        }

        private static async ValueTask WaitAsync(long started, long finished, double periodSeconds, CancellationToken cancellationToken)
        {
            var elapsedTicks = (double)(finished - started);
            var elapsedSeconds = elapsedTicks / Stopwatch.Frequency;
            var needToWait = periodSeconds - elapsedSeconds;

            if (needToWait > 0)
            {
                await Task.Delay(TimeSpan.FromSeconds(needToWait), cancellationToken);
            }
        }
    }
}
