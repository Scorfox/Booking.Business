using Booking.Business.Application.Consumers.Reservation;
using Booking.Business.Application.Consumers.Table;
using Booking.Business.Application;
using Booking.Business.Persistence;
using MassTransit;

namespace Booking.Presentation
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigurePersistence(Configuration);
            services.ConfigureApplication(Configuration);

            services.AddControllers();
            services.AddEndpointsApiExplorer();

            services.AddMassTransit(x =>
            {
                // Добавляем шину сообщений
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(Configuration["RabbitMQ:Host"], h =>
                    {
                        h.Username(Configuration["RabbitMQ:Username"]);
                        h.Password(Configuration["RabbitMQ:Password"]);
                    });

                    cfg.ConfigureEndpoints(context);

                });

                // Table
                x.AddConsumer<CreateTableConsumer>();
                x.AddConsumer<GetTableConsumer>();
                x.AddConsumer<GetTablesListConsumer>();
                x.AddConsumer<UpdateTableConsumer>();
                x.AddConsumer<DeleteTableConsumer>();

                // Reservation
                x.AddConsumer<CreateReservationConsumer>();
                x.AddConsumer<GetReservationConsumer>();
                x.AddConsumer<GetReservationsListConsumer>();
                x.AddConsumer<UpdateReservationConsumer>();

                x.AddConsumer<CancelReservationConsumer>();
                x.AddConsumer<ConfirmReservationConsumer>();
            });
        }

        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            app.UseCors();
            app.MapControllers();
        }
    }
}
