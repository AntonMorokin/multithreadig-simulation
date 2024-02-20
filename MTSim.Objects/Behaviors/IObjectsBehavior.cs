using MTSim.Objects.Animals;
using MTSim.Objects.Plants;

namespace MTSim.Objects.Behaviors;

public interface IObjectsBehavior
{
    void Act(Animal animal);
    void Act(Plant plant);
}