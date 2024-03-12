﻿using AutoMapper;
using Booking.Business.Application.Repositories;
using MassTransit;
using Otus.Booking.Common.Booking.Contracts.Table.Models;
using Otus.Booking.Common.Booking.Contracts.Table.Requests;
using Otus.Booking.Common.Booking.Contracts.Table.Responses;

namespace Booking.Business.Application.Consumers.Table;

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

        var tables = await _tableRepository.GetPaginatedListAsync(request.Offset, request.Count);
        var totalCount = await _tableRepository.GetTotalCount();
        var values = new GetTablesListResult
        {
            Elements = _mapper.Map<List<FullTableDto>>(tables), TotalCount = totalCount
        };

        await context.RespondAsync(values);
    }
}