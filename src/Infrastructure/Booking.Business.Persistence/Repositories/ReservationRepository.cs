using Booking.Business.Application.Repositories;
using Booking.Business.Domain.Entities;
using Booking.Business.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Booking.Business.Persistence.Repositories;

public class ReservationRepository(DataContext context) : BaseRepository<Reservation>(context), IReservationRepository
{
    public async Task<List<Reservation>> GetReservationsList(int offset, int limit, CancellationToken cancellationToken = default)
    {
        return await Context.Reservations.AsNoTracking().Skip(offset).Take(limit).ToListAsync(cancellationToken);
    }

    public async Task DeleteReservation(Guid id, CancellationToken cancellationToken)
    {
        await Context.Reservations.AsNoTracking().Where(elm => elm.Id == id).ExecuteDeleteAsync(cancellationToken);
    }
}