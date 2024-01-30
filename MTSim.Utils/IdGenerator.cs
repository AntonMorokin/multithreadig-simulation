using System.Threading;

namespace MTSim.Utils
{
    public sealed class IdGenerator
    {
        private int _id = -1;

        public int GetNextId() => Interlocked.Increment(ref _id);
    }
}
