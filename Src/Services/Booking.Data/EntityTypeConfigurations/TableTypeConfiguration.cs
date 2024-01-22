using Booking.Objects.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Objects.EntityTypeConfigurations;

public class TableTypeConfiguration : IEntityTypeConfiguration<Table>
{
    public void Configure(EntityTypeBuilder<Table> builder)
    {
        builder
            .HasMany<Reservation>(e => e.Reservations)
            .WithOne(e => e.Table)
            .HasForeignKey(e => e.TableId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}