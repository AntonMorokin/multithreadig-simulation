using MTSim.Utils;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace MTSim.Stats.Model
{
    public sealed class ByCycle : IDisposable
    {
        private readonly ReaderWriterLockSlim _sync = new();

        private readonly ConcurrentDictionary<string, Counter> _total = new(); // who acted
        private readonly ConcurrentDictionary<string, Counter> _moved = new(); // who moved
        private readonly ConcurrentDictionary<string, Counter> _eaten = new(); // who was eaten
        private readonly ConcurrentDictionary<string, Counter> _born = new(); // who was born
        private readonly ConcurrentDictionary<string, Counter> _grew = new(); // who grew
        private readonly ConcurrentDictionary<string, Counter> _dead = new(); // who got dead

        private long _startedTimestamp = -1;
        private TimeSpan _cycleDuration;

        private bool _disposed;

        private void CheckIfDisposed()
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
        }

        private void Add(ConcurrentDictionary<string, Counter> dict, string typeName)
        {
            CheckIfDisposed();

            _sync.EnterReadLock();
            try
            {
                var counter = dict.GetOrAdd(typeName, _ => new Counter());
                counter.Increment();
            }
            finally
            {
                _sync.ExitReadLock();
            }
        }

        public void StartCycle()
        {
            CheckIfDisposed();

            _sync.EnterReadLock();
            try
            {
                _startedTimestamp = Stopwatch.GetTimestamp();
            }
            finally
            {
                _sync.ExitReadLock();
            }
        }

        public void FinishCycle()
        {
            CheckIfDisposed();

            _sync.EnterReadLock();
            try
            {
                var finishedTs = Stopwatch.GetTimestamp();
                _cycleDuration = StopwatchHelper.Elapsed(_startedTimestamp, finishedTs);
            }
            finally
            {
                _sync.ExitReadLock();
            }
        }

        public void AddTotal(string typeName) => Add(_total, typeName);

        public void AddMoved(string typeName) => Add(_moved, typeName);

        public void AddEaten(string typeName) => Add(_eaten, typeName);

        public void AddBorn(string typeName) => Add(_born, typeName);

        public void AddGrew(string typeName) => Add(_grew, typeName);

        public void AddDead(string typeName) => Add(_dead, typeName);

        public Snapshot GetSnapshot()
        {
            CheckIfDisposed();

            _sync.EnterWriteLock();
            try
            {
                var snapshot = new Snapshot(
                    _cycleDuration,
                    _total.ToDictionary(x => x.Key, x => x.Value.Count),
                    _moved.ToDictionary(x => x.Key, x => x.Value.Count),
                    _eaten.ToDictionary(x => x.Key, x => x.Value.Count),
                    _born.ToDictionary(x => x.Key, x => x.Value.Count),
                    _grew.ToDictionary(x => x.Key, x => x.Value.Count),
                    _dead.ToDictionary(x => x.Key, x => x.Value.Count)
                );

                Clear();

                return snapshot;
            }
            finally
            {
                _sync.ExitWriteLock();
            }
        }

        private void Clear()
        {
            _startedTimestamp = -1;
            _cycleDuration = TimeSpan.Zero;

            _total.Clear();
            _moved.Clear();
            _eaten.Clear();
            _born.Clear();
            _grew.Clear();
            _dead.Clear();
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            _sync.Dispose();
        }

        public sealed class Snapshot
        {
            public readonly TimeSpan CycleDuration;

            public readonly IReadOnlyDictionary<string, int> Total;
            public readonly IReadOnlyDictionary<string, int> Moved;
            public readonly IReadOnlyDictionary<string, int> Eaten;
            public readonly IReadOnlyDictionary<string, int> Born;
            public readonly IReadOnlyDictionary<string, int> Grew;
            public readonly IReadOnlyDictionary<string, int> Dead;

            public Snapshot(
                TimeSpan cycleDuration,
                IReadOnlyDictionary<string, int> total,
                IReadOnlyDictionary<string, int> moved,
                IReadOnlyDictionary<string, int> eaten,
                IReadOnlyDictionary<string, int> born,
                IReadOnlyDictionary<string, int> grew,
                IReadOnlyDictionary<string, int> dead)
            {
                CycleDuration = cycleDuration;
                Total = total;
                Moved = moved;
                Eaten = eaten;
                Born = born;
                Grew = grew;
                Dead = dead;
            }
        }
    }
}
