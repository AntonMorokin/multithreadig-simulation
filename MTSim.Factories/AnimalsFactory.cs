using MTSim.Configuration;
using MTSim.Map;
using MTSim.Objects.Abstraction;
using MTSim.Objects.Animals.Herbivores;
using MTSim.Objects.Animals.Predators;
using MTSim.Utils;

namespace MTSim.Factories
{
    public sealed class AnimalsFactory
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

        public GameObject CreateWolf(Point coords)
        {
            var nextId = _idGenerator.GetNextId();
            var config = _animalsConfig[TypeNames.Wolf];

            return new Wolf(nextId, _island, coords, config.MaxSpeed, config.MaxSatiety, config.Weight, config.Food);
        }

        public GameObject CreateFox(Point coords)
        {
            var nextId = _idGenerator.GetNextId();
            var config = _animalsConfig[TypeNames.Fox];

            return new Fox(nextId, _island, coords, config.MaxSpeed, config.MaxSatiety, config.Weight, config.Food);
        }

        public GameObject CreateDuck(Point coords)
        {
            var nextId = _idGenerator.GetNextId();
            var config = _animalsConfig[TypeNames.Duck];

            return new Duck(nextId, _island, coords, config.MaxSpeed, config.MaxSatiety, config.Weight, config.Food);
        }

        public GameObject CreateCaterpillar(Point coords)
        {
            var nextId = _idGenerator.GetNextId();
            var config = _animalsConfig[TypeNames.Caterpillar];

            return new Caterpillar(nextId, _island, coords, config.MaxSpeed, config.MaxSatiety, config.Weight, config.Food);
        }
    }
}
