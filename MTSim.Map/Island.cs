using MTSim.Objects.Abstraction;

namespace MTSim.Map
{
    public sealed class Island
    {
        private readonly object _sync = new();
        private readonly Location[,] _map;

        public int Width { get; }

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

        public Location Get(in Point coords)
        {
            return _map[coords.Y, coords.X];
        }

        public void Move(GameObject obj, in Point oldLocation, in Point newLocation)
        {
            Location oldLoc;
            lock (_sync)
            {
                oldLoc = _map[oldLocation.Y, oldLocation.X];
            }

            // To prevent deadlocks don't try to take one lock (Location._sync) from another (Island._sync)
            oldLoc.Remove(obj);

            Location newLoc;
            lock (_sync)
            {
                newLoc = _map[newLocation.Y, newLocation.X];
            }

            newLoc.Add(obj);
        }
    }
}
