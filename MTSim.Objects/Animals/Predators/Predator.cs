using MTSim.Map;
using MTSim.Objects.Behaviors;
using MTSim.Objects.Factories;

namespace MTSim.Objects.Animals.Predators
{
    public abstract class Predator : Animal
    {
        protected Predator(
            long id,
            Island island,
            Point coords,
            AnimalProps props,
            IObjectsBehavior behavior,
            IAnimalsFactory factory)
            : base(id, island, coords, props, behavior, factory)
        {
        }
    }
}
