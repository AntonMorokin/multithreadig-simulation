using MTSim.Utils;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace MTSim.Configuration
{
    public sealed class ConfigLoader
    {
        private static JsonSerializerOptions Options = new(JsonSerializerDefaults.Web);

        public static GameConfig Load(string configFilePath, string foodMatrixFilePath)
        {
            var gameConfigFile = File.ReadAllText(configFilePath);
            var gameConfig = JsonSerializer.Deserialize<GameConfig>(gameConfigFile, Options)
                ?? throw new System.InvalidOperationException($"Unable to deserialize game config from '{configFilePath}'");

            var csvReader = new CsvReader();
            var foodMatrixFile = csvReader.ReadAll(foodMatrixFilePath);

            Merge(gameConfig, foodMatrixFile);

            return gameConfig;
        }

        private static void Merge(GameConfig gameConfig, IReadOnlyCollection<string[]> foodMatrixFile)
        {
            var header = foodMatrixFile.First()[1..];

            // lines are predators and columns are victims
            // first line is header
            foreach (var predatorFood in foodMatrixFile.Skip(1))
            {
                var predatorTypeName = predatorFood[0];
                var predator = gameConfig.Animals[predatorTypeName];

                predator.Food = predatorFood
                    .Skip(1)
                    .Zip(header)
                    .ToDictionary(x => x.Second /*victim type name*/, x => double.Parse(x.First) / 100d /*possibility to catch victim*/);
            }
        }
    }
}
