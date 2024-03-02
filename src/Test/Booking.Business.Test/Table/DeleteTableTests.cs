using AutoFixture;
using AutoMapper;
using Booking.Business.Application.Consumers.Reservation;
using Booking.Business.Persistence.Repositories;
using MassTransit.Testing;
using Otus.Booking.Common.Booking.Contracts.Reservation.Requests;
using Otus.Booking.Common.Booking.Contracts.Reservation.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Booking.Business.Application.Consumers.Table;
using Booking.Business.Application.Mappings;
using Otus.Booking.Common.Booking.Contracts.Table.Requests;
using Otus.Booking.Common.Booking.Contracts.Table.Responses;

namespace Booking.Business.Test.Table
{
    internal class DeleteTableTests : BaseTest
    {
        private DeleteTableConsumer Consumer { get; }

        public DeleteTableTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<TableMapper>());

            var reservationRepository = new TableRepository(DataContext);

            Consumer = new DeleteTableConsumer(reservationRepository);
        }

        [Test]
        public async Task DeleteTable()
        {
            var table = Fixture.Build<Domain.Entities.Table>().Without(e => e.Reservations).Create();
            await DataContext.Tables.AddAsync(table);

            await DataContext.SaveChangesAsync();
            var testHarness = new InMemoryTestHarness();
            var consumerHarness = testHarness.Consumer(() => Consumer);

            await testHarness.Start();

            var request = Fixture.Create<DeleteTable>();
            request.Id = table.Id;

            // Act
            await testHarness.InputQueueSendEndpoint.Send(request);
            var result = testHarness.Published.Select<DeleteTableResult>().FirstOrDefault()?.Context.Message;

            Assert.Multiple(() =>
            {
                Assert.That(testHarness.Consumed.Select<DeleteTable>().Any(), Is.True);
                Assert.That(consumerHarness.Consumed.Select<DeleteTable>().Any(), Is.True);
            });

            await testHarness.Stop();
        }
    }
}
