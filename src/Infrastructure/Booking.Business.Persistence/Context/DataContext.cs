using Booking.Business.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Booking.Business.Persistence.Context;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<Table> Tables { get; set; } = null!;
    public DbSet<Reservation> Reservations { get; set; } = null!;
}
