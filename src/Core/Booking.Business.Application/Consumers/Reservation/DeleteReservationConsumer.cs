using Booking.Business.Application.Repositories;
using MassTransit;
using Otus.Booking.Common.Booking.Contracts.Reservation.Requests;
using Otus.Booking.Common.Booking.Contracts.Reservation.Responses;

namespace Booking.Business.Application.Consumers.Reservation
{
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

            await _reservationRepository.DeleteReservation(request.Id);

            await context.RespondAsync(new DeleteReservationResult());
        }
    }
}
