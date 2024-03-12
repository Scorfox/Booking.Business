﻿using Booking.Business.Domain.Entities;

namespace Booking.Business.Application.Repositories;

public interface IReservationRepository : IBaseRepository<Reservation>
{
    public Task DeleteReservation(Guid id, CancellationToken cancellationToken = default);
}