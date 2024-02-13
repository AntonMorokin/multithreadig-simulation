using Microsoft.Extensions.Logging;
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
            var cts = new CancellationTokenSource();
            //cts.CancelAfter(TimeSpan.FromSeconds(30));

            Console.CancelKeyPress += (sender, e) =>
            {
                cts.Cancel();
                e.Cancel = true;
            };

            var loggerFactory = LoggerFactory.Create(x =>
            {
                x.AddConsole();
            });

            var logger = loggerFactory.CreateLogger("game");

            using var disposable = await GameBuilder
                .InitGame("D:\\repos\\Simulation\\MTSim\\gameConfig.json", "D:\\repos\\Simulation\\MTSim\\foodMatrix.csv", logger)
                .RunAsync(cts.Token);

            logger.LogInformation("Finished");
        }
    }
}
