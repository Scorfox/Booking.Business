using AutoMapper;
using Booking.Domain.Entities;

namespace Booking.Application.Features.TableFeatures;

public sealed class CreateTableMapper : Profile
{
    public CreateTableMapper()
    {
        CreateMap<CreateTableRequest, Table>();
        CreateMap<Table, CreateTableResponse>();
    }
}