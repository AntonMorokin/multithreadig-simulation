using MTSim.Map;
using MTSim.Objects.Abstraction;
using MTSim.Objects.Factories;
using System.Collections.Generic;

namespace MTSim.Objects.Animals.Herbivores
{
    public class Duck : Herbivore
    {
        public override string TypeName => TypeNames.Duck;

        private readonly IAnimalsFactory _factory;

        public Duck(long id, Island island, Point coords, int maxSpeed, double maxSatiety, double weight, Dictionary<string, double> food, IAnimalsFactory factory)
            : base(id, island, coords, maxSpeed, maxSatiety, weight, food)
        {
            _factory = factory;
        }

        public override IReadOnlyCollection<Animal> BornNewAnimals(Animal partner)
        {
            return BornNewAnimalsTemplate(partner, 4, _factory, static (c, f) => f.CreateDuck(c));
        }
    }
}
