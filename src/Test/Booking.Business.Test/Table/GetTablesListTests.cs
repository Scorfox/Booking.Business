using AutoMapper;
using Booking.Business.Persistence.Repositories;
using Booking.Business.Application.Consumers.Table;
using Booking.Business.Application.Mappings;
using AutoFixture;
using MassTransit.Testing;
using Otus.Booking.Common.Booking.Contracts.Table.Requests;
using Otus.Booking.Common.Booking.Contracts.Table.Responses;

namespace Booking.Business.Test.Table;

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
        // Arrange
        const int tableFromRequestCount = 3;
        var tables = new List<Domain.Entities.Table>();
        var filialId = Guid.NewGuid();
        for (var i = 0; i < 5; i++)
            tables.Add(Fixture.Build<Domain.Entities.Table>()
                .With(e => e.FilialId, filialId)
                .Without(e => e.Reservations)
                .Create());

        await DataContext.AddRangeAsync(tables);
        await DataContext.SaveChangesAsync();
        
        var testHarness = new InMemoryTestHarness();
        testHarness.Consumer(() => Consumer);
        await testHarness.Start();

        var request = Fixture.Build<GetTablesList>()
            .With(e => e.Offset, 0)
            .With(e => e.Count, tableFromRequestCount)
            .With(e => e.FilialId, filialId)
            .Create();
            
        // Act
        await testHarness.InputQueueSendEndpoint.Send(request);
        var result = testHarness.Published.Select<GetTablesListResult>().FirstOrDefault()?.Context.Message;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(testHarness.Consumed.Select<GetTablesList>().Any(), Is.True);
            Assert.That(testHarness.Published.Select<GetTablesListResult>().Any(), Is.True);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Elements.Count, Is.EqualTo(tableFromRequestCount));
        });

        await testHarness.Stop();
    }
}