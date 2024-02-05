using MTSim.Map;
using MTSim.Map.Interfaces;
using MTSim.Objects.Abstraction;
using MTSim.Objects.Abstraction.Interfaces;
using System;

namespace MTSim.Objects.Plants
{
    public abstract class Plant : GameObject, ICanBeEaten, IPositioned, IAlive
    {
        protected const double MinWeight = 0.1d;

        /// <summary>
        /// Скорость роста
        /// </summary>
        protected double GrowSpeed { get; }

        /// <summary>
        /// Вес
        /// </summary>
        public double Weight { get; protected set; }

        /// <summary>
        /// Остров, на котором находится животное
        /// </summary>
        public Island Island { get; }

        /// <summary>
        /// Координаты животного на острове
        /// </summary>
        public Point Coords { get; }

        /// <summary>
        /// Признак того, что растение мертво
        /// </summary>
        public bool IsDead => false; // cannot die

        protected Plant(int id, Island island, Point coords, double growSpeed, double weight)
            : base(id)
        {
            Island = island;
            Coords = coords;
            GrowSpeed = growSpeed;
            Weight = weight;
        }

        protected override void ActInternal()
        {
            Grow();
        }

        protected virtual void Grow()
        {
            var coef = Random.Shared.NextDouble() + 0.5d;
            Weight += GrowSpeed * coef; // [+0.5*GrowSpeed...+1.5*GrowSpeed]
        }

        public virtual double BeEaten()
        {
            // Assuming that it will be called only when captured. But check it anyway
            ThrowIfNotCaptured();

            var eaten = Weight - MinWeight;
            Weight = MinWeight;
            return eaten;
        }
    }
}
