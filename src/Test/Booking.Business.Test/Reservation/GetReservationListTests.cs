using AutoMapper;
using Booking.Business.Application.Consumers.Reservation;
using Booking.Business.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Booking.Business.Application.Mappings;
using AutoFixture;
using MassTransit.Testing;
using Otus.Booking.Common.Booking.Contracts.Filial.Requests;
using Otus.Booking.Common.Booking.Contracts.Filial.Responses;
using Otus.Booking.Common.Booking.Contracts.Reservation.Requests;
using Otus.Booking.Common.Booking.Contracts.Reservation.Responses;

namespace Booking.Business.Test.Reservation
{
    public class GetReservationListTests : BaseTest
    {
        private GetReservationsListConsumer Consumer { get; }

        public GetReservationListTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<ReservationMapper>());

            var reservationRepository = new ReservationRepository(DataContext);
            
            Consumer = new GetReservationsListConsumer(reservationRepository, new Mapper(config));
        }

        [Test]
        public async Task GetReservationList()
        {
            for (int i = 0; i < 5; i++)
            {
                var reservation = Fixture.Build<Domain.Entities.Reservation>().Without(e => e.Table).Create();

                await DataContext.Reservations.AddAsync(reservation);
            }

            await DataContext.SaveChangesAsync();
            var testHarness = new InMemoryTestHarness();
            var consumerHarness = testHarness.Consumer(() => Consumer);

            await testHarness.Start();

            var request = Fixture.Create<GetReservationsList>();
            request.Offset = 0;
            request.Count = 3;
            // Act
            await testHarness.InputQueueSendEndpoint.Send(request);
            var result = testHarness.Published.Select<GetReservationsListResult>().FirstOrDefault()?.Context.Message;

            Assert.Multiple(() =>
            {
                Assert.That(testHarness.Consumed.Select<GetReservationsList>().Any(), Is.True);
                Assert.That(consumerHarness.Consumed.Select<GetReservationsList>().Any(), Is.True);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Elements.Count, Is.EqualTo(3));
            });

            await testHarness.Stop();
        }
    }
}
