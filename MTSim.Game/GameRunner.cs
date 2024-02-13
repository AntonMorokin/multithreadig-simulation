using MTSim.Game.Interfaces;
using MTSim.Map;
using MTSim.Stats;
using MTSim.Stats.Printers;
using MTSim.Utils;
using MTSim.Utils.Java;

namespace MTSim.Game
{
    internal sealed class GameRunner : IInitializedGame
    {
        private static readonly TimeSpan CycleDuration = TimeSpan.FromMilliseconds(1000); // can be moved to config

        private readonly Island _island;
        private readonly StatsCollector _statsCollector;
        private readonly IStatsPrinter _statsPrinter;

        public GameRunner(Island island, StatsCollector statsCollector, IStatsPrinter statsPrinter)
        {
            _island = island;
            _statsCollector = statsCollector;
            _statsPrinter = statsPrinter;
        }

        public async Task<IDisposable> RunAsync(CancellationToken cancellationToken)
        {
            try
            {
                var simulationTask = ScheduledThreadPoolExecutor.ScheduleAtFixedRate(MakeSimulationCycleAsync, TimeSpan.Zero, CycleDuration, cancellationToken);
                var statsTask = ScheduledThreadPoolExecutor.ScheduleAtFixedRate(CollectStats, CycleDuration / 2, CycleDuration, cancellationToken);
                //var statsTask = Task.CompletedTask;

                await Task.WhenAll(simulationTask, statsTask);
            }
            catch (OperationCanceledException)
            {
            }

            return new CompositeDisposable(_island, _statsCollector);
        }

        private async Task MakeSimulationCycleAsync(CancellationToken cancellationToken)
        {
            _statsCollector.StartCycle();

            await ThreadPoolExecutor.InvokeAll(_island.GetObjects(), static x => x.Act(), cancellationToken);
            _island.RemoveDeadObjects();

            _statsCollector.FinishCycle();
        }

        private void CollectStats()
        {
            var snapshot = _statsCollector.GetSnapshot();
            _statsPrinter.Print(snapshot);
        }
    }
}
