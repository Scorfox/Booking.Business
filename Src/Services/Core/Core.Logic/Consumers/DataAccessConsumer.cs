using Base.Contracts.RabbitMq;
using MassTransit;

namespace Core.Logic.Consumers
{
    public class DataAccessConsumer :IConsumer<IDataService>
    {
        public Task Consume(ConsumeContext<IDataService> context)
        {
            return Task.CompletedTask;
        }
    }
}
