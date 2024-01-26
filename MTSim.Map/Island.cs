using MTSim.Objects.Abstraction;

namespace MTSim.Map
{
    public sealed class Island
    {
        private readonly object _sync = new();
        private readonly Location[,] _map;

        /// <summary>
        /// Длина острова
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Ширина острова
        /// </summary>
        public int Height { get; }

        public Island(int width, int height)
        {
            _map = new Location[height, width];

            Width = width;
            Height = height;

            Init();
        }

        private void Init()
        {
            for (var j = 0; j < Height; j++)
            {
                for (var i = 0; i < Width; i++)
                {
                    var coords = new Point(i, j);
                    // TODO load capacity from config
                    _map[j, i] = new Location(coords, 100);
                }
            }
        }

        public void Add(GameObject obj, in Point coords)
        {
            var location = Get(coords);
            location.Add(obj);
        }

        public void Move(GameObject obj, in Point oldLocation, in Point newLocation)
        {
            var oldLoc = Get(oldLocation);
            oldLoc.Remove(obj);

            var newLoc = Get(newLocation);
            newLoc.Add(obj);
        }

        public T GetRandomOf<T>(in Point coords)
            where T : GameObject
        {
            var location = Get(coords);
            return location.GetRandomOf<T>();
        }

        private Location Get(in Point coords)
        {
            // To prevent deadlocks don't try to take one lock (Location._sync) from another (Island._sync)
            lock (_sync)
            {
                return _map[coords.Y, coords.X];
            }
        }
    }
}
