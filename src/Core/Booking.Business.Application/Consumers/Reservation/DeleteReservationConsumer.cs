using AutoMapper;
using Booking.Business.Application.Repositories;
using MassTransit;
using Otus.Booking.Common.Booking.Contracts.Reservation.Requests;
using Otus.Booking.Common.Booking.Contracts.Reservation.Responses;
using Otus.Booking.Common.Booking.Contracts.Table.Requests;

namespace Booking.Business.Application.Consumers.Reservation
{
    public class DeleteReservationConsumer : IConsumer<DeleteReservation>
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IMapper _mapper;

        public DeleteReservationConsumer(IReservationRepository reservationRepository, IMapper mapper)
        {
            _reservationRepository = reservationRepository;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<DeleteReservation> context)
        {
            var request = context.Message;

            await _reservationRepository.DeleteReservation(request.Id);

            await context.RespondAsync(new DeleteReservationResult());
        }
    }
}
