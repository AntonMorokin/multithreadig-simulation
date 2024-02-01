namespace MTSim.Game
{
    public interface IInitializedGame
    {
        Task RunAsync(CancellationToken cancellationToken);
    }
}
