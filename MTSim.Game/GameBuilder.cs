using MTSim.Configuration;
using MTSim.Factories;
using MTSim.Game.Interfaces;
using MTSim.Utils;

namespace MTSim.Game
{
    public sealed class GameBuilder
    {
        public static IInitializedGame InitGame(string configFilePath, string foodMatrixFilePath)
        {
            var config = ConfigLoader.Load(configFilePath, foodMatrixFilePath);

            var islandFactory = new IslandFactory(config.Island);
            var island = islandFactory.Create();

            var idGenerator = new IdGenerator();

            var animalsFactory = new AnimalsFactory(config.Animals, island, idGenerator);
            var plantsFactory = new PlantsFactory(config.Plants, island, idGenerator);

            var seeder = new InitialSeeder(config.Island, animalsFactory, plantsFactory);
            seeder.SeedObjects(island);

            return new GameRunner(island);
        }
    }
}
