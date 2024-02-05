namespace MTSim.Map.Interfaces
{
    public interface IPositioned
    {
        Island Island { get; }

        Point Coords { get; }
    }
}
