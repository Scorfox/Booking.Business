using Booking.Business.Application.Repositories;
using Booking.Business.Domain.Entities;
using Booking.Business.Persistence.Context;

namespace Booking.Business.Persistence.Repositories;

public class ReservationRepository(DataContext context) : BaseRepository<Reservation>(context), IReservationRepository;