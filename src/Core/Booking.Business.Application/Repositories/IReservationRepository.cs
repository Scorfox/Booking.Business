using Booking.Business.Domain.Entities;

namespace Booking.Business.Application.Repositories;

public interface IReservationRepository : IBaseRepository<Reservation>
{
    public Task<Tuple<List<Reservation>, int>> GetReservationsList(int offset, int limit,
        CancellationToken cancellationToken = default);

    public Task DeleteReservation(Guid id, CancellationToken cancellationToken = default);
}