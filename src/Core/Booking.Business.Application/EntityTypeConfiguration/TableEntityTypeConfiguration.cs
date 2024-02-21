using Booking.Business.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Business.Application.EntityTypeConfiguration;

public class TableEntityTypeConfiguration : IEntityTypeConfiguration<Table>
{
    public void Configure(EntityTypeBuilder<Table> builder)
    {
        builder.HasIndex(i => i.Name)
            .IsUnique();

        builder.HasMany<Reservation>()
            .WithOne(r => r.Table)
            .HasForeignKey(e => e.TableId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}