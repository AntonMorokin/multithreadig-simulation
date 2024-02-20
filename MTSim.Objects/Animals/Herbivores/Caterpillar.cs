using MTSim.Map;
using MTSim.Objects.Abstraction;
using MTSim.Objects.Behaviors;
using MTSim.Objects.Factories;

namespace MTSim.Objects.Animals.Herbivores
{
    public class Caterpillar : Herbivore
    {
        public override string TypeName => TypeNames.Caterpillar;

        public override int MaxChildrenCount => 6;
        
        public Caterpillar(
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
