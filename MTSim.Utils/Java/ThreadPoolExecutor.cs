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
        public Task<T> Submit<T>(Func<T> task)
        {
            return Task.Run(task);
        }

        public Task Submit(Action task)
        {
            return Task.Run(task);
        }

        public async Task<IReadOnlyCollection<T>> InvokeAll<T>(IEnumerable<Func<T>> tasks)
        {
            return await Task.WhenAll(tasks.Select(Task.Run));
        }

        public Task InvokeAll<T>(IEnumerable<Func<CancellationToken, Task>> tasks, CancellationToken cancellationToken)
        {
            return Task.WhenAll(tasks.Select(task => Task.Run(() => task(cancellationToken), cancellationToken)));
        }

        public async Task<T> InvokeAny<T>(IEnumerable<Func<T>> tasks)
        {
            var completedTask = await Task.WhenAny(tasks.Select(Task.Run));
            return await completedTask;
        }
    }
}
