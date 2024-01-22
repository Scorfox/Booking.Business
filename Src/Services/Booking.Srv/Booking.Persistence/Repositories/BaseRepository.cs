using Booking.Application.Context;
using Booking.Application.Repositories;
using Booking.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Booking.Persistence.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    protected readonly DataContext Context;

    protected BaseRepository(DataContext context)
    {
        Context = context;
    }
    
    public void Create(T entity)
    {
        entity.CreatedAt = DateTimeOffset.UtcNow;
        Context.Add(entity);
    }

    public void Update(T entity)
    {
        entity.UpdatedAt = DateTimeOffset.UtcNow;
        Context.Update(entity);
    }

    public void Delete(T entity)
    {
        Context.Remove(entity);
    }

    public Task<T> Get(Guid id, CancellationToken cancellationToken)
    {
        return Context.Set<T>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<List<T>> GetAll(CancellationToken cancellationToken)
    {
        return Context.Set<T>().ToListAsync(cancellationToken);
    }
}