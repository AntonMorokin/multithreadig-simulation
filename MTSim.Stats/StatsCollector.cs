using MTSim.Objects.Abstraction;
using MTSim.Stats.Model;

namespace MTSim.Stats
{
    public sealed class StatsCollector : IDisposable
    {
        private readonly ByCycle _byCycleData = new();

        private bool _disposed;

        private void CheckIfDisposed()
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
        }

        public void StartCycle()
        {
            CheckIfDisposed();

            _byCycleData.StartCycle();
        }

        public void FinishCycle()
        {
            CheckIfDisposed();

            _byCycleData.FinishCycle();
        }

        public void AddTotalInCycle(GameObject obj)
        {
            CheckIfDisposed();

            _byCycleData.AddTotal(obj.TypeName);
        }

        public void AddMovedInCycle(GameObject obj)
        {
            CheckIfDisposed();

            _byCycleData.AddMoved(obj.TypeName);
        }

        public void AddEatenInCycle(GameObject obj)
        {
            CheckIfDisposed();

            _byCycleData.AddEaten(obj.TypeName);
        }

        public void AddBornInCycle(IReadOnlyCollection<GameObject> born)
        {
            CheckIfDisposed();

            foreach (var obj in born)
            {
                _byCycleData.AddBorn(obj.TypeName);
            }

        }

        public void AddGrewInCycle(GameObject obj)
        {
            CheckIfDisposed();

            _byCycleData.AddGrew(obj.TypeName);
        }

        public void AddDeadInCycle(GameObject obj)
        {
            CheckIfDisposed();

            _byCycleData.AddDead(obj.TypeName);
        }

        public ByCycle.Snapshot GetSnapshot()
        {
            CheckIfDisposed();

            return _byCycleData.GetSnapshot();
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            _byCycleData.Dispose();
        }
    }
}
