using MTSim.Map;
using MTSim.Objects.Abstraction;
using MTSim.Objects.Behaviors;
using MTSim.Objects.Factories;

namespace MTSim.Objects.Animals.Predators
{
    public class Fox : Predator
    {
        public override string TypeName => TypeNames.Fox;

        public override int MaxChildrenCount => 3;

        public Fox(
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
