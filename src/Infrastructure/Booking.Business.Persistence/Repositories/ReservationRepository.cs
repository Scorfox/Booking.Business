using Booking.Business.Application.Repositories;
using Booking.Business.Domain.Entities;
using Booking.Business.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Booking.Business.Persistence.Repositories;

public class ReservationRepository(DataContext context) : BaseRepository<Reservation>(context), IReservationRepository
{
    public async Task<List<Reservation>> GetReservationsList(int offset, int limit,
        CancellationToken cancellationToken = default)
        => await base.GetPaginatedListAsync(offset, limit, cancellationToken);

    public async Task<int> GetReservationsTotalCount(CancellationToken cancellationToken = default)
        => await base.GetTotalCount(cancellationToken);

    public async Task DeleteReservation(Guid id, CancellationToken cancellationToken)
    {
        bool any = await Context.Reservations.AnyAsync(elm => elm.Id == id, cancellationToken);
        if(any)
            await Context.Reservations.AsNoTracking().Where(elm => elm.Id == id).ExecuteDeleteAsync(cancellationToken);
    }
}