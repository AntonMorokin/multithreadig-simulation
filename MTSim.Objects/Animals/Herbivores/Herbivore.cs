using MTSim.Map;
using MTSim.Objects.Behaviors;
using MTSim.Objects.Factories;

namespace MTSim.Objects.Animals.Herbivores
{
    public abstract class Herbivore : Animal
    {
        protected Herbivore(
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
