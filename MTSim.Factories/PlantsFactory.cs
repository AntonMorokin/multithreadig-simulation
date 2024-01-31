using MTSim.Configuration;
using MTSim.Map;
using MTSim.Objects.Abstraction;
using MTSim.Objects.Factories;
using MTSim.Objects.Plants;
using MTSim.Utils;

namespace MTSim.Factories
{
    public sealed class PlantsFactory : IPlantsFactory
    {
        private readonly Dictionary<string, GameConfig.PlantConfig> _plantsConfig;
        private readonly Island _island;
        private readonly IdGenerator _idGenerator;

        public PlantsFactory(Dictionary<string, GameConfig.PlantConfig> config, Island island, IdGenerator idGenerator)
        {
            _plantsConfig = config;
            _island = island;
            _idGenerator = idGenerator;
        }

        public Plant CreateGrass(Point coords)
        {
            var nextId = _idGenerator.GetNextId();
            var config = _plantsConfig[TypeNames.Grass];

            return new Grass(nextId, _island, coords, config.GrowSpeed, config.Weight);
        }
    }
}
