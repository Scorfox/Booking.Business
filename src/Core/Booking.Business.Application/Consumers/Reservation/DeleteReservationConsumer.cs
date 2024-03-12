using Booking.Business.Application.Repositories;
using MassTransit;
using Otus.Booking.Common.Booking.Contracts.Reservation.Requests;
using Otus.Booking.Common.Booking.Contracts.Reservation.Responses;
using Otus.Booking.Common.Booking.Exceptions;

namespace Booking.Business.Application.Consumers.Reservation;

public class DeleteReservationConsumer : IConsumer<DeleteReservation>
{
    private readonly IReservationRepository _reservationRepository;

    public DeleteReservationConsumer(IReservationRepository reservationRepository)
    {
        _reservationRepository = reservationRepository;
    }

    public async Task Consume(ConsumeContext<DeleteReservation> context)
    {
        var request = context.Message;

        var reservation = await _reservationRepository.FindByIdAsync(request.Id);
        
        if (reservation == null)
            throw new NotFoundException($"Reservation with ID {request.Id} doesn't exists");
        
        await _reservationRepository.Delete(reservation);

        await context.RespondAsync(new DeleteReservationResult());
    }
}