using Booking.Business.Application.Repositories;
using Booking.Business.Domain.Entities;
using Booking.Business.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Booking.Business.Persistence.Repositories;

public class ReservationRepository(DataContext context) : BaseRepository<Reservation>(context), IReservationRepository
{
    public async Task<Tuple<List<Reservation>, int>> GetReservationsList(int offset, int limit, CancellationToken cancellationToken = default)
    {
        return new Tuple<List<Reservation>, int>(await Context.Reservations.AsNoTracking().Skip(offset).Take(limit).ToListAsync(cancellationToken), await Context.Reservations.CountAsync(cancellationToken));
    }

    public async Task DeleteReservation(Guid id, CancellationToken cancellationToken)
    {
        bool any = await Context.Reservations.AnyAsync(elm => elm.Id == id, cancellationToken);
        if(any)
            await Context.Reservations.AsNoTracking().Where(elm => elm.Id == id).ExecuteDeleteAsync(cancellationToken);
    }
}