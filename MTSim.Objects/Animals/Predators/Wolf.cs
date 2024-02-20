using MTSim.Map;
using MTSim.Objects.Abstraction;
using MTSim.Objects.Behaviors;
using MTSim.Objects.Factories;

namespace MTSim.Objects.Animals.Predators
{
    public class Wolf : Predator
    {
        public override string TypeName => TypeNames.Wolf;

        public override int MaxChildrenCount => 3;

        public Wolf(
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
