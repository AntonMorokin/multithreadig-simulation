namespace MTSim.Game.Interfaces
{
    public interface IInitializedGame
    {
        Task<IDisposable> RunAsync(CancellationToken cancellationToken);
    }
}
