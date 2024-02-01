using MTSim.Map;
using System.Diagnostics;

namespace MTSim.Game
{
    internal sealed class GameRunner : IInitializedGame
    {
        private static TimeSpan SimulationCycleDuration = TimeSpan.FromMilliseconds(100); // can be moved to config

        private Island _island;

        public GameRunner(Island island)
        {
            _island = island;
        }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            try
            {
                var sw = new Stopwatch();

                while (!cancellationToken.IsCancellationRequested)
                {
                    sw.Restart();

                    await MakeCycleAsync(cancellationToken);

                    sw.Stop();
                    await Task.Delay(SimulationCycleDuration - sw.Elapsed, cancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
            }
        }

        private async Task MakeCycleAsync(CancellationToken cancellationToken)
        {
            // run three scheduled task: plants grow, animal acting and statistics collection

            throw new NotImplementedException();
        }
    }
}
