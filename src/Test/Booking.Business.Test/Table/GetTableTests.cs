using AutoFixture;
using AutoMapper;
using Booking.Business.Application.Consumers.Table;
using Booking.Business.Application.Mappings;
using Booking.Business.Persistence.Repositories;
using MassTransit.Testing;
using Otus.Booking.Common.Booking.Contracts.Filial.Requests;
using Otus.Booking.Common.Booking.Contracts.Table.Requests;

namespace Booking.Business.Test.Table
{
    public class GetTableTests : BaseTest
    {
        private GetTableConsumer Consumer { get; }

        public GetTableTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<TableMapper>());

            var tableRepository = new TableRepository(DataContext);
            Consumer = new GetTableConsumer(tableRepository, new Mapper(config));
        }

        [Test]
        public async Task GetTableByIdTest_ReturnsSuccess()
        {
            // Arrange
            var testHarness = new InMemoryTestHarness();
            var consumerHarness = testHarness.Consumer(() => Consumer);
            Guid id = Guid.NewGuid();

            var request = Fixture.Create<GetTableId>();
            request.Id = id;

            await testHarness.Start();

            // Act
            await testHarness.InputQueueSendEndpoint.Send(request);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(testHarness.Consumed.Select<GetTableId>().Any(), Is.True);
                Assert.That(consumerHarness.Consumed.Select<GetTableId>().Any(), Is.True);
            });

            await testHarness.Stop();
        }
    }
}
