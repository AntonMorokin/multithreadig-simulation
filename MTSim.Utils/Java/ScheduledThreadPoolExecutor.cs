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
        public Task Schedule(Action action, TimeSpan delay)
        {
            return Task.Delay(delay).ContinueWith(_ =>
            {
                action();
            });
        }

        public Task<T> Schedule<T>(Func<T> callable, TimeSpan delay)
        {
            return Task.Delay(delay).ContinueWith(_ =>
            {
                return callable();
            });
        }

        public Task ScheduleAtFixedRate(Func<CancellationToken, Task> runnable, TimeSpan initialDelay, TimeSpan period, CancellationToken cancellationToken)
        {
            // don't like two awaits
            return Task.Run(async () =>
            {
                await Task.Delay(initialDelay, cancellationToken);
                await ExecutionLoop(runnable, period, cancellationToken);
            }, cancellationToken);
        }

        private async Task ExecutionLoop(Func<CancellationToken, Task> runnable, TimeSpan period, CancellationToken cancellationToken)
        {
            var sw = new Stopwatch();

            while (!cancellationToken.IsCancellationRequested)
            {
                sw.Restart();

                await runnable(cancellationToken);

                sw.Stop();
                await Task.Delay(period - sw.Elapsed);
            }
        }
    }
}
