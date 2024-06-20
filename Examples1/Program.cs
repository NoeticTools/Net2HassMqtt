using FluentDate;
using Microsoft.Extensions.Configuration;


namespace NoeticTools.Net2HassMqtt.Examples1;

internal class Program
{
    private static async Task Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        var appConfig = new ConfigurationBuilder().AddUserSecrets<Program>()
                                                  .Build();

        var bridge = NodeIdsConfigurationExample.Example(appConfig);

        await bridge.StartAsync();
        await Run();
        await bridge.StopAsync();
    }

    private static async Task Run()
    {
        while (true)
        {
            await Task.Delay(100.Milliseconds());
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey();
                if (key.KeyChar == 'x')
                {
                    break;
                }
            }
        }
    }
}