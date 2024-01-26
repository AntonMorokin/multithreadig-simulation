namespace MTSim.Objects.Abstraction.Animals
{
    public abstract class Animal : GameObject
    {
        /// <summary>
        /// Вес
        /// </summary>
        public double Weight { get; }

        /// <summary>
        /// Максимальная скорость перемещения в клетках
        /// </summary>
        protected int MaxSpeed { get; }

        /// <summary>
        /// Кг еды до полного насыщения
        /// </summary>
        protected decimal FullSatiety { get; }

        public abstract void Eat();

        public abstract void Fuck();

        public abstract void Move();
    }
}
