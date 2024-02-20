using MTSim.Configuration;
using MTSim.Map;
using MTSim.Objects.Abstraction;
using MTSim.Objects.Animals;
using MTSim.Objects.Animals.Herbivores;
using MTSim.Objects.Animals.Predators;
using MTSim.Objects.Behaviors;
using MTSim.Objects.Factories;
using MTSim.Utils;

namespace MTSim.Factories
{
    public sealed class AnimalsFactory : IAnimalsFactory
    {
        private readonly Dictionary<string, GameConfig.AnimalConfig> _animalsConfig;
        private readonly Island _island;
        private readonly IObjectsBehavior _behavior;
        private readonly IdGenerator _idGenerator;

        public AnimalsFactory(
            Dictionary<string, GameConfig.AnimalConfig> config,
            Island island,
            IObjectsBehavior behavior,
            IdGenerator idGenerator)
        {
            _animalsConfig = config;
            _island = island;
            _idGenerator = idGenerator;
            _behavior = behavior;
        }

        public Animal CreateWolf(Point coords)
        {
            var nextId = _idGenerator.GetNextId();
            var config = _animalsConfig[TypeNames.Wolf];

            return new Wolf(nextId, _island, coords, Convert(config), _behavior, this);
        }

        private static AnimalProps Convert(GameConfig.AnimalConfig config)
            => new(config.MaxSpeed, config.MaxSatiety, config.Weight, config.Food);

        public Animal CreateFox(Point coords)
        {
            var nextId = _idGenerator.GetNextId();
            var config = _animalsConfig[TypeNames.Fox];

            return new Fox(nextId, _island, coords, Convert(config), _behavior, this);
        }

        public Animal CreateDuck(Point coords)
        {
            var nextId = _idGenerator.GetNextId();
            var config = _animalsConfig[TypeNames.Duck];

            return new Duck(nextId, _island, coords, Convert(config), _behavior, this);
        }

        public Animal CreateCaterpillar(Point coords)
        {
            var nextId = _idGenerator.GetNextId();
            var config = _animalsConfig[TypeNames.Caterpillar];

            return new Caterpillar(nextId, _island, coords, Convert(config), _behavior, this);
        }

        public Animal Create(string typeName, Point coords)
        {
            return typeName switch
            {
                TypeNames.Caterpillar => CreateCaterpillar(coords),
                TypeNames.Duck => CreateDuck(coords),
                TypeNames.Fox => CreateFox(coords),
                TypeNames.Wolf => CreateWolf(coords),
                _ => throw new ArgumentOutOfRangeException(nameof(typeName), typeName, $"Unknown animal type '{typeName}'")
            };
        }
    }
}
