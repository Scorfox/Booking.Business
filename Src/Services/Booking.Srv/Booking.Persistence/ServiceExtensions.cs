using System.Reflection;
using Booking.Application.Context;
using Booking.Application.Repositories;
using Booking.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Booking.Persistence;

public static class ServiceExtensions
{
    public static void ConfigurePersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("PostgreSQL");
        
        services.AddDbContext<DataContext>(opt => { opt.UseNpgsql(connectionString); });
        services.AddScoped<IReservationRepository, ReservationRepository>();
        services.AddScoped<ITableRepository, TableRepository>();
    }
}
