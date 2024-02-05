using System.Diagnostics;

namespace MTSim.Objects.Abstraction.Utils
{
    public ref struct SafeExecutor
    {
        private static TimeSpan Timeout = TimeSpan.FromMicroseconds(100); // 100mks
        private static double TimeoutSeconds = Timeout.TotalSeconds;

        private readonly GameObject _obj;
        private bool _captured;

        private SafeExecutor(GameObject obj)
        {
            _obj = obj;
        }

        public static bool TryToCapture(GameObject obj, out SafeExecutor executor)
        {
            executor = new SafeExecutor(obj);
            var sw = new SpinWait();

            var started = Stopwatch.GetTimestamp();

            executor._captured = executor._obj.TryToCapture();

            while (!TimeIsOver(started, Stopwatch.GetTimestamp()))
            {
                sw.SpinOnce();
                executor._captured = executor._obj.TryToCapture();
            }

            return executor._captured;
        }

        private static bool TimeIsOver(long started, long finished)
        {
            var elapsedSeconds = (double)(finished - started) / Stopwatch.Frequency;
            return elapsedSeconds > TimeoutSeconds;
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
