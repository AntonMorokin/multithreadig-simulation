using System;

namespace MTSim.Utils
{
    public sealed class CompositeDisposable : IDisposable
    {
        private readonly IDisposable[] _disposable;
        private bool _disposed;

        public CompositeDisposable(params IDisposable[] disposable)
        {
            _disposable = disposable;
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            foreach (var disposable in _disposable)
            {
                disposable.Dispose();
            }
        }
    }
}
