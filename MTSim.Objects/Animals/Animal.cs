using MTSim.Map;
using MTSim.Objects.Abstraction;
using System;
using System.Collections.Generic;

namespace MTSim.Objects.Animals
{
    public abstract class Animal : GameObject
    {
        protected const double MinSatiety = 0d;

        /// <summary>
        /// Максимальная скорость перемещения в клетках
        /// </summary>
        protected int MaxSpeed { get; }

        /// <summary>
        /// Кг еды до полного насыщения
        /// </summary>
        protected double MaxSatiety { get; }

        /// <summary>
        /// Вектор пищи с вероятностью того, что она будет съедена
        /// </summary>
        protected Dictionary<string, double> WhatCanBeEaten { get; } = new();

        /// <summary>
        /// Вес
        /// </summary>
        public double Weight { get; }

        /// <summary>
        /// Текущее насыщение
        /// </summary>
        public double CurrentSatiety { get; protected set; }

        /// <summary>
        /// Признак того, что животное мертво
        /// </summary>
        public virtual bool IsDead => CurrentSatiety <= MinSatiety;

        /// <summary>
        /// Остров, на котором находится животное
        /// </summary>
        protected Island Island { get; }

        /// <summary>
        /// Координаты животного на острове
        /// </summary>
        protected Point Coords { get; }

        public override void Act()
        {
            if (CurrentSatiety < (MaxSatiety / 2d))
            {
                // It's hungry
                Eat();
                return;
            }

            if (Random.Shared.NextDouble() < 0.5d)
            {
                Reproduce();
            }
            else
            {
                Move();
            }
        }

        protected virtual void Eat() { }

        protected virtual void Reproduce() { }

        protected virtual void Move() { }
    }
}
