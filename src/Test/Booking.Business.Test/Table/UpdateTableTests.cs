using AutoFixture;
using AutoMapper;
using Booking.Business.Application.Consumers.Table;
using Booking.Business.Application.Mappings;
using Booking.Business.Persistence.Repositories;
using MassTransit;
using MassTransit.Testing;
using Otus.Booking.Common.Booking.Contracts.Table.Requests;
using Otus.Booking.Common.Booking.Contracts.Table.Responses;

namespace Booking.Business.Test.Table;

public class UpdateTableTests : BaseTest
{
    private UpdateTableConsumer Consumer { get; }
    
    public UpdateTableTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<TableMapper>());
        var companyRepository = new TableRepository(DataContext);
        
        Consumer = new UpdateTableConsumer(companyRepository, new Mapper(config));
    }

    [Test]
    public async Task UpdateTable_ReturnsSuccess()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var table = Fixture.Build<Domain.Entities.Table>()
            .Without(e => e.Reservations)
            .With(e => e.CompanyId, companyId)
            .Create();
        await DataContext.Tables.AddAsync(table);
        await DataContext.SaveChangesAsync();

        var request = Fixture.Build<UpdateTable>()
            .With(e => e.Id, table.Id)
            .With(e => e.CompanyId, table.CompanyId)
            .Create();
        
        var testHarness = new InMemoryTestHarness();
        testHarness.Consumer(() => Consumer);
        await testHarness.Start(); 
        
        // Act
        await testHarness.InputQueueSendEndpoint.Send(request);
        var result = testHarness.Published.Select<UpdateTableResult>().FirstOrDefault()?.Context.Message;
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(testHarness.Consumed.Select<UpdateTable>().Any(), Is.True);
            Assert.That(testHarness.Published.Select<UpdateTableResult>().Any(), Is.True);
            Assert.That(table.Name.ToLower(), Is.EqualTo(result?.Name.ToLower()));
        });
        
        await testHarness.Stop();
    }

    [Test]
    public async Task UpdateNotCreatedTable_ReturnsException()
    {
        // Arrange
        var request = Fixture.Create<UpdateTable>();
        
        var testHarness = new InMemoryTestHarness();
        testHarness.Consumer(() => Consumer);
        await testHarness.Start(); 
        
        // Act
        await testHarness.InputQueueSendEndpoint.Send(request);
       
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(testHarness.Consumed.Select<UpdateTable>().Any(), Is.True);
            Assert.That(testHarness.Published.Select<Fault>().FirstOrDefault(), Is.Not.Null);
        });
        
        await testHarness.Stop();
    }
}