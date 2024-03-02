using AutoMapper;
using Booking.Business.Application.Repositories;
using MassTransit;
using Otus.Booking.Common.Booking.Contracts.Table.Requests;
using Otus.Booking.Common.Booking.Contracts.Table.Responses;
using Otus.Booking.Common.Booking.Exceptions;

namespace Booking.Business.Application.Consumers.Table;

public class CreateTableConsumer : IConsumer<CreateTable>
{
    private readonly ITableRepository _tableRepository;
    private readonly IMapper _mapper;

    public CreateTableConsumer(ITableRepository tableRepository, IMapper mapper)
    {
        _tableRepository = tableRepository;
        _mapper = mapper;
    }
    
    public async Task Consume(ConsumeContext<CreateTable> context)
    {
        var request = context.Message;
        
        if (await _tableRepository.HasAnyWithFilialIdAndNameAsync(request.FilialId, request.Name))
            throw new BadRequestException($"Table with NAME {request.Name} and FILIAL {request.FilialId} already exists");
            
        var filial = _mapper.Map<Domain.Entities.Table>(request);
        
        await _tableRepository.CreateAsync(filial);

        await context.RespondAsync(_mapper.Map<CreateTableResult>(filial));
    }
}