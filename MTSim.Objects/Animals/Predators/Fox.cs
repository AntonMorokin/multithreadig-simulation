using MTSim.Map;
using MTSim.Objects.Abstraction;
using System.Collections.Generic;

namespace MTSim.Objects.Animals.Predators
{
    public class Fox : Predator
    {
        public override string TypeName => TypeNames.Fox;

        public Fox(int id, Island island, Point coords, int maxSpeed, double maxSatiety, double weight, Dictionary<string, double> food)
            : base(id, island, coords, maxSpeed, maxSatiety, weight, food)
        {
        }
    }
}
