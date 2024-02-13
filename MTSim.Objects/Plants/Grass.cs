using MTSim.Map;
using MTSim.Objects.Abstraction;

namespace MTSim.Objects.Plants
{
    public class Grass : Plant
    {
        public override string TypeName => TypeNames.Grass;

        public Grass(long id, Island island, Point coords, double growSpeed, double weight)
            : base(id, island, coords, growSpeed, weight)
        {
        }
    }
}
