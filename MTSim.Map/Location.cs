using MTSim.Objects.Abstraction;
using MTSim.Objects.Abstraction.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MTSim.Map
{
    internal sealed class Location : IDisposable
    {
        private readonly LinkedList<GameObject> _objects = new();
        private readonly Dictionary<long, LinkedListNode<GameObject>> _objectsMap = new(64);

        private readonly ReaderWriterLockSlim _sync = new(LockRecursionPolicy.SupportsRecursion);

        private readonly Dictionary<string, int> _byTypeCapacity;

        private bool _disposed;

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

        private void CheckIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(Island));
            }
        }

        public bool CanBeMovedTo(GameObject obj)
        {
            CheckIfDisposed();

            return ExecSafeReading(this, obj, CanBeMovedToInternal);
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

        public bool AnyOfExcept(IReadOnlySet<string> typeNames, GameObject except)
        {
            CheckIfDisposed();

            return ExecSafeReading(this, typeNames, except, AnyOfExceptInternal);
        }

        private static bool AnyOfExceptInternal(Location location, IReadOnlySet<string> typeNames, GameObject except)
        {
            return location._objects
                .Any(x => typeNames.Contains(x.TypeName) && x.Id != except.Id);
        }

        public bool AnyOfExcept<T>(T except)
            where T : GameObject
        {
            CheckIfDisposed();

            return ExecSafeReading(this, except, AnyOfExceptInternal);
        }

        private static bool AnyOfExceptInternal<T>(Location location, T except)
            where T : GameObject
        {
            return location._objects
                .Any(x => x.TypeName == except.TypeName && x.Id != except.Id);
        }

        public bool TryGetRandomOfExcept(IReadOnlySet<string> typeNames, GameObject except, out GameObject? random)
        {
            CheckIfDisposed();

            var (found, result) = ExecSafeReading(this, typeNames, except, GetRandomOfExceptInternal);

            random = result;
            return found;
        }

        private static (bool found, GameObject? obj) GetRandomOfExceptInternal(Location location, IReadOnlySet<string> typeNames, GameObject except)
        {
            var objects = location._objects
                .Where(x => typeNames.Contains(x.TypeName) && x.Id != except.Id)
                .ToArray();

            if (objects.Length == 0)
            {
                return (false, null);
            }

            var i = Random.Shared.Next(objects.Length);
            return (true, objects[i]);
        }

        public bool TryGetRandomOfExcept<T>(T except, out T? random)
            where T : GameObject
        {
            CheckIfDisposed();

            var (found, result) = ExecSafeReading(this, except, GetRandomOfExceptInternal);

            random = result;
            return found;
        }

        private static (bool found, T? obj) GetRandomOfExceptInternal<T>(Location location, T except)
            where T : GameObject
        {
            // TODO would be nice to not allocate new array on the heap
            var objects = location._objects
                .Where(x => x.TypeName == except.TypeName && x.Id != except.Id) // reference equality
                .ToArray();

            if (objects.Length == 0)
            {
                return (false, null);
            }

            var i = Random.Shared.Next(objects.Length);
            return (true, (T)objects[i]);
        }

        public void Add(GameObject obj)
        {
            CheckIfDisposed();

            ExecSafeWriting(this, obj, AddInternal);
        }

        private static void AddInternal(Location location, GameObject obj)
        {
            var node = location._objects.AddLast(obj);
            location._objectsMap.Add(obj.Id, node);
        }

        public void Remove(GameObject obj)
        {
            CheckIfDisposed();

            ExecSafeWriting(this, obj, RemoveInternal);
        }

        private static void RemoveInternal(Location location, GameObject obj)
        {
            var node = location._objectsMap[obj.Id];

            location._objectsMap.Remove(obj.Id);
            location._objects.Remove(node);
        }

        public void RemoveDeadObjects()
        {
            CheckIfDisposed();

            ExecSafeWriting(this, RemoveDeadObjectsInternal);
        }

        private static void RemoveDeadObjectsInternal(Location location)
        {
            var dead = location._objects
                .Where(x => x is IAlive alive && alive.IsDead)
                .ToArray();

            foreach (var obj in dead)
            {
                RemoveInternal(location, obj);
            }
        }

        public IEnumerable<GameObject> GetObjects()
        {
            CheckIfDisposed();

            return ExecSafeReading(this, GetObjectsInternal);
        }

        private static IEnumerable<GameObject> GetObjectsInternal(Location location)
        {
            // Need to copy because the list is changed
            return location._objects.ToArray();
        }

        private TRes ExecSafeReading<TRes>(Location location, Func<Location, TRes> func)
        {
            _sync.EnterReadLock();
            try
            {
                return func(location);
            }
            finally
            {
                _sync.ExitReadLock();
            }
        }

        private TRes ExecSafeReading<TArg, TRes>(Location location, TArg arg, Func<Location, TArg, TRes> func)
        {
            _sync.EnterReadLock();
            try
            {
                return func(location, arg);
            }
            finally
            {
                _sync.ExitReadLock();
            }
        }

        private TRes ExecSafeReading<TArg1, TArg2, TRes>(Location location, TArg1 arg1, TArg2 arg2, Func<Location, TArg1, TArg2, TRes> func)
        {
            _sync.EnterReadLock();
            try
            {
                return func(location, arg1, arg2);
            }
            finally
            {
                _sync.ExitReadLock();
            }
        }

        private void ExecSafeWriting(Location location, Action<Location> action)
        {
            _sync.EnterWriteLock();
            try
            {
                action(location);
            }
            finally
            {
                _sync.ExitWriteLock();
            }
        }

        private void ExecSafeWriting<T>(Location location, T arg, Action<Location, T> action)
        {
            _sync.EnterWriteLock();
            try
            {
                action(location, arg);
            }
            finally
            {
                _sync.ExitWriteLock();
            }
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            _sync.Dispose();
        }
    }
}
