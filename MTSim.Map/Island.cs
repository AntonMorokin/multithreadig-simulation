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

        public void Add(GameObject obj, Point where)
        {
            var location = Get(where);
            location.Add(obj);
        }

        public void Move(GameObject obj, Point from, Point to)
        {
            var oldLoc = Get(from);
            oldLoc.Remove(obj);

            var newLoc = Get(to);
            newLoc.Add(obj);
        }

        public bool CanBeMovedTo(GameObject obj, Point where)
        {
            var location = Get(where);
            return location.CanBeMovedTo(obj);
        }

        public bool AnyOfExcept(Point where, HashSet<string> typeNames, GameObject except)
        {
            var location = Get(where);
            return location.AnyOfExcept(typeNames, except);
        }

        public bool AnyOfExcept<T>(Point where, T except)
            where T : GameObject
        {
            var location = Get(where);
            return location.AnyOfExcept(except);
        }

        public GameObject GetRandomOfExcept(Point where, HashSet<string> typeNames, GameObject except)
        {
            var location = Get(where);
            return location.GetRandomOfExcept(typeNames, except);
        }

        public T GetRandomOfExcept<T>(Point where, T except)
            where T : GameObject
        {
            var location = Get(where);
            return location.GetRandomOfExcept(except);
        }

        private Location Get(Point from)
        {
            // To prevent deadlocks don't try to take one lock (Location._sync) from another (Island._sync)
            lock (_sync)
            {
                return _map[from.Y, from.X];
            }
        }
    }
}
