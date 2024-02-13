using MTSim.Objects.Abstraction.Utils;

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

        public GameObject(long id)
        {
            Id = id;
        }

        public virtual void Act()
        {
            SafeExecutor exec;

            while (!SafeExecutor.TryToCapture(this, out exec)) { }

            using (exec)
            {
                ActInternal();
            }
        }

        public virtual bool TryToCapture()
        {
            return Interlocked.CompareExchange(ref _isInAction, IsInAction, IsFree) == IsFree;
        }

        public virtual void SetFree()
        {
            Interlocked.Exchange(ref _isInAction, IsFree);
        }

        protected abstract void ActInternal();

        protected void ThrowIfNotCaptured()
        {
            if (!IsCaptured)
            {
                throw new InvalidOperationException("Game object has to be captured before using it");
            }
        }
    }
}
