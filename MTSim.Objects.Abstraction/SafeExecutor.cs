namespace MTSim.Objects.Abstraction
{
    public readonly ref struct SafeExecutor
    {
        private readonly GameObject _obj;

        private SafeExecutor(GameObject obj)
        {
            _obj = obj;
        }

        public static bool TryUse(GameObject obj, out SafeExecutor executor)
        {
            executor = new SafeExecutor(obj);
            return executor._obj.TryToCapture();
        }

        public void Dispose()
        {
            _obj.SetFree();
        }
    }
}
