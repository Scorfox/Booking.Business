using Booking.Objects.Models;
using Microsoft.EntityFrameworkCore;

namespace Booking.Objects;

public sealed class BookingDbContext : DbContext
{
    public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
    
    public DbSet<Table> Tables { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
}