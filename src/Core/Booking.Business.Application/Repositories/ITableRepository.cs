using Booking.Business.Domain.Entities;

namespace Booking.Business.Application.Repositories;

public interface ITableRepository : IBaseRepository<Table>
{
    public Task<bool> HasAnyWithFilialIdAndNameAsync(Guid filialId, string name, CancellationToken token = default);
    public Task<bool> HasAnyWithFilialIdAndNameExceptIdAsync(Guid id, Guid filialId, string name, CancellationToken token = default);
    public Task<Tuple<List<Table>, int>> GetTablesList(int offset, int limit, CancellationToken token = default);
    public Task DeleteTable(Guid id, CancellationToken cancellationToken = default);
}