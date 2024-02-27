using AutoMapper;
using Booking.Business.Application.Repositories;
using MassTransit;
using Otus.Booking.Common.Booking.Contracts.Table.Models;
using Otus.Booking.Common.Booking.Contracts.Table.Requests;
using Otus.Booking.Common.Booking.Contracts.Table.Responses;

namespace Booking.Business.Application.Consumers.Table
{
    public class GetTablesListConsumer:IConsumer<GetTablesList>
    {
        private readonly ITableRepository _tableRepository;
        private readonly IMapper _mapper;

        public GetTablesListConsumer(ITableRepository tableRepository, IMapper mapper)
        {
            _tableRepository = tableRepository;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<GetTablesList> context)
        {
            var request = context.Message;

            var data = await _tableRepository.GetTablesList(request.Offset, request.Limit);

            await context.RespondAsync(new GetTablesListResult
                {Tables = data.Select(elm => _mapper.Map<FullTableDto>(elm)).ToList()});
        }
    }
}
