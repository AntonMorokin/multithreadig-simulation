using MTSim.Map;
using MTSim.Objects.Animals;

namespace MTSim.Objects.Factories
{
    public interface IAnimalsFactory
    {
        Animal CreateCaterpillar(Point coords);
        Animal CreateDuck(Point coords);
        Animal CreateFox(Point coords);
        Animal CreateWolf(Point coords);
    }
}