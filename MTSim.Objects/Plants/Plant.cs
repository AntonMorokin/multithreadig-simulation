using MTSim.Map;
using MTSim.Objects.Abstraction;
using System;

namespace MTSim.Objects.Plants
{
    public abstract class Plant : GameObject
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
        protected Island Island { get; }

        /// <summary>
        /// Координаты животного на острове
        /// </summary>
        protected Point Coords { get; }

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
            // Assuming that it will be called only when captured
            // TODO need to add API to call these methods only when captured
            var eaten = Weight - MinWeight;
            Weight = MinWeight;
            return eaten;
        }
    }
}
