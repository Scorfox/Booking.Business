using Booking.Domain.Common;

namespace Booking.Domain.Entities;

public class Reservation : BaseEntity
{   
    public Guid TableId { get; set; }
    public DateTimeOffset StartedAt { get; set; }
    public DateTimeOffset FinishedAt { get; set; }
    public Guid WhoBookedId { get; set; }
    public Guid WhoConfirmedId { get; set; }
    public Guid WhoCancelledId { get; set; }
}