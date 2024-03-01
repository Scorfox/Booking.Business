using AutoFixture;
using AutoMapper;
using Booking.Business.Application.Consumers.Reservation;
using Booking.Business.Application.Mappings;
using Booking.Business.Persistence.Repositories;
using MassTransit.Testing;
using Otus.Booking.Common.Booking.Contracts.Filial.Requests;
using Otus.Booking.Common.Booking.Contracts.Filial.Responses;
using Otus.Booking.Common.Booking.Contracts.Reservation.Requests;
using Otus.Booking.Common.Booking.Contracts.Reservation.Responses;

namespace Booking.Business.Test.Reservation
{
    public class DeleteReservationTests : BaseTest
    {
        private DeleteReservationConsumer Consumer { get; }

        public DeleteReservationTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<ReservationMapper>());

            var reservationRepository = new ReservationRepository(DataContext);

            Consumer = new DeleteReservationConsumer(reservationRepository, new Mapper(config));
        }

        [Test]
        public async Task DeleteReservationTest()
        {
            var reservation = Fixture.Build<Domain.Entities.Reservation>().Without(e => e.Table).Create();
            await DataContext.Reservations.AddAsync(reservation);

            await DataContext.SaveChangesAsync();
            var testHarness = new InMemoryTestHarness();
            var consumerHarness = testHarness.Consumer(() => Consumer);

            await testHarness.Start();

            var request = Fixture.Create<DeleteReservation>();
            request.Id = reservation.Id;

            // Act
            await testHarness.InputQueueSendEndpoint.Send(request);
            var result = testHarness.Published.Select<DeleteReservationResult>().FirstOrDefault()?.Context.Message;

            Assert.Multiple(() =>
            {
                Assert.That(testHarness.Consumed.Select<DeleteReservation>().Any(), Is.True);
                Assert.That(consumerHarness.Consumed.Select<DeleteReservation>().Any(), Is.True);
            });

            await testHarness.Stop();
        }
    }
}
