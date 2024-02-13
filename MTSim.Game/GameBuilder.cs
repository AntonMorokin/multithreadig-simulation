using Microsoft.Extensions.Logging;
using MTSim.Configuration;
using MTSim.Factories;
using MTSim.Game.Interfaces;
using MTSim.Stats;
using MTSim.Stats.Factories;
using MTSim.Stats.Formatters;
using MTSim.Utils;

namespace MTSim.Game
{
    public sealed class GameBuilder
    {
        public static IInitializedGame InitGame(string configFilePath, string foodMatrixFilePath, ILogger logger)
        {
            var config = ConfigLoader.Load(configFilePath, foodMatrixFilePath);

            var islandFactory = new IslandFactory(config.Island);
            var island = islandFactory.Create();

            var idGenerator = new IdGenerator();

            var animalsFactory = new AnimalsFactory(config.Animals, island, idGenerator);
            var plantsFactory = new PlantsFactory(config.Plants, island, idGenerator);

            var statsCollector = new StatsCollector();

            var animalWithStatsFactory = new AnimalStatsProxiesFactory(statsCollector, animalsFactory);
            var plantWithStatsFactory = new PlantStatsProxiesFactory(statsCollector, plantsFactory);

            var seeder = new InitialSeeder(config.Island, animalWithStatsFactory, plantWithStatsFactory);
            seeder.SeedObjects(island);

            var statsFormatter = new GeneralStatsFormatter();
            var statsPrinter = new StatsToLogPrinter(statsFormatter, logger);

            return new GameRunner(island, statsCollector, statsPrinter);
        }
    }
}
