using Booking.Objects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Booking.Controllers;

[ApiController]
[Route("[controller]")]
public class TableController : ControllerBase
{
    private BookingDbContext _dbContext;
    
    public TableController(BookingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpPost("add")]
    public async Task<bool> AddTable()
    {
        var table = new Objects.Models.Table
        {
            FilialId = Guid.NewGuid(),
            Name = "name",
            SeatsNumber = 3
        };
        
        await _dbContext.AddAsync(table);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}