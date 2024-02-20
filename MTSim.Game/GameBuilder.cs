using Microsoft.Extensions.Logging;
using MTSim.Configuration;
using MTSim.Factories;
using MTSim.Game.Interfaces;
using MTSim.Stats;
using MTSim.Stats.Behaviors;
using MTSim.Stats.Formatters;
using MTSim.Utils;

namespace MTSim.Game
{
    public static class GameBuilder
    {
        public static IInitializedGame InitGame(string configFilePath, string foodMatrixFilePath, ILogger logger)
        {
            var config = ConfigLoader.Load(configFilePath, foodMatrixFilePath);

            var islandFactory = new IslandFactory(config.Island);
            var island = islandFactory.Create();

            var statsCollector = new StatsCollector();
            var behavior = new StatsCollectingObjectBehavior(statsCollector);

            var idGenerator = new IdGenerator();

            var animalsFactory = new AnimalsFactory(config.Animals, island, behavior, idGenerator);
            var plantsFactory = new PlantsFactory(config.Plants, island, behavior, idGenerator);

            var seeder = new InitialSeeder(config.Island, animalsFactory, plantsFactory);
            seeder.SeedObjects(island);

            var statsFormatter = new GeneralStatsFormatter();
            var statsPrinter = new StatsToLogPrinter(statsFormatter, logger);

            return new GameRunner(island, statsCollector, statsPrinter);
        }
    }
}
