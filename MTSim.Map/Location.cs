using MTSim.Objects.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MTSim.Map
{
    internal sealed class Location
    {
        private readonly LinkedList<GameObject> _objects = new();
        private readonly Dictionary<long, LinkedListNode<GameObject>> _objectsMap = new(64);

        private readonly object _sync = new();

        /// <summary>
        /// Координаты на острове
        /// </summary>
        public Point Coords { get; }

        /// <summary>
        /// Количество объектов в локации
        /// </summary>
        public int ObjectsCount => _objects.Count;

        /// <summary>
        /// Вместимость локации
        /// </summary>
        // TODO add different constrains by different types
        public int Capacity { get; }

        public Location(Point coords, int capacity)
        {
            Coords = coords;
            Capacity = capacity;
        }

        public T GetRandomOf<T>()
            where T : GameObject
        {
            return ExecSafe(this, GetRandomOfInternal<T>);
        }

        private static T GetRandomOfInternal<T>(Location location)
        {
            // TODO would be nice to not allocate new array on the heap
            var objects = location._objects.OfType<T>().ToArray();
            var i = Random.Shared.Next(objects.Length);
            return objects[i];
        }

        public void Add(GameObject obj)
        {
            ExecSafe(this, obj, AddInternal);
        }

        private static void AddInternal(Location location, GameObject obj)
        {
            var node = location._objects.AddLast(obj);
            location._objectsMap.Add(obj.Id, node);
        }

        public void Remove(GameObject obj)
        {
            ExecSafe(this, obj, RemoveInternal);
        }

        private static void RemoveInternal(Location location, GameObject obj)
        {
            var node = location._objectsMap[obj.Id];

            location._objectsMap.Remove(obj.Id);
            location._objects.Remove(node);
        }

        private T ExecSafe<T>(Location location, Func<Location, T> func)
        {
            lock (_sync)
            {
                return func(location);
            }
        }

        private void ExecSafe(Location location, GameObject gameObject, Action<Location, GameObject> action)
        {
            lock (_sync)
            {
                action(location, gameObject);
            }
        }
    }
}
