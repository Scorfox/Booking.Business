using AutoFixture;
using AutoMapper;
using Booking.Business.Application.Consumers.Table;
using Booking.Business.Application.Mappings;
using Booking.Business.Persistence.Repositories;
using MassTransit;
using MassTransit.Testing;
using Microsoft.EntityFrameworkCore;
using Otus.Booking.Common.Booking.Contracts.Table.Requests;

namespace Booking.Business.Test.Table;

public class CreateTableTests : BaseTest
{
    private CreateTableConsumer Consumer { get; }
    
    public CreateTableTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<TableMapper>());
        
        var tableRepository = new TableRepository(DataContext);
        Consumer = new CreateTableConsumer(tableRepository, new Mapper(config));
    }

    [Test]
    public async Task CreateTable_ReturnsSuccess()
    {
        // Arrange
        var testHarness = new InMemoryTestHarness();
        var consumerHarness = testHarness.Consumer(() => Consumer);
        
        await testHarness.Start(); 
        
        // Act
        await testHarness.InputQueueSendEndpoint.Send(Fixture.Create<CreateTable>());
       
        // Assert
        Assert.Multiple(async () =>
        {
            Assert.That(testHarness.Consumed.Select<CreateTable>().Any(), Is.True);
            Assert.That(consumerHarness.Consumed.Select<CreateTable>().Any(), Is.True);
            Assert.That(await DataContext.Tables.AnyAsync(), Is.True);
        });
        
        await testHarness.Stop();
    }

    [Test]
    public async Task CreateTable_NotUnique_ReturnsException()
    {
        // Arrange
        var testHarness = new InMemoryTestHarness();
        var consumerHarness = testHarness.Consumer(() => Consumer);
        
        var table = Fixture.Build<Domain.Entities.Table>().Without(e => e.Reservations).Create();

        await DataContext.Tables.AddAsync(table);
        await DataContext.SaveChangesAsync();

        var request = Fixture.Create<CreateTable>();
        request.FilialId = table.FilialId;
        request.Name = table.Name;
        
        await testHarness.Start(); 
        
        // Act
        await testHarness.InputQueueSendEndpoint.Send(request);
       
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(testHarness.Published.Select<Fault>().FirstOrDefault(), Is.Not.Null);
            Assert.That(consumerHarness.Consumed.Select<CreateTable>().Any(), Is.True);
        });
        
        await testHarness.Stop();
    }
}