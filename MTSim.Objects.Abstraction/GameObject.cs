namespace MTSim.Objects.Abstraction
{
    public abstract class GameObject
    {
        public long Id { get; }

        public string TypeName { get; }

        public abstract void Act();
    }
}
