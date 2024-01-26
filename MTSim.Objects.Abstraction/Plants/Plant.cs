namespace MTSim.Objects.Abstraction.Plants
{
    public abstract class Plant : GameObject
    {
        /// <summary>
        /// Вес
        /// </summary>
        public double Weight { get; }

        /// <summary>
        /// Скорость роста
        /// </summary>
        protected double GrowSpeed { get; }

        public abstract void Grow();
    }
}
