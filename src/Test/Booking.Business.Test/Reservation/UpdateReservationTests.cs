using AutoFixture;
using AutoMapper;
using Booking.Business.Application.Consumers.Reservation;
using Booking.Business.Application.Mappings;
using Booking.Business.Persistence.Repositories;
using MassTransit;
using MassTransit.Testing;
using Otus.Booking.Common.Booking.Contracts.Reservation.Requests;
using Otus.Booking.Common.Booking.Contracts.Reservation.Responses;

namespace Booking.Business.Test.Reservation;

public class UpdateReservationTests : BaseTest
{
    private UpdateReservationConsumer Consumer { get; }
    
    public UpdateReservationTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<ReservationMapper>());
        
        var reservationRepository = new ReservationRepository(DataContext);
        var tableRepository = new TableRepository(DataContext);
        Consumer = new UpdateReservationConsumer(reservationRepository, tableRepository, new Mapper(config));
    }

    [Test]
    public async Task UpdateReservation_ReturnsSuccess()
    {
        // Arrange
        var reservation = Fixture.Build<Domain.Entities.Reservation>()
            .With(e => e.Table, 
            Fixture.Build<Domain.Entities.Table>().Without(e => e.Reservations).Create)
            .Create();
        await DataContext.Reservations.AddAsync(reservation);
        await DataContext.SaveChangesAsync();
        
        var request = Fixture.Create<UpdateReservation>();
        request.Id = reservation.Id;
        request.TableId = reservation.TableId;
        
        var testHarness = new InMemoryTestHarness();
        var consumerHarness = testHarness.Consumer(() => Consumer);
        
        await testHarness.Start(); 
        
        // Act
        await testHarness.InputQueueSendEndpoint.Send(request);
        var result = testHarness.Published.Select<UpdateReservationResult>().FirstOrDefault()?.Context.Message;
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(testHarness.Consumed.Select<UpdateReservation>().Any(), Is.True);
            Assert.That(consumerHarness.Consumed.Select<UpdateReservation>().Any(), Is.True);
            Assert.That(reservation.From, Is.EqualTo(result.From));
            Assert.That(reservation.To, Is.EqualTo(result.To));
            Assert.That(reservation.WhoBookedId, Is.EqualTo(result.WhoBookedId));
        });
        
        await testHarness.Stop();
    }

    [Test]
    public async Task UpdateNotCreatedReservation_ReturnsException()
    {
        // Arrange
        var testHarness = new InMemoryTestHarness();
        var consumerHarness = testHarness.Consumer(() => Consumer);

        var request = Fixture.Create<UpdateReservation>();
        
        await testHarness.Start(); 
        
        // Act
        await testHarness.InputQueueSendEndpoint.Send(request);
       
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(testHarness.Published.Select<Fault>().FirstOrDefault(), Is.Not.Null);
            Assert.That(consumerHarness.Consumed.Select<UpdateReservation>().Any(), Is.True);
        });
        
        await testHarness.Stop();
    }
}