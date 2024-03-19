using AutoMapper;
using Otus.Booking.Common.Booking.Contracts.Table.Models;
using Otus.Booking.Common.Booking.Contracts.Table.Requests;
using Otus.Booking.Common.Booking.Contracts.Table.Responses;

namespace Booking.Business.Application.Mappings;

public sealed class TableMapper : Profile
{
    public TableMapper()
    {
        // Create
        CreateMap<CreateTable, Domain.Entities.Table>();
        CreateMap<Domain.Entities.Table, CreateTableResult>();

        // Read
        CreateMap<Domain.Entities.Table, GetTableResult>();
        CreateMap<Domain.Entities.Table, TableGettingDto>();
        
        // Update
        CreateMap<UpdateTable, Domain.Entities.Table>();
        CreateMap<Domain.Entities.Table, UpdateTableResult>();
    }
}