using MTSim.Map;
using MTSim.Objects.Abstraction;
using MTSim.Objects.Factories;
using System.Collections.Generic;
using System.Linq;

namespace MTSim.Objects.Animals.Herbivores
{
    public class Caterpillar : Herbivore
    {
        public override string TypeName => TypeNames.Caterpillar;

        private readonly IAnimalsFactory _factory;

        public Caterpillar(int id, Island island, Point coords, int maxSpeed, double maxSatiety, double weight, Dictionary<string, double> food, IAnimalsFactory factory)
            : base(id, island, coords, maxSpeed, maxSatiety, weight, food)
        {
            _factory = factory;
        }

        protected override IReadOnlyCollection<Animal> BornNewAnimals(Animal partner)
        {
            const int ChildrenCount = 1;

            return Enumerable
                .Range(0, ChildrenCount)
                .Select(x => _factory.CreateCaterpillar(Coords))
                .ToArray();
        }
    }
}
