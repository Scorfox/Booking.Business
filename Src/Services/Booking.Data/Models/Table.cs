using Base.Objects;

namespace Booking.Objects.Models;

public class Table : Entity
{
    /// <summary>
    /// Идентификатор филиала
    /// </summary>
    public Guid FilialId { get; set; }
    /// <summary>
    /// Наименование стола
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Описание
    /// </summary>
    public string? Description { get; set; }
    /// <summary>
    /// Количество мест
    /// </summary>
    public int SeatsNumber { get; set; }
    
    /// <summary>
    /// Навигационное поле на бронирования
    /// </summary>
    public List<Reservation> Reservations { get; set; }
}