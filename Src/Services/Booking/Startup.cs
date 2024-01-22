using Booking.Objects;
using Microsoft.EntityFrameworkCore;

namespace Booking;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    
    public IConfiguration Configuration { get; }
    
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMvc();
        services.AddControllers();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        
        services.AddDbContext<BookingDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")), 
            ServiceLifetime.Transient);
    }
    public void Configure(IApplicationBuilder app)
    {
        //Write code here to configure the request processing pipeline
    }
    //Other members have been removed for brevity
}