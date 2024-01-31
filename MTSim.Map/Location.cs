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

        private readonly Dictionary<string, int> _byTypeCapacity;

        /// <summary>
        /// Координаты на острове
        /// </summary>
        public Point Coords { get; }

        /// <summary>
        /// Количество объектов в локации
        /// </summary>
        public int ObjectsCount => _objects.Count;

        public Location(Point coords, Dictionary<string, int> byTypeCapacity)
        {
            Coords = coords;
            _byTypeCapacity = byTypeCapacity;
        }

        public bool CanBeMovedTo(GameObject obj)
        {
            return ExecSafe(this, obj, CanBeMovedToInternal);
        }

        private static bool CanBeMovedToInternal(Location location, GameObject obj)
        {
            var typeCapacity = location._byTypeCapacity[obj.TypeName];
            if (location._objects.Count(x => x.TypeName == obj.TypeName) < typeCapacity)
            {
                return true;
            }

            return false;
        }

        // For moving
        public bool AnyOfExcept(HashSet<string> typeNames, GameObject except)
        {
            return ExecSafe(this, typeNames, except, AnyOfExceptInternal);
        }

        private static bool AnyOfExceptInternal(Location location, HashSet<string> typeNames, GameObject except)
        {
            return location._objects
                .Where(x => typeNames.Contains(x.TypeName) && !object.Equals(x, except))
                .Any();
        }

        // For eating
        public GameObject GetRandomOfExcept(HashSet<string> typeNames, GameObject except)
        {
            return ExecSafe(this, typeNames, except, GetRandomOfExceptInternal);
        }

        private static GameObject GetRandomOfExceptInternal(Location location, HashSet<string> typeNames, GameObject except)
        {
            var objects = location._objects
                .Where(x => typeNames.Contains(x.TypeName) && !object.Equals(x, except))
                .ToArray();

            if (objects.Length == 0)
            {
                throw new InvalidOperationException($"There is no other game objects of types {string.Join(", ", typeNames)}");
            }

            var i = Random.Shared.Next(objects.Length);
            return objects[i];
        }

        // For reproduction
        public T GetRandomOfExcept<T>(T except)
            where T : GameObject
        {
            return ExecSafe(this, except, GetRandomOfExceptInternal);
        }

        private static T GetRandomOfExceptInternal<T>(Location location, T except)
            where T : GameObject
        {
            // TODO would be nice to not allocate new array on the heap
            var objects = location._objects
                .OfType<T>()
                .Where(x => !object.Equals(x, except)) // reference equality
                .ToArray();

            if (objects.Length  == 0)
            {
                throw new InvalidOperationException($"There is no other game objects of type {typeof(T).Name}");
            }

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

        private void ExecSafe<T>(Location location, T arg, Action<Location, T> action)
        {
            lock (_sync)
            {
                action(location, arg);
            }
        }

        private TRes ExecSafe<TArg, TRes>(Location location, TArg arg, Func<Location, TArg, TRes> func)
        {
            lock (_sync)
            {
                return func(location, arg);
            }
        }

        private TRes ExecSafe<TArg1, TArg2, TRes>(Location location, TArg1 arg1, TArg2 arg2, Func<Location, TArg1, TArg2, TRes> func)
        {
            lock (_sync)
            {
                return func(location, arg1, arg2);
            }
        }
    }
}
