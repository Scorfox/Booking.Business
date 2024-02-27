using Booking.Business.Application.Repositories;
using Booking.Business.Domain.Entities;
using Booking.Business.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Booking.Business.Persistence.Repositories;

public class TableRepository(DataContext context) : BaseRepository<Table>(context), ITableRepository
{
    public async Task<bool> HasAnyWithFilialIdAndNameAsync(Guid filialId, string name, CancellationToken token = default)
    {
        return await Context.Tables
            .AsNoTracking()
            .AnyAsync(x => x.FilialId == filialId && x.Name.ToLower() == name.ToLower(), token);
    }

    public async Task<bool> HasAnyWithFilialIdAndNameExceptIdAsync(Guid id, Guid filialId, string name, CancellationToken token = default)
    {
        return await Context.Tables
            .AsNoTracking()
            .AnyAsync(x => x.FilialId == filialId && x.Name.ToLower() == name.ToLower() && x.Id != id, token);
    }

    public async Task<List<Table>> GetTablesList(int offset, int limit, CancellationToken token = default)
    {
        return await Context.Tables.AsNoTracking().Skip(offset).Take(limit).ToListAsync(token);
    }

    public async Task DeleteTable(Guid id, CancellationToken cancellationToken = default)
    {
        await Context.Tables.AsNoTracking().Where(elm=>elm.Id == id).ExecuteDeleteAsync(cancellationToken);
    }
}