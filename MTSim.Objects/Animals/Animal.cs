using MTSim.Map;
using MTSim.Map.Interfaces;
using MTSim.Objects.Abstraction;
using MTSim.Objects.Abstraction.Interfaces;
using MTSim.Objects.Abstraction.Utils;
using MTSim.Objects.Factories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MTSim.Objects.Animals
{
    public abstract class Animal : GameObject, ICanBeEaten, IPositioned, IAlive
    {
        protected const double MinSatiety = 0d;

        protected readonly HashSet<string> _typesCanBeEaten;

        /// <summary>
        /// Максимальная скорость перемещения в клетках
        /// </summary>
        public virtual int MaxSpeed { get; }

        /// <summary>
        /// Кг еды до полного насыщения
        /// </summary>
        public virtual double MaxSatiety { get; }

        /// <summary>
        /// Вектор пищи с вероятностью того, что она будет съедена
        /// </summary>
        public virtual IReadOnlyDictionary<string, double> WhatCanBeEaten { get; }

        /// <summary>
        /// Вес
        /// </summary>
        public virtual double Weight { get; }

        /// <summary>
        /// Текущее насыщение
        /// </summary>
        public virtual double CurrentSatiety { get; set; }

        /// <summary>
        /// Скорость потери насыщения
        /// </summary>
        public virtual double SatietyDecreaseSpeed { get; }

        /// <summary>
        /// Признак того, что животное голодно
        /// </summary>
        public virtual bool IsHungry => 2 * CurrentSatiety < MaxSatiety; // Current is less than half of Max

        /// <summary>
        /// Признак того, что животное мертво
        /// </summary>
        public virtual bool IsDead => CurrentSatiety <= MinSatiety;

        /// <summary>
        /// Остров, на котором находится животное
        /// </summary>
        public virtual Island Island { get; }

        /// <summary>
        /// Координаты животного на острове
        /// </summary>
        public virtual Point Coords { get; set; }

        protected Animal(long id, Island island, Point coords, int maxSpeed, double maxSatiety, double weight, IReadOnlyDictionary<string, double> food)
            : base(id)
        {
            Island = island;
            Coords = coords;
            MaxSpeed = maxSpeed;
            MaxSatiety = maxSatiety;
            Weight = weight;
            WhatCanBeEaten = food;

            _typesCanBeEaten = food.Keys.ToHashSet();
            CurrentSatiety = maxSatiety;
            SatietyDecreaseSpeed = maxSatiety / 8d;
        }

        protected override void ActInternal()
        {
            Live();
            FinishAct();
        }

        private void Live()
        {
            if (IsHungry)
            {
                if (Island.AnyOfExcept(Coords, _typesCanBeEaten, this))
                {
                    // It's hungry and here is something to eat
                    Eat();
                }
                else
                {
                    // It's hungry and but here is nothing to eat. Leave location
                    Move();
                }

                return;
            }

            var wantToReproduce = Random.Shared.NextDouble() < 0.5d;
            if (wantToReproduce)
            {
                if (Island.AnyOfExcept(Coords, this))
                {
                    // It wants to reproduce and here is someone to do so with
                    Reproduce();
                    return;
                }
            }

            // It wants to reproduce but here is noone to do so with. Leave location
            Move();
        }

        public virtual void Eat()
        {
            const int MaxAttempts = 3;

            for (var i = 0; i < MaxAttempts; i++)
            {
                GameObject victim;
                if (!Island.TryGetRandomOfExcept(Coords, _typesCanBeEaten, this, out victim!))
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
                        throw new InvalidOperationException($"Found victim of unknown type: {victim.GetType().FullName}");
                    }

                    var possibility = WhatCanBeEaten[victim.TypeName];
                    var catched = Random.Shared.NextDouble() <= possibility;

                    if (catched)
                    {
                        var eaten = knownVictim.BeEaten();
                        var newSatiety = CurrentSatiety + eaten;
                        CurrentSatiety = Math.Min(newSatiety, MaxSatiety);
                    }
                }
            }
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

        public virtual void Reproduce()
        {
            const int MaxAttempts = 3;

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
                    foreach (var newAnimal in newAnimals)
                    {
                        if (Island.CanBeMovedTo(newAnimal, Coords))
                        {
                            Island.Add(newAnimal, Coords);
                        }
                    }
                }
            }
        }

        public abstract IReadOnlyCollection<Animal> BornNewAnimals(Animal partner);

        protected virtual IReadOnlyCollection<Animal> BornNewAnimalsTemplate(
            Animal partner,
            int maxChilderNumber,
            IAnimalsFactory factory,
            Func<Point, IAnimalsFactory, Animal> creator)
        {
            var number = Random.Shared.Next(maxChilderNumber + 1);
            if (number == 0)
            {
                return Array.Empty<Animal>();
            }

            return Enumerable
                .Range(0, number)
                .Select(x => creator(Coords, factory))
                .ToArray();
        }

        public virtual void Move()
        {
            const int MaxAttempts = 3;

            if (MaxSpeed <= 0)
            {
                return;
            }

            for (var i = 0; i < MaxAttempts; i++)
            {
                var newCoords = CreateNextPoint();

                if (Island.CanBeMovedTo(this, newCoords))
                {
                    Island.Move(this, Coords, newCoords);
                    Coords = newCoords;
                    return;
                }
            }

        }

        private Point CreateNextPoint()
        {
            var newXDiff = Random.Shared.Next(-MaxSpeed, MaxSpeed + 1); // [-MaxSpeed;+MaxSpeed]
            var newYDiff = Random.Shared.Next(-MaxSpeed, MaxSpeed + 1);

            var newX = RoundToInterval(0, Coords.X + newXDiff, Island.Width - 1);
            var newY = RoundToInterval(0, Coords.Y + newYDiff, Island.Height - 1);

            return new Point(newX, newY);
        }

        private int RoundToInterval(int min, int value, int max)
        {
            return Math.Max(min, Math.Min(max, value));
        }

        protected virtual void FinishAct()
        {
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
