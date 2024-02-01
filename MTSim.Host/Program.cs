using MTSim.Game;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MTSim.Host
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await await Task.Delay(1000)
                .ContinueWith(async _ =>
                {
                    Console.WriteLine("First");
                    await Task.Delay(2000);
                    Console.WriteLine("Second");
                });

            Console.WriteLine("Third");
            Console.ReadLine();

            //var cts = new CancellationTokenSource();

            //Console.CancelKeyPress += (sender, e) =>
            //{
            //    cts.Cancel();
            //    e.Cancel = true;
            //};

            //await GameBuilder
            //    .InitGame("D:\\repos\\Simulation\\MTSim\\gameConfig.json", "D:\\repos\\Simulation\\MTSim\\foodMatrix.csv")
            //    .RunAsync(cts.Token);
        }
    }
}
