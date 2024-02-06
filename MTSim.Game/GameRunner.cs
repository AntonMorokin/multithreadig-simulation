using MTSim.Game.Interfaces;
using MTSim.Map;
using MTSim.Utils.Java;

namespace MTSim.Game
{
    internal sealed class GameRunner : IInitializedGame
    {
        private static readonly TimeSpan CycleDuration = TimeSpan.FromMilliseconds(500); // can be moved to config

        private readonly Island _island;

        public GameRunner(Island island)
        {
            _island = island;
        }

        public async Task<IDisposable> RunAsync(CancellationToken cancellationToken)
        {
            try
            {
                var simulationTask = ScheduledThreadPoolExecutor.ScheduleAtFixedRate(MakeSimulationCycleAsync, TimeSpan.Zero, CycleDuration, cancellationToken);
                var statsTask = ScheduledThreadPoolExecutor.ScheduleAtFixedRate(CollectStatsAsync, CycleDuration / 2, CycleDuration, cancellationToken);

                await Task.WhenAll(simulationTask, statsTask);
            }
            catch (OperationCanceledException)
            {
            }

            return _island;
        }

        private async Task MakeSimulationCycleAsync(CancellationToken cancellationToken)
        {
#if DEBUG
            Console.WriteLine($"{DateTime.Now:O}: New cycle! Objects count: {_island.GetObjects().Count()}");
#endif

            await ThreadPoolExecutor.InvokeAll(_island.GetObjects(), static x => x.Act(), cancellationToken);

            _island.RemoveDeadObjects();
        }

        private Task CollectStatsAsync(CancellationToken cancellationToken)
        {
            // TODO to be implemented
            return Task.CompletedTask;
        }
    }
}
