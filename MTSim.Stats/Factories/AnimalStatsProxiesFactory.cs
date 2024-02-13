using MTSim.Map;
using MTSim.Objects.Animals;
using MTSim.Objects.Factories;
using MTSim.Stats.Proxies;

namespace MTSim.Stats.Factories
{
    public sealed class AnimalStatsProxiesFactory : IAnimalsFactory
    {
        private readonly StatsCollector _collector;
        private readonly IAnimalsFactory _real;

        public AnimalStatsProxiesFactory(StatsCollector collector, IAnimalsFactory real)
        {
            _collector = collector;
            _real = real;
        }

        private Animal CreateProxy(Animal animal)
        {
            return new AnimalStatsProxy(_collector, animal);
        }

        public Animal CreateCaterpillar(Point coords)
        {
            var animal = _real.CreateCaterpillar(coords);
            return CreateProxy(animal);
        }

        public Animal CreateDuck(Point coords)
        {
            var animal = _real.CreateDuck(coords);
            return CreateProxy(animal);
        }

        public Animal CreateFox(Point coords)
        {
            var animal = _real.CreateFox(coords);
            return CreateProxy(animal);
        }

        public Animal CreateWolf(Point coords)
        {
            var animal = _real.CreateWolf(coords);
            return CreateProxy(animal);
        }
    }
}
