using MTSim.Map;
using MTSim.Objects.Factories;
using MTSim.Objects.Plants;
using MTSim.Stats.Proxies;

namespace MTSim.Stats.Factories
{
    public sealed class PlantStatsProxiesFactory : IPlantsFactory
    {
        private readonly StatsCollector _collector;
        private readonly IPlantsFactory _real;

        public PlantStatsProxiesFactory(StatsCollector collector, IPlantsFactory real)
        {
            _collector = collector;
            _real = real;
        }

        private Plant CreateProxy(Plant plant)
        {
            return new PlantStatsProxy(_collector, plant);
        }

        public Plant CreateGrass(Point coords)
        {
            var plant = _real.CreateGrass(coords);
            return CreateProxy(plant);
        }
    }
}
