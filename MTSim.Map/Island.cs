﻿using MTSim.Objects.Abstraction;
using System;
using System.Collections.Generic;

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
            ForEachLocation(static (map, j, i, arg) =>
            {
                var coords = new Point(i, j);
                map[j, i] = new Location(coords, arg);
            }, byTypeLocationCapacity);
        }

        // TODO would be nice to create own enumerator based on ref struct
        private void ForEachLocation(Action<Location[,], int, int> action)
        {
            for (var j = 0; j < Height; j++)
            {
                for (var i = 0; i < Width; i++)
                {
                    action(_map, i, j);
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

        public GameObject GetRandomOfExcept(Point where, HashSet<string> typeNames, GameObject except)
        {
            CheckIfDisposed();

            var location = Get(where);
            return location.GetRandomOfExcept(typeNames, except);
        }

        public T GetRandomOfExcept<T>(Point where, T except)
            where T : GameObject
        {
            CheckIfDisposed();

            var location = Get(where);
            return location.GetRandomOfExcept(except);
        }

        public void RemoveDeadObjects()
        {
            CheckIfDisposed();

            ForEachLocation(static (map, j, i) => map[j, i].RemoveDeadObjects());
        }

        private Location Get(Point from)
        {
            return _map[from.Y, from.X];
        }

        public void Dispose()
        {
            _disposed = true;

            ForEachLocation(static (map, j, i) => map[j, i].Dispose());
        }
    }
}
