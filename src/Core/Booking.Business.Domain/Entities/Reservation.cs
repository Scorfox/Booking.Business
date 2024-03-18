using Booking.Business.Domain.Common;

namespace Booking.Business.Domain.Entities;

public class Reservation : BaseEntity
{
    public Guid TableId { get; set; }
    public virtual Table Table { get; set; }
    
    public Guid WhoBookedId { get; set; }
    public Guid? WhoConfirmedId { get; set; }
    public Guid? WhoCancelledId { get; set; }
    
    public DateTimeOffset From { get; set; }
    public DateTimeOffset To { get; set; }
}