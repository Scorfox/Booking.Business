using MediatR;

namespace Booking.Application.Features.TableFeatures;

public sealed record CreateTableRequest : IRequest<CreateTableResponse>
{
    public Guid FilialId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int SeatsNumber { get; set; }
}