using MTSim.Map;
using MTSim.Map.Interfaces;
using MTSim.Objects.Abstraction;
using MTSim.Objects.Abstraction.Interfaces;
using MTSim.Objects.Abstraction.Utils;
using MTSim.Objects.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using MTSim.Objects.Behaviors;

namespace MTSim.Objects.Animals
{
    public abstract class Animal : GameObject, ICanBeEaten, IPositioned, IAlive
    {
        protected const double MinSatiety = 0d;

        protected readonly IObjectsBehavior Behavior;
        protected readonly IAnimalsFactory Factory;

        /// <summary>
        /// Остров, на котором находится животное
        /// </summary>
        public Island Island { get; }

        /// <summary>
        /// Координаты животного на острове
        /// </summary>
        public Point Coords { get; protected set; }

        /// <summary>
        /// Максимальная скорость перемещения в клетках
        /// </summary>
        public int MaxSpeed { get; }

        /// <summary>
        /// Вес
        /// </summary>
        public double Weight { get; }

        /// <summary>
        /// Пища и вероятность того, что она будет съедена
        /// </summary>
        public IReadOnlyDictionary<string, double> WhatCanBeEaten { get; }

        /// <summary>
        /// Что может быть съедено
        /// </summary>
        public readonly IReadOnlySet<string> CanBeEaten;

        /// <summary>
        /// Кг еды до полного насыщения
        /// </summary>
        public double MaxSatiety { get; }

        /// <summary>
        /// Текущее насыщение
        /// </summary>
        public double CurrentSatiety { get; protected set; }

        /// <summary>
        /// Скорость потери насыщения
        /// </summary>
        public double SatietyDecreaseSpeed { get; }

        /// <summary>
        /// Признак того, что животное голодно
        /// </summary>
        public bool IsHungry => 2 * CurrentSatiety < MaxSatiety; // Current is less than half of Max

        /// <summary>
        /// Признак того, что животное мертво
        /// </summary>
        public bool IsDead => CurrentSatiety <= MinSatiety;

        /// <summary>
        /// Максимальное количество рождаемых детей
        /// </summary>
        public abstract int MaxChildrenCount { get; }

        protected Animal(
            long id,
            Island island,
            Point coords,
            AnimalProps props,
            IObjectsBehavior behavior,
            IAnimalsFactory factory)
            : base(id)
        {
            Island = island;
            Coords = coords;

            MaxSpeed = props.MaxSpeed;
            MaxSatiety = props.MaxSatiety;
            Weight = props.Weight;
            WhatCanBeEaten = props.Food;

            Behavior = behavior;
            Factory = factory;

            CanBeEaten = props.Food.Keys.ToHashSet();
            CurrentSatiety = props.MaxSatiety;
            SatietyDecreaseSpeed = props.MaxSatiety / 8d;
        }

        public override void Act()
        {
            Behavior.Act(this);
        }

        public virtual GameObject? Eat()
        {
            const int MaxAttempts = 3;

            ThrowIfNotCaptured();

            for (var i = 0; i < MaxAttempts; i++)
            {
                GameObject victim;
                if (!Island.TryGetRandomOfExcept(Coords, CanBeEaten, this, out victim!))
                {
                    continue;
                }

                // trying to capture victim
                if (!SafeExecutor.TryToCapture(victim, out var victimExec))
                {
                    continue;
                }

                using (victimExec)
                {
                    if (!CanWorkWith(victim))
                    {
                        continue;
                    }

                    if (victim is not ICanBeEaten knownVictim)
                    {
                        throw new InvalidOperationException(
                            $"Found victim of unknown type: {victim.GetType().FullName}");
                    }

                    var possibility = WhatCanBeEaten[victim.TypeName];
                    var caught = Random.Shared.NextDouble() <= possibility;

                    if (caught)
                    {
                        var eaten = knownVictim.BeEaten();
                        var newSatiety = CurrentSatiety + eaten;
                        CurrentSatiety = Math.Min(newSatiety, MaxSatiety);

                        return victim;
                    }
                }
            }

            return null;
        }

        private bool CanWorkWith(GameObject obj)
        {
            if (obj is IPositioned positioned
                && positioned.Coords != Coords)
            {
                // Ran away before we captured it
                return false;
            }

            if (obj is IAlive alive
                && alive.IsDead)
            {
                // Already dead
                return false;
            }

            return true;
        }

        public virtual IReadOnlyCollection<Animal> Reproduce()
        {
            const int MaxAttempts = 3;

            ThrowIfNotCaptured();

            for (var i = 0; i < MaxAttempts; i++)
            {
                Animal partner;
                if (!Island.TryGetRandomOfExcept(Coords, this, out partner!))
                {
                    continue;
                }

                if (!SafeExecutor.TryToCapture(partner, out var partnerExec))
                {
                    continue;
                }

                using (partnerExec)
                {
                    if (!CanWorkWith(partner))
                    {
                        continue;
                    }

                    var newAnimals = BornNewAnimals(partner);
                    var added = new List<Animal>(newAnimals.Count);

                    foreach (var newAnimal in newAnimals)
                    {
                        if (Island.CanBeMovedTo(newAnimal, Coords))
                        {
                            Island.Add(newAnimal, Coords);
                            added.Add(newAnimal);
                        }
                    }

                    return added;
                }
            }

            return Array.Empty<Animal>();
        }

        protected virtual IReadOnlyCollection<Animal> BornNewAnimals(Animal partner)
        {
            var number = Random.Shared.Next(MaxChildrenCount + 1);
            if (number == 0)
            {
                return Array.Empty<Animal>();
            }

            return Enumerable
                .Range(0, number)
                .Select(_ => Factory.Create(TypeName, Coords))
                .ToArray();
        }

        public virtual bool Move()
        {
            const int MaxAttempts = 3;

            ThrowIfNotCaptured();

            if (MaxSpeed <= 0)
            {
                return false;
            }

            for (var i = 0; i < MaxAttempts; i++)
            {
                var newCoords = CreateNextPoint();

                if (Island.CanBeMovedTo(this, newCoords))
                {
                    Island.Move(this, Coords, newCoords);
                    Coords = newCoords;
                    return true;
                }
            }

            return false;
        }

        private Point CreateNextPoint()
        {
            var newXDiff = Random.Shared.Next(-MaxSpeed, MaxSpeed + 1); // [-MaxSpeed;+MaxSpeed]
            var newYDiff = Random.Shared.Next(-MaxSpeed, MaxSpeed + 1);

            var newX = RoundToInterval(0, Coords.X + newXDiff, Island.Width - 1);
            var newY = RoundToInterval(0, Coords.Y + newYDiff, Island.Height - 1);

            return new Point(newX, newY);
        }

        private static int RoundToInterval(int min, int value, int max)
        {
            return Math.Max(min, Math.Min(max, value));
        }

        public virtual void DecreaseSatiety()
        {
            ThrowIfNotCaptured();

            var newSatiety = CurrentSatiety - SatietyDecreaseSpeed;
            CurrentSatiety = Math.Max(MinSatiety, newSatiety);
        }

        public virtual double BeEaten()
        {
            ThrowIfNotCaptured();

            // TODO maybe it's better to use dedicated flag
            CurrentSatiety = 0d;

            var coef = (Random.Shared.NextDouble() / 2) + 0.5d; //[0.25;0.75]
            return Weight * coef;
        }
    }
}