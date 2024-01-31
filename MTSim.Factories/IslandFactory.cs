using MTSim.Map;
using static MTSim.Configuration.GameConfig;

namespace MTSim.Factories
{
    public sealed class IslandFactory
    {
        private readonly IslandConfig _islandConfig;

        public IslandFactory(IslandConfig islandConfig)
        {
            _islandConfig = islandConfig;
        }

        public Island Create()
        {
            var island = new Island(_islandConfig.Width, _islandConfig.Height, _islandConfig.ByTypeLocationCapacity);
            return island;
        }
    }
}
