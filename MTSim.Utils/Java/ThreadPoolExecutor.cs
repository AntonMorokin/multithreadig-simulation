using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Threading;

namespace MTSim.Utils.Java
{
    /// <summary>
    /// Emulate Java's ThreadPoolExecutor
    /// </summary>
    public sealed class ThreadPoolExecutor
    {
        private static readonly int Threads = (int)Math.Ceiling(Environment.ProcessorCount * 0.8m);

        public static Task InvokeAll(IEnumerable<Action> tasks, CancellationToken cancellationToken)
        {
            return Task.WhenAll(tasks.Select(task => Task.Run(task, cancellationToken)));
        }

        public static Task InvokeAll<T>(IEnumerable<T> source, Action<T> action, CancellationToken cancellationToken)
        {
            var options = new ParallelOptions
            {
                MaxDegreeOfParallelism = Threads,
                CancellationToken = cancellationToken
            };

            return Task.Run(() => Parallel.ForEach(source, options, action), cancellationToken);
        }

        public static async Task<IReadOnlyCollection<T>> InvokeAll<T>(IEnumerable<Func<T>> tasks)
        {
            return await Task.WhenAll(tasks.Select(Task.Run));
        }

        public static Task InvokeAll<T>(IEnumerable<Func<CancellationToken, Task>> tasks, CancellationToken cancellationToken)
        {
            return Task.WhenAll(tasks.Select(task => Task.Run(() => task(cancellationToken), cancellationToken)));
        }
    }
}
