using Booking.Business.Application.Repositories;
using Booking.Business.Persistence.Context;
using Booking.Business.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Booking.Business.Persistence;

public static class ServiceExtensions
{
    public static void ConfigurePersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("PostgreSQL");
        
        services.AddDbContext<DataContext>(opt => { opt.UseNpgsql(connectionString); });
        services.AddTransient<ITableRepository, TableRepository>();
        services.AddTransient<IReservationRepository, ReservationRepository>();
    }
}