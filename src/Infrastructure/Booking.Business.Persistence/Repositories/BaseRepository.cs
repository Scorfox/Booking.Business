using Booking.Business.Application.Repositories;
using Booking.Business.Domain.Common;
using Booking.Business.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Booking.Business.Persistence.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    protected readonly DataContext Context;

    protected BaseRepository(DataContext context)
    {
        Context = context;
    }
    
    public async Task CreateAsync(T entity)
    {
        entity.CreatedAt = DateTimeOffset.UtcNow;
        await Context.AddAsync(entity);
        await Context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        entity.UpdatedAt = DateTimeOffset.UtcNow;
        Context.Update(entity);
        await Context.SaveChangesAsync();
    }

    public async Task Delete(T entity)
    {
        Context.Remove(entity);
        await Context.SaveChangesAsync();
    }

    public async Task<T?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await Context.Set<T>().SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<bool> HasAnyByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await Context.Set<T>().AnyAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<List<T>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await Context.Set<T>().ToListAsync(cancellationToken);
    }
}