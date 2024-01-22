using Base.Objects;
using Booking.Objects.Models;

namespace Booking.Objects;

public class Reservation : Entity
{
    /// <summary>
    /// Идентификатор стола
    /// </summary>
    public Guid TableId { get; set; }
    /// <summary>
    /// Начало бронирования
    /// </summary>
    public DateTimeOffset StartedAt { get; set; }
    /// <summary>
    /// Конец бронирования
    /// </summary>
    public DateTimeOffset FinishedAt { get; set; }
    /// <summary>
    /// Идентификатор пользователя, отправившего заявку на бронирование
    /// </summary>
    public Guid WhoBookedId { get; set; }
    /// <summary>
    /// Идентификатор пользователя, подтвердившего бронирование
    /// </summary>
    public Guid WhoConfirmedId { get; set; }
    /// <summary>
    /// Идентификатор пользователя, отменившего бронирование
    /// </summary>
    public Guid? WhoCancelledId { get; set; }
    /// <summary>
    /// Навигационное свойство на столик
    /// </summary>
    public Table Table { get; set; }
}