using MTSim.Stats.Model;

namespace MTSim.Stats.Formatters
{
    public interface IStatsFormatter
    {
        string Format(ByCycle.Snapshot snapshot);
    }
}