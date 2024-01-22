using Booking.Domain.Entities;

namespace Booking.Application.Repositories;

public interface ITableRepository : IBaseRepository<Table>
{
    public Task<bool> HasAnyByFilialIdAndName(Guid filialId, string name, CancellationToken token = default);
}