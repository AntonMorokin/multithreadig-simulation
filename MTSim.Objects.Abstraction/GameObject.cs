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

        protected GameObject(long id)
        {
            Id = id;
        }

        public virtual bool TryToCapture()
        {
            return Interlocked.CompareExchange(ref _isInAction, IsInAction, IsFree) == IsFree;
        }

        public virtual void SetFree()
        {
            Interlocked.Exchange(ref _isInAction, IsFree);
        }

        public abstract void Act();

        protected void ThrowIfNotCaptured()
        {
            if (!IsCaptured)
            {
                throw new InvalidOperationException("Game object has to be captured before using it");
            }
        }
    }
}
