using Booking.Business.Domain.Entities;

namespace Booking.Business.Application.Repositories;

public interface IReservationRepository : IBaseRepository<Reservation>
{
    public Task<List<Reservation>> GetReservationsList(int offset, int limit,
        CancellationToken cancellationToken = default);

    public Task<int> GetReservationsTotalCount(CancellationToken  cancellationToken = default);


    public Task DeleteReservation(Guid id, CancellationToken cancellationToken = default);
}