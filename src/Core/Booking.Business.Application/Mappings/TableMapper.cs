using AutoMapper;
using Otus.Booking.Common.Booking.Contracts.Table.Models;
using Otus.Booking.Common.Booking.Contracts.Table.Requests;
using Otus.Booking.Common.Booking.Contracts.Table.Responses;

namespace Booking.Business.Application.Mappings;

public sealed class TableMapper : Profile
{
    public TableMapper()
    {
        CreateMap<CreateTable, Domain.Entities.Table>();
        CreateMap<Domain.Entities.Table, CreateTableResult>();

        CreateMap<Domain.Entities.Table, FullTableDto>();

        CreateMap<UpdateTable, Domain.Entities.Table>();
        CreateMap<Domain.Entities.Table, UpdateTableResult>();

        CreateMap<GetTableById, Domain.Entities.Table>();
        CreateMap<Domain.Entities.Table, GetTableResult>();
    }
}