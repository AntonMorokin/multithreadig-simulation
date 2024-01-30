namespace MTSim.Objects.Abstraction
{
    public abstract class GameObject
    {
        private const int IsInAction = 1;
        private const int IsFree = 0;

        private int _isInAction;

        public long Id { get; }

        public abstract string TypeName { get; }

        public bool IsCaptured => _isInAction == IsInAction;

        public GameObject(int id)
        {
            Id = id;
        }

        protected abstract void ActInternal();

        public void Act()
        {
            if (!TryToCapture())
            {
                // already captured by another object
                return;
            }

            try
            {
                ActInternal();
            }
            finally
            {
                SetFree();
            }
        }

        public bool TryToCapture()
        {
            return Interlocked.CompareExchange(ref _isInAction, IsInAction, IsFree) == IsFree;
        }

        public void SetFree()
        {
            Interlocked.Exchange(ref _isInAction, IsFree);
        }
    }
}
