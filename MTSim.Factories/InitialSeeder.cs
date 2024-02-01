using MTSim.Map;
using MTSim.Objects.Abstraction;
using MTSim.Objects.Factories;
using static MTSim.Configuration.GameConfig;

namespace MTSim.Factories
{
    public sealed class InitialSeeder
    {
        private readonly IslandConfig _config;
        private readonly IAnimalsFactory _animalsFactory;
        private readonly IPlantsFactory _plantsFactory;

        public InitialSeeder(IslandConfig config, IAnimalsFactory animalsFactory, IPlantsFactory plantsFactory)
        {
            _config = config;
            _animalsFactory = animalsFactory;
            _plantsFactory = plantsFactory;
        }

        public void SeedObjects(Island island)
        {
            for (var j = 0; j < island.Height; j++)
            {
                for (var i = 0; i < island.Width; i++)
                {
                    var coords = new Point(i, j);

                    foreach (var (typeName, capacity) in _config.ByTypeLocationCapacity)
                    {
                        var possibility = capacity / 2;

                        for (var k = 0; k < Random.Shared.Next(possibility); k++)
                        {
                            var obj = CreateByType(typeName, coords);
                            island.Add(obj, coords);
                        }
                    }
                }
            }
        }

        private GameObject CreateByType(string typeName, Point coords)
        {
            switch (typeName)
            {
                case TypeNames.Caterpillar:
                    return _animalsFactory.CreateCaterpillar(coords);
                case TypeNames.Duck:
                    return _animalsFactory.CreateDuck(coords);
                case TypeNames.Fox:
                    return _animalsFactory.CreateFox(coords);
                case TypeNames.Wolf:
                    return _animalsFactory.CreateWolf(coords);
                case TypeNames.Grass:
                    return _plantsFactory.CreateGrass(coords);
                default:
                    throw new ArgumentOutOfRangeException(nameof(typeName), $"Unknown type name {typeName}");
            }
        }
    }
}
