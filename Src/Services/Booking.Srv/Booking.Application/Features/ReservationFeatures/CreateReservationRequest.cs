using MediatR;

namespace Booking.Application.Features.ReservationFeatures;

public class CreateReservationRequest : IRequest<CreateReservationResponse>
{
    public Guid TableId { get; set; }
    public DateTimeOffset StartedAt { get; set; }
    public DateTimeOffset FinishedAt { get; set; }
    public Guid WhoBookedId { get; set; }
    public Guid WhoConfirmedId { get; set; }
    public Guid WhoCancelledId { get; set; }
}