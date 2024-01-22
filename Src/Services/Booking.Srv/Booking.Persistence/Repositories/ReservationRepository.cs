using Booking.Application.Context;
using Booking.Application.Repositories;
using Booking.Domain.Entities;

namespace Booking.Persistence.Repositories;

public class ReservationRepository : BaseRepository<Reservation>, IReservationRepository
{
    protected ReservationRepository(DataContext context) : base(context)
    {
    }
}