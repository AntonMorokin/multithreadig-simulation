namespace MTSim.Objects.Abstraction
{
    public ref struct SafeExecutor
    {
        private readonly GameObject _obj;
        private bool _captured;

        private SafeExecutor(GameObject obj)
        {
            _obj = obj;
        }

        public static bool TryToCapture(GameObject obj, out SafeExecutor executor)
        {
            const int MaxAttempts = 5;

            executor = new SafeExecutor(obj);
            var sw = new SpinWait();

            executor._captured = executor._obj.TryToCapture();

            for (int i = 1; i < MaxAttempts || !executor._captured; i++)
            {
                sw.SpinOnce();
                executor._captured = executor._obj.TryToCapture();
            }

            return executor._captured;
        }

        public void Dispose()
        {
            if (_captured)
            {
                _obj.SetFree();
            }
        }
    }
}
