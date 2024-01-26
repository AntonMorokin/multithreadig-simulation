namespace MTSim.Objects.Abstraction
{
    public abstract class GameObject
    {
        public long Id { get; }

        public abstract void Act();
    }
}
