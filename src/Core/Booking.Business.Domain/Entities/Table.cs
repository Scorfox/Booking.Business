using Booking.Business.Domain.Common;

namespace Booking.Business.Domain.Entities;

public class Table : BaseEntity
{
    public Guid FilialId { get; set; }
    public Guid CompanyId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int SeatsNumber { get; set; }
    
    public virtual List<Reservation> Reservations { get; set; }
}