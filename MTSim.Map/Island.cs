using MTSim.Objects.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MTSim.Map
{
    public sealed class Island : IDisposable
    {
        private readonly Location[,] _map;

        private bool _disposed;

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
            ForEachLocation(static (map, i, j, arg) =>
            {
                var coords = new Point(i, j);
                map[j, i] = new Location(coords, arg);
            }, byTypeLocationCapacity);
        }

        // TODO would be nice to create own enumerator based on ref struct
        private void ForEachLocation(Action<Location> action)
        {
            for (var j = 0; j < Height; j++)
            {
                for (var i = 0; i < Width; i++)
                {
                    action(_map[j, i]);
                }
            }
        }

        private void ForEachLocation<T>(Action<Location[,], int, int, T> action, T arg)
        {
            for (var j = 0; j < Height; j++)
            {
                for (var i = 0; i < Width; i++)
                {
                    action(_map, i, j, arg);
                }
            }
        }

        private void CheckIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(Island));
            }
        }

        public void Add(GameObject obj, Point where)
        {
            CheckIfDisposed();

            var location = Get(where);
            location.Add(obj);
        }

        public void Move(GameObject obj, Point from, Point to)
        {
            CheckIfDisposed();

            var oldLoc = Get(from);
            oldLoc.Remove(obj);

            var newLoc = Get(to);
            newLoc.Add(obj);
        }

        public bool CanBeMovedTo(GameObject obj, Point where)
        {
            CheckIfDisposed();

            var location = Get(where);
            return location.CanBeMovedTo(obj);
        }

        public bool AnyOfExcept(Point where, HashSet<string> typeNames, GameObject except)
        {
            CheckIfDisposed();

            var location = Get(where);
            return location.AnyOfExcept(typeNames, except);
        }

        public bool AnyOfExcept<T>(Point where, T except)
            where T : GameObject
        {
            CheckIfDisposed();

            var location = Get(where);
            return location.AnyOfExcept(except);
        }

        public bool TryGetRandomOfExcept(Point where, HashSet<string> typeNames, GameObject except, out GameObject? random)
        {
            CheckIfDisposed();

            var location = Get(where);
            return location.TryGetRandomOfExcept(typeNames, except, out random);
        }

        public bool TryGetRandomOfExcept<T>(Point where, T except, out T? random)
            where T : GameObject
        {
            CheckIfDisposed();

            var location = Get(where);
            return location.TryGetRandomOfExcept(except, out random);
        }

        public void RemoveDeadObjects()
        {
            CheckIfDisposed();

            ForEachLocation(static (location) => location.RemoveDeadObjects());
        }

        private Location Get(Point from)
        {
            return _map[from.Y, from.X];
        }

        public void Dispose()
        {
            _disposed = true;

            ForEachLocation(static (location) => location.Dispose());
        }

        private IEnumerable<Location> GetLocations()
        {
            for (var j = 0; j < Height; j++)
            {
                for (var i = 0; i < Width; i++)
                {
                    yield return _map[j, i];
                }
            }
        }

        public IEnumerable<GameObject> GetObjects()
        {
            CheckIfDisposed();

            return GetLocations().SelectMany(location => location.GetObjects());
        }
    }
}
