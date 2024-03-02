using AutoMapper;
using Booking.Business.Application.Consumers.Reservation;
using Booking.Business.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Booking.Business.Application.Consumers.Table;
using Booking.Business.Application.Mappings;
using AutoFixture;
using MassTransit.Testing;
using Otus.Booking.Common.Booking.Contracts.Reservation.Requests;
using Otus.Booking.Common.Booking.Contracts.Reservation.Responses;
using Otus.Booking.Common.Booking.Contracts.Table.Requests;
using Otus.Booking.Common.Booking.Contracts.Table.Responses;

namespace Booking.Business.Test.Table
{
    public class GetTablesListTests : BaseTest
    {
        private GetTablesListConsumer Consumer { get; }

        public GetTablesListTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<TableMapper>());

            var reservationRepository = new TableRepository(DataContext);

            Consumer = new GetTablesListConsumer(reservationRepository, new Mapper(config));
        }

        [Test]
        public async Task GetTablesList()
        {
            for (int i = 0; i < 5; i++)
            {
                var table = Fixture.Build<Domain.Entities.Table>().Without(e => e.Reservations).Create();

                await DataContext.Tables.AddAsync(table);
            }

            await DataContext.SaveChangesAsync();
            var testHarness = new InMemoryTestHarness();
            var consumerHarness = testHarness.Consumer(() => Consumer);

            await testHarness.Start();

            var request = Fixture.Create<GetTablesList>();
            request.Offset = 0;
            request.Limit = 3;
            // Act
            await testHarness.InputQueueSendEndpoint.Send(request);
            var result = testHarness.Published.Select<GetTablesListResult>().FirstOrDefault()?.Context.Message;

            Assert.Multiple(() =>
            {
                Assert.That(testHarness.Consumed.Select<GetTablesList>().Any(), Is.True);
                Assert.That(consumerHarness.Consumed.Select<GetTablesList>().Any(), Is.True);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Tables.Count, Is.EqualTo(3));
            });

            await testHarness.Stop();
        }
    }
}
