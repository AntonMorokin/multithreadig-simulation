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

        public GameObject(int id)
        {
            Id = id;
        }

        protected abstract void ActInternal();

        public void Act()
        {
            if (!SafeExecutor.TryToCapture(this, out var exec))
            {
                return;
            }

            using (exec)
            {
                ActInternal();
            }

            //try
            //{
            //    _isInAction = IsInAction;
            //    ActInternal();
            //}
            //finally
            //{
            //    _isInAction = IsFree;
            //}
        }

        public bool TryToCapture()
        {
            return Interlocked.CompareExchange(ref _isInAction, IsInAction, IsFree) == IsFree;
        }

        public void SetFree()
        {
            Interlocked.Exchange(ref _isInAction, IsFree);
        }

        protected void ThrowIfNotCaptured()
        {
            if (!IsCaptured)
            {
                throw new InvalidOperationException("Game object has to be captured before using it");
            }
        }
    }
}
