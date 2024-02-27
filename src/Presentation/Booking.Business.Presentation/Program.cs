//#define dds_tests
using Booking.Business.Application;
using Booking.Business.Application.Consumers.Reservation;
using Booking.Business.Application.Consumers.Table;
using Booking.Business.Persistence;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureApplication();
builder.Services.ConfigurePersistence(builder.Configuration);

builder.Services.AddMassTransit(x =>
{
    // Добавляем шину сообщений
    x.UsingRabbitMq((context, cfg) =>
    {
#if true
        cfg.Host(builder.Configuration["RabbitMQdds:Host"], h =>
        {
            h.Username(builder.Configuration["RabbitMQdds:Username"]);
            h.Password(builder.Configuration["RabbitMQdds:Password"]);
        });
#else
        cfg.Host(builder.Configuration["RabbitMQ:Host"], h =>
        {
            h.Username(builder.Configuration["RabbitMQ:Username"]);
            h.Password(builder.Configuration["RabbitMQ:Password"]);
        });
#endif

        cfg.ConfigureEndpoints(context);

    });
        // Table
        x.AddConsumer<CreateTableConsumer>();
    x.AddConsumer<UpdateTableConsumer>();
    
    // Reservation
    x.AddConsumer<CreateReservationConsumer>();
    x.AddConsumer<UpdateReservationConsumer>();
});

var app = builder.Build();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
app.Run();