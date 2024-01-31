using MTSim.Objects.Abstraction;
using System.Collections.Generic;

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

        public Island(int width, int height, Dictionary<string, int> byTypeLocationCapacity)
        {
            _map = new Location[height, width];

            Width = width;
            Height = height;

            Init(byTypeLocationCapacity);
        }

        private void Init(Dictionary<string, int> byTypeLocationCapacity)
        {
            for (var j = 0; j < Height; j++)
            {
                for (var i = 0; i < Width; i++)
                {
                    var coords = new Point(i, j);
                    _map[j, i] = new Location(coords, byTypeLocationCapacity);
                }
            }
        }

        public void Add(GameObject obj, Point coords)
        {
            var location = Get(coords);
            location.Add(obj);
        }

        public void Move(GameObject obj, Point oldLocation, Point newLocation)
        {
            var oldLoc = Get(oldLocation);
            oldLoc.Remove(obj);

            var newLoc = Get(newLocation);
            newLoc.Add(obj);
        }

        public bool AnyOfExcept(Point coords, HashSet<string> typeNames, GameObject except)
        {
            var location = Get(coords);
            return location.AnyOfExcept(typeNames, except);
        }

        public GameObject GetRandomOfExcept(Point coords, HashSet<string> typeNames, GameObject except)
        {
            var location = Get(coords);
            return location.GetRandomOfExcept(typeNames, except);
        }

        public T GetRandomOfExcept<T>(Point coords, T except)
            where T : GameObject
        {
            var location = Get(coords);
            return location.GetRandomOfExcept(except);
        }

        private Location Get(Point coords)
        {
            // To prevent deadlocks don't try to take one lock (Location._sync) from another (Island._sync)
            lock (_sync)
            {
                return _map[coords.Y, coords.X];
            }
        }
    }
}
