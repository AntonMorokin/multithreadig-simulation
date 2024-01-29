using System.Collections.Generic;

namespace MTSim.Configuration
{
    public sealed class GameConfig
    {
        public IslandConfig Island { get; set; }

        public Dictionary<string, AnimalConfig> Animals { get; set; } = new();

        public Dictionary<string, PlantConfig> Plants { get; set; } = new();

        public sealed class IslandConfig
        {
            public int Width { get; set; }

            public int Height { get; set; }
        }

        public sealed class AnimalConfig
        {
            public double Weight { get; set; }

            public int MaxSpeed { get; set; }

            public double MaxSatiety { get; set; }

            public Dictionary<string, double> Food { get; set; } = new();

            public int MaxCountOnLocation { get; set; }
        }

        public sealed class PlantConfig
        {
            public double Weight { get; set; }

            public double GrowSpeed { get; set; }
        }
    }
}
