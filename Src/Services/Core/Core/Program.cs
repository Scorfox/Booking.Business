using Core.Helpers;
using MassTransit;

namespace Core;

internal class Program
{
    static async Task Main(string[] args)
    {
        var busControl = Bus.Factory.CreateUsingRabbitMq(RabbitMqConfigurator.ConfigureBus);

        using var ct = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        await busControl.StartAsync(ct.Token);

        try
        {
            Console.WriteLine("Bus was started");
        }
        finally
        {
            await busControl.StopAsync(CancellationToken.None);
        }
    }
}
