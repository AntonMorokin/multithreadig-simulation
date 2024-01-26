using MTSim.Map;
using MTSim.Objects.Abstraction;
using System;

namespace MTSim.Objects.Animals
{
    public abstract class Animal : GameObject
    {
        /// <summary>
        /// Максимальная скорость перемещения в клетках
        /// </summary>
        protected int MaxSpeed { get; }

        /// <summary>
        /// Кг еды до полного насыщения
        /// </summary>
        protected double MaxSatiety { get; }

        /// <summary>
        /// Вес
        /// </summary>
        public double Weight { get; }

        /// <summary>
        /// Текущее насыщение
        /// </summary>
        public double CurrentSatiety { get; protected set; }

        protected Island Island { get; }

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
                Fuck();
            }
            else
            {
                Move();
            }
        }

        protected abstract void Eat();

        protected abstract void Fuck();

        protected abstract void Move();
    }
}
