namespace Booking.Application.Features.TableFeatures;

public class CreateTableResponse
{
    public Guid Id { get; set; }
    public Guid FilialId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int SeatsNumber { get; set; }
}