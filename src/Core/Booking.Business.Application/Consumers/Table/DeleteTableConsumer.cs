using Booking.Business.Application.Repositories;
using MassTransit;
using Otus.Booking.Common.Booking.Contracts.Table.Requests;
using Otus.Booking.Common.Booking.Contracts.Table.Responses;

namespace Booking.Business.Application.Consumers.Table;

public class DeleteTableConsumer:IConsumer<DeleteTable>
{
    private readonly ITableRepository _tableRepository;

    public DeleteTableConsumer(ITableRepository tableRepository)
    {
        _tableRepository = tableRepository;
    }

    public async Task Consume(ConsumeContext<DeleteTable> context)
    {
        var req = context.Message;

        await _tableRepository.DeleteTable(req.Id);

        await context.RespondAsync(new DeleteTableResult());
    }
}