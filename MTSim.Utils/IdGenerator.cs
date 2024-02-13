using System.Threading;

namespace MTSim.Utils
{
    public sealed class IdGenerator
    {
        private long _id = -1;

        public long GetNextId() => Interlocked.Increment(ref _id);
    }
}
