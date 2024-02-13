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
        public static Task ScheduleAtFixedRate(Func<CancellationToken, Task> runnable, TimeSpan initialDelay, TimeSpan period, CancellationToken cancellationToken)
        {
            var task = Task.Factory.StartNew(async () =>
            {
                await Task.Delay(initialDelay, cancellationToken);
                await ExecutionLoop(runnable, period, cancellationToken);
            }, cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Current);

            return task.Unwrap();
        }

        public static Task ScheduleAtFixedRate(Action runnable, TimeSpan initialDelay, TimeSpan period, CancellationToken cancellationToken)
        {
            var task = Task.Factory.StartNew(async () =>
            {
                await Task.Delay(initialDelay, cancellationToken);
                await ExecutionLoop(runnable, period, cancellationToken);
            }, cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Current);

            return task.Unwrap();
        }

        private static async Task ExecutionLoop(Func<CancellationToken, Task> runnable, TimeSpan period, CancellationToken cancellationToken)
        {
            var periodSeconds = period.TotalSeconds;

            while (!cancellationToken.IsCancellationRequested)
            {
                var started = Stopwatch.GetTimestamp();

                await runnable(cancellationToken);

                await WaitIfNeededAsync(started, Stopwatch.GetTimestamp(), periodSeconds, cancellationToken);
            }
        }

        private static async Task ExecutionLoop(Action runnable, TimeSpan period, CancellationToken cancellationToken)
        {
            var periodSeconds = period.TotalSeconds;

            while (!cancellationToken.IsCancellationRequested)
            {
                var started = Stopwatch.GetTimestamp();

                runnable();

                await WaitIfNeededAsync(started, Stopwatch.GetTimestamp(), periodSeconds, cancellationToken);
            }
        }

        private static async ValueTask WaitIfNeededAsync(long started, long finished, double periodSeconds, CancellationToken cancellationToken)
        {
            var elapsedSeconds = StopwatchHelper.ElapsedSeconds(started, finished);
            var needToWait = periodSeconds - elapsedSeconds;

            if (needToWait > 0)
            {
                await Task.Delay(TimeSpan.FromSeconds(needToWait), cancellationToken);
            }
        }
    }
}
