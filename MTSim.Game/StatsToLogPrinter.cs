using Microsoft.Extensions.Logging;
using MTSim.Stats.Formatters;
using MTSim.Stats.Model;
using MTSim.Stats.Printers;

namespace MTSim.Game
{
    public sealed class StatsToLogPrinter : IStatsPrinter
    {
        private readonly IStatsFormatter _formatter;
        private readonly ILogger _logger;

        public StatsToLogPrinter(IStatsFormatter formatter, ILogger logger)
        {
            _formatter = formatter;
            _logger = logger;
        }

        public void Print(ByCycle.Snapshot snapshot)
        {
            var message = _formatter.Format(snapshot);
            _logger.LogInformation(message);
        }
    }
}
