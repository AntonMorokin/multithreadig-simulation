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
        public virtual double GrowSpeed { get; }

        /// <summary>
        /// Вес
        /// </summary>
        public virtual double Weight { get; set; }

        /// <summary>
        /// Остров, на котором находится животное
        /// </summary>
        public virtual Island Island { get; }

        /// <summary>
        /// Координаты животного на острове
        /// </summary>
        public virtual Point Coords { get; }

        /// <summary>
        /// Признак того, что растение мертво
        /// </summary>
        public virtual bool IsDead => false; // cannot die

        protected Plant(long id, Island island, Point coords, double growSpeed, double weight)
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

        public virtual void Grow()
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
