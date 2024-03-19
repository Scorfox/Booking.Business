using AutoMapper;
using Booking.Business.Application.Consumers.Reservation;
using Booking.Business.Persistence.Repositories;
using Booking.Business.Application.Mappings;
using AutoFixture;
using MassTransit.Testing;
using Otus.Booking.Common.Booking.Contracts.Reservation.Requests;
using Otus.Booking.Common.Booking.Contracts.Reservation.Responses;

namespace Booking.Business.Test.Reservation;

public class GetReservationListTests : BaseTest
{
    private GetReservationsListConsumer Consumer { get; }

    public GetReservationListTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<ReservationMapper>());

        var reservationRepository = new ReservationRepository(DataContext);
            
        Consumer = new GetReservationsListConsumer(new Mapper(config), reservationRepository);
    }

    [Test]
    public async Task GetReservationList()
    {
        // Arrange
        var filialId = Guid.NewGuid();
        var reservations = new List<Domain.Entities.Reservation>();
        
        for (var i = 0; i < 5; i++)
            reservations.Add(Fixture.Build<Domain.Entities.Reservation>()
                .Without(e => e.Table)
                .Create());

        var tableWithReservations = Fixture.Build<Domain.Entities.Table>()
            .With(e => e.Reservations, reservations)
            .With(e => e.FilialId, filialId)
            .Create();
            
        await DataContext.Tables.AddAsync(tableWithReservations);
        await DataContext.SaveChangesAsync();
            
        var testHarness = new InMemoryTestHarness();
        testHarness.Consumer(() => Consumer);

        await testHarness.Start();

        var request = Fixture.Build<GetReservationsList>()
            .With(e => e.Offset, 0)
            .With(e => e.Count, 3)
            .With(e => e.FilialId, filialId)
            .Create();
        
        // Act
        await testHarness.InputQueueSendEndpoint.Send(request);
        var result = testHarness.Published.Select<GetReservationsListResult>().FirstOrDefault()?.Context.Message;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(testHarness.Consumed.Select<GetReservationsList>().Any(), Is.True);
            Assert.That(testHarness.Published.Select<GetReservationsListResult>().Any(), Is.True);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Elements.Count, Is.EqualTo(3));
        });

        await testHarness.Stop();
    }
}