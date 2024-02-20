using MTSim.Configuration;
using MTSim.Map;
using MTSim.Objects.Abstraction;
using MTSim.Objects.Behaviors;
using MTSim.Objects.Factories;
using MTSim.Objects.Plants;
using MTSim.Utils;

namespace MTSim.Factories
{
    public sealed class PlantsFactory : IPlantsFactory
    {
        private readonly Dictionary<string, GameConfig.PlantConfig> _plantsConfig;
        private readonly Island _island;
        private readonly IObjectsBehavior _behavior;
        private readonly IdGenerator _idGenerator;

        public PlantsFactory(
            Dictionary<string, GameConfig.PlantConfig> config,
            Island island,
            IObjectsBehavior behavior,
            IdGenerator idGenerator)
        {
            _plantsConfig = config;
            _island = island;
            _idGenerator = idGenerator;
            _behavior = behavior;
        }

        public Plant CreateGrass(Point coords)
        {
            var nextId = _idGenerator.GetNextId();
            var config = _plantsConfig[TypeNames.Grass];

            return new Grass(nextId, _island, coords, Convert(config), _behavior);
        }

        private static PlantProps Convert(GameConfig.PlantConfig config) => new(config.GrowSpeed, config.Weight);

        public Plant Create(string typeName, Point coords)
        {
            return typeName switch
            {
                TypeNames.Grass => CreateGrass(coords),
                _ => throw new ArgumentOutOfRangeException(nameof(typeName), typeName,
                    $"Unknown plant type '{typeName}'")
            };
        }
    }
}
