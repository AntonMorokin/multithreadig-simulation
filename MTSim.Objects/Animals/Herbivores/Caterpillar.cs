using MTSim.Map;
using MTSim.Objects.Abstraction;
using System.Collections.Generic;

namespace MTSim.Objects.Animals.Herbivores
{
    public class Caterpillar : Herbivore
    {
        public override string TypeName => TypeNames.Caterpillar;

        public Caterpillar(int id, Island island, Point coords, int maxSpeed, double maxSatiety, double weight, Dictionary<string, double> food)
            : base(id, island, coords, maxSpeed, maxSatiety, weight, food)
        {
        }
    }
}
