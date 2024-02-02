using MTSim.Map;

namespace MTSim.Objects.Abstraction.Interfaces
{
    public interface IPositioned
    {
        Island Island { get; }

        Point Coords { get; }
    }
}
