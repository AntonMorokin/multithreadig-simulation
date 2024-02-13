using MTSim.Map;
using MTSim.Objects.Abstraction;
using MTSim.Objects.Factories;
using System.Collections.Generic;

namespace MTSim.Objects.Animals.Predators
{
    public class Fox : Predator
    {
        public override string TypeName => TypeNames.Fox;

        private readonly IAnimalsFactory _factory;

        public Fox(long id, Island island, Point coords, int maxSpeed, double maxSatiety, double weight, Dictionary<string, double> food, IAnimalsFactory factory)
            : base(id, island, coords, maxSpeed, maxSatiety, weight, food)
        {
            _factory = factory;
        }

        public override IReadOnlyCollection<Animal> BornNewAnimals(Animal partner)
        {
            return BornNewAnimalsTemplate(partner, 3, _factory, static (c, f) => f.CreateFox(c));
        }
    }
}
