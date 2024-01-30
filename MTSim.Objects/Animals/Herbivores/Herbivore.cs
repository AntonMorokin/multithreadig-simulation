using MTSim.Map;
using System.Collections.Generic;

namespace MTSim.Objects.Animals.Herbivores
{
    public abstract class Herbivore : Animal
    {
        protected Herbivore(int id, Island island, Point coords, int maxSpeed, double maxSatiety, double weight, Dictionary<string, double> food)
            : base(id, island, coords, maxSpeed, maxSatiety, weight, food)
        {
        }
    }
}
