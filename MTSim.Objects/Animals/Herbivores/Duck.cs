using MTSim.Map;
using MTSim.Objects.Abstraction;
using MTSim.Objects.Behaviors;
using MTSim.Objects.Factories;

namespace MTSim.Objects.Animals.Herbivores
{
    public class Duck : Herbivore
    {
        public override string TypeName => TypeNames.Duck;

        public override int MaxChildrenCount => 4;

        public Duck(
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