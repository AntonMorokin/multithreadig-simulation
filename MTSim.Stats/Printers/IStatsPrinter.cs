using MTSim.Stats.Model;

namespace MTSim.Stats.Printers
{
    public interface IStatsPrinter
    {
        void Print(ByCycle.Snapshot snapshot);
    }
}
