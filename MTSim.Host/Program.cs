using MTSim.Configuration;

namespace MTSim.Host
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var config = ConfigLoader.Load("D:\\repos\\Simulation\\MTSim\\gameConfig.json", "D:\\repos\\Simulation\\MTSim\\foodMatrix.csv");
        }
    }
}
