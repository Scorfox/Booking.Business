using AutoFixture;
using Booking.Business.Persistence.Repositories;
using MassTransit.Testing;
using Booking.Business.Application.Consumers.Table;
using Otus.Booking.Common.Booking.Contracts.Table.Requests;
using Otus.Booking.Common.Booking.Contracts.Table.Responses;

namespace Booking.Business.Test.Table;

internal class DeleteTableTests : BaseTest
{
    private DeleteTableConsumer Consumer { get; }

    public DeleteTableTests()
    {
        var reservationRepository = new TableRepository(DataContext);

        Consumer = new DeleteTableConsumer(reservationRepository);
    }

    [Test]
    public async Task DeleteTable()
    {
        // Arrange
        var table = Fixture.Build<Domain.Entities.Table>().Without(e => e.Reservations).Create();
        await DataContext.Tables.AddAsync(table);
        await DataContext.SaveChangesAsync();
        
        var testHarness = new InMemoryTestHarness();
        testHarness.Consumer(() => Consumer);

        await testHarness.Start();

        var request = Fixture.Build<DeleteTable>()
            .With(e => e.Id, table.Id)
            .With(e => e.CompanyId, table.CompanyId)
            .Create();

        // Act
        await testHarness.InputQueueSendEndpoint.Send(request);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(testHarness.Consumed.Select<DeleteTable>().Any(), Is.True);
            Assert.That(testHarness.Published.Select<DeleteTableResult>().Any(), Is.True);
        });

        await testHarness.Stop();
    }
}