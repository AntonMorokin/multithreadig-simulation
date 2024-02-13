using MTSim.Map;
using System.Collections.Generic;

namespace MTSim.Objects.Animals.Predators
{
    public abstract class Predator : Animal
    {
        protected Predator(long id, Island island, Point coords, int maxSpeed, double maxSatiety, double weight, Dictionary<string, double> food)
            : base(id, island, coords, maxSpeed, maxSatiety, weight, food)
        {
        }
    }
}
