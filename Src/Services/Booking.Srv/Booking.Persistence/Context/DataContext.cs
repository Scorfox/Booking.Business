using Booking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Booking.Application.Context;
public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        Database.EnsureDeleted();
        Database.EnsureCreated();
    }
    
    public DbSet<Table> Tables { get; set; } = null!;
    public DbSet<Reservation> Reservations { get; set; } = null!;
}