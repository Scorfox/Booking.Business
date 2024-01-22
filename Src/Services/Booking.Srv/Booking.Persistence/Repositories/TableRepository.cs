using Booking.Application.Context;
using Booking.Application.Repositories;
using Booking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Booking.Persistence.Repositories;

public class TableRepository : BaseRepository<Table>, ITableRepository
{
    protected TableRepository(DataContext context) : base(context)
    {
    }

    public async Task<bool> HasAnyByFilialIdAndName(Guid filialId, string name, CancellationToken token = default)
    {
        return await Context.Tables
            .AsNoTracking()
            .AnyAsync(e => e.FilialId == filialId && e.Name == name, cancellationToken: token);
    }
}