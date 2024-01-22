using Booking.Domain.Common;

namespace Booking.Domain.Entities;

public class Table : BaseEntity
{
    public Guid FilialId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int SeatsNumber { get; set; }

    public ICollection<Reservation> Reservations { get; set; }
}