using Booking.Domain.Common;

namespace Booking.Application.Repositories;

public interface IBaseRepository<T> where T : class
{
    void Create(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task<T> Get(Guid id, CancellationToken cancellationToken);
    Task<List<T>> GetAll(CancellationToken cancellationToken);
}