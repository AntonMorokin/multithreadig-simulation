namespace MTSim.Stats.Model
{
    internal sealed class Counter
    {
        private int _counter;

        public int Count => _counter;

        public void Increment()
        {
            Interlocked.Increment(ref _counter);
        }
    }
}
