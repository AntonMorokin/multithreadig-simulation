using MTSim.Utils;
using System.Diagnostics;

namespace MTSim.Objects.Abstraction.Utils
{
    public readonly ref struct SafeExecutor
    {
        private static double TimeoutSeconds = TimeSpan.FromMicroseconds(10).TotalSeconds;

        private readonly GameObject _obj;
        private readonly bool _captured;

        private SafeExecutor(GameObject obj, bool captured)
        {
            _obj = obj;
            _captured = captured;
        }

        public static bool TryToCapture(GameObject obj, out SafeExecutor executor)
        {
            var sw = new SpinWait();

            var started = Stopwatch.GetTimestamp();

            var captured = obj.TryToCapture();

            while (!(captured || TimeIsOver(started, Stopwatch.GetTimestamp())))
            {
                sw.SpinOnce();
                captured = obj.TryToCapture();
            }

            executor = new SafeExecutor(obj, captured);
            return captured;
        }

        private static bool TimeIsOver(long started, long finished)
        {
            var elapsedSeconds = StopwatchHelper.ElapsedSeconds(started, finished);
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
