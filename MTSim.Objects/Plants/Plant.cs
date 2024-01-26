using MTSim.Objects.Abstraction;

namespace MTSim.Objects.Plants
{
    public abstract class Plant : GameObject
    {
        /// <summary>
        /// Скорость роста
        /// </summary>
        protected double GrowSpeed { get; }

        /// <summary>
        /// Вес
        /// </summary>
        public double Weight { get; protected set; }

        public override void Act()
        {
            Grow();
        }

        protected abstract void Grow();
    }
}
