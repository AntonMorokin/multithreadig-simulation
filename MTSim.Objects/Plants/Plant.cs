using MTSim.Map;
using MTSim.Map.Interfaces;
using MTSim.Objects.Abstraction;
using MTSim.Objects.Abstraction.Interfaces;
using System;
using MTSim.Objects.Behaviors;

namespace MTSim.Objects.Plants
{
    public abstract class Plant : GameObject, ICanBeEaten, IPositioned, IAlive
    {
        protected const double MinWeight = 0.1d;

        protected readonly IObjectsBehavior Behavior;

        /// <summary>
        /// Остров, на котором находится животное
        /// </summary>
        public Island Island { get; }

        /// <summary>
        /// Координаты животного на острове
        /// </summary>
        public Point Coords { get; }

        /// <summary>
        /// Скорость роста
        /// </summary>
        public double GrowSpeed { get; }

        /// <summary>
        /// Вес
        /// </summary>
        public double Weight { get; protected set; }

        /// <summary>
        /// Признак того, что растение мертво
        /// </summary>
        public virtual bool IsDead => false; // cannot die

        protected Plant(long id, Island island, Point coords, PlantProps props, IObjectsBehavior behavior)
            : base(id)
        {
            Behavior = behavior;
            Island = island;
            Coords = coords;
            GrowSpeed = props.GrowSpeed;
            Weight = props.Weight;
        }

        public override void Act()
        {
            Behavior.Act(this);
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
