using MTSim.Map;
using MTSim.Objects.Plants;

namespace MTSim.Objects.Factories
{
    public interface IPlantsFactory
    {
        Plant CreateGrass(Point coords);

        Plant Create(string typeName, Point coords);
    }
}