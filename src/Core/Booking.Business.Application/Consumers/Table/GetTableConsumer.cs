using AutoMapper;
using Booking.Business.Application.Exceptions;
using Booking.Business.Application.Repositories;
using MassTransit;
using Otus.Booking.Common.Booking.Contracts.Table.Requests;

namespace Booking.Business.Application.Consumers.Table
{
    public sealed class GetTableConsumer : IConsumer<GetTableId>
    {
        private readonly ITableRepository _tableRepository;
        private readonly IMapper _mapper;

        public GetTableConsumer(ITableRepository tableRepository, IMapper mapper)
        {
            _tableRepository = tableRepository;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<GetTableId> context)
        {
            var request = context.Message;

            if (!await _tableRepository.HasAnyByIdAsync(request.Id))
                throw new BadRequestException($"Table with ID {request.Id} doesn't exists");

            await context.RespondAsync(_tableRepository.FindByIdAsync(request.Id, default));
        }
    }
}
