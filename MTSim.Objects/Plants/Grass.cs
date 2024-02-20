using MTSim.Map;
using MTSim.Objects.Abstraction;
using MTSim.Objects.Behaviors;

namespace MTSim.Objects.Plants
{
    public class Grass : Plant
    {
        public override string TypeName => TypeNames.Grass;

        public Grass(long id, Island island, Point coords, PlantProps props, IObjectsBehavior behavior)
            : base(id, island, coords, props, behavior)
        {
        }
    }
}
