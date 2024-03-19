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
        Consumer = new UpdateReservationConsumer(new Mapper(config), reservationRepository, tableRepository);
    }

    [Test]
    public async Task UpdateReservation_ReturnsSuccess()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var reservation = Fixture.Build<Domain.Entities.Reservation>()
            .With(e => e.Table, 
            Fixture.Build<Domain.Entities.Table>().Without(e => e.Reservations).Create)
            .Create();
        
        reservation.Table.CompanyId = companyId;
        await DataContext.Reservations.AddAsync(reservation);
        await DataContext.SaveChangesAsync();
        
        var request = Fixture.Create<UpdateReservation>();
        request.Id = reservation.Id;
        request.CompanyId = reservation.Table.CompanyId;
        request.TableId = reservation.TableId;
        
        var testHarness = new InMemoryTestHarness();
        testHarness.Consumer(() => Consumer);
        
        await testHarness.Start(); 
        
        // Act
        await testHarness.InputQueueSendEndpoint.Send(request);
        var result = testHarness.Published.Select<UpdateReservationResult>().FirstOrDefault()?.Context.Message;
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(testHarness.Consumed.Select<UpdateReservation>().Any(), Is.True);
            Assert.That(testHarness.Published.Select<UpdateReservationResult>().Any(), Is.True);
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
        testHarness.Consumer(() => Consumer);

        var request = Fixture.Create<UpdateReservation>();
        
        await testHarness.Start(); 
        
        // Act
        await testHarness.InputQueueSendEndpoint.Send(request);
       
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(testHarness.Consumed.Select<UpdateReservation>().Any(), Is.True);
            Assert.That(testHarness.Published.Select<Fault>().FirstOrDefault(), Is.Not.Null);
        });
        
        await testHarness.Stop();
    }
}