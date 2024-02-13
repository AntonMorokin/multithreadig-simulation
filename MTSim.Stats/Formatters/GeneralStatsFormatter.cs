using MTSim.Stats.Model;
using System.Text;

namespace MTSim.Stats.Formatters
{
    public sealed class GeneralStatsFormatter : IStatsFormatter
    {
        public string Format(ByCycle.Snapshot snapshot)
        {
            /*
            Cycle duration: ###ms
            Total objects on the island: ###
                type1: ###
            Moved total: ###
                type1: ###
            Born total: ###
                type1: ###
            Grew total: ###
                type1: ###
            Eaten total: ###
                type1: ###
            Dead total: ###
                type1: ###

            */

            var sb = new StringBuilder(500);

            sb.Append("Cycle duration: ").Append(snapshot.CycleDuration.TotalMilliseconds.ToString("F3")).AppendLine("ms");

            FormatStats(sb, "Total objects on the island: ", snapshot.Total, "    ");
            FormatStats(sb, "Moved total: ", snapshot.Moved, "    ");
            FormatStats(sb, "Born total: ", snapshot.Born, "    ");
            FormatStats(sb, "Grew total: ", snapshot.Grew, "    ");
            FormatStats(sb, "Eaten total: ", snapshot.Eaten, "    ");
            FormatStats(sb, "Dead total: ", snapshot.Dead, "    ");

            return sb.ToString();
        }

        private void FormatStats(StringBuilder sb, string description, IReadOnlyDictionary<string, int> stats, string identation)
        {
            sb.Append(description).AppendLine(stats.Values.Sum().ToString());

            foreach (var (type, value) in stats)
            {
                sb.Append(identation).Append(type).Append(": ").AppendLine(value.ToString());
            }
        }
    }
}
