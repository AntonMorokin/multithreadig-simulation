using MTSim.Map;
using MTSim.Objects.Abstraction;
using System;

namespace MTSim.Objects.Plants
{
    public abstract class Plant : GameObject
    {
        protected const double MinWeight = 0.1d;

        protected readonly object _sync = new();

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
        protected Island Island { get; }

        /// <summary>
        /// Координаты животного на острове
        /// </summary>
        protected Point Coords { get; }

        public override void Act()
        {
            Grow();
        }

        protected virtual void Grow()
        {
            var coef = Random.Shared.NextDouble() + 0.5d;
            lock (_sync)
            {
                Weight += GrowSpeed * coef; // [+0.5*GrowSpeed...+1.5*GrowSpeed]
            }
        }

        public virtual double BeEaten()
        {
            // TODO it can be lock-free but needs to be correctly implemented
            lock (_sync)
            {
                var eaten = Weight - MinWeight;
                Weight = MinWeight;
                return eaten;
            }
        }
    }
}
