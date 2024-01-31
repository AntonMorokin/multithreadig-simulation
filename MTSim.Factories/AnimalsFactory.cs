using MTSim.Configuration;
using MTSim.Map;
using MTSim.Objects.Abstraction;
using MTSim.Objects.Animals;
using MTSim.Objects.Animals.Herbivores;
using MTSim.Objects.Animals.Predators;
using MTSim.Objects.Factories;
using MTSim.Utils;

namespace MTSim.Factories
{
    public sealed class AnimalsFactory : IAnimalsFactory
    {
        private readonly Dictionary<string, GameConfig.AnimalConfig> _animalsConfig;
        private readonly Island _island;
        private readonly IdGenerator _idGenerator;

        public AnimalsFactory(Dictionary<string, GameConfig.AnimalConfig> config, Island island, IdGenerator idGenerator)
        {
            _animalsConfig = config;
            _island = island;
            _idGenerator = idGenerator;
        }

        public Animal CreateWolf(Point coords)
        {
            var nextId = _idGenerator.GetNextId();
            var config = _animalsConfig[TypeNames.Wolf];

            return new Wolf(nextId, _island, coords, config.MaxSpeed, config.MaxSatiety, config.Weight, config.Food, this);
        }

        public Animal CreateFox(Point coords)
        {
            var nextId = _idGenerator.GetNextId();
            var config = _animalsConfig[TypeNames.Fox];

            return new Fox(nextId, _island, coords, config.MaxSpeed, config.MaxSatiety, config.Weight, config.Food, this);
        }

        public Animal CreateDuck(Point coords)
        {
            var nextId = _idGenerator.GetNextId();
            var config = _animalsConfig[TypeNames.Duck];

            return new Duck(nextId, _island, coords, config.MaxSpeed, config.MaxSatiety, config.Weight, config.Food, this);
        }

        public Animal CreateCaterpillar(Point coords)
        {
            var nextId = _idGenerator.GetNextId();
            var config = _animalsConfig[TypeNames.Caterpillar];

            return new Caterpillar(nextId, _island, coords, config.MaxSpeed, config.MaxSatiety, config.Weight, config.Food, this);
        }
    }
}
