using AutoMapper;
using Booking.Business.Application.Exceptions;
using Booking.Business.Application.Repositories;
using MassTransit;
using Otus.Booking.Common.Booking.Contracts.Reservation.Requests;

namespace Booking.Business.Application.Consumers.Reservation
{
    public sealed class GetReservationConsumer : IConsumer<GetReservationId>
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IMapper _mapper;

        public GetReservationConsumer(IReservationRepository reservationRepository, IMapper mapper)
        {
            _reservationRepository = reservationRepository;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<GetReservationId> context)
        {
            var request = context.Message;

            if (!await _reservationRepository.HasAnyByIdAsync(request.Id))
                throw new NotFoundException($"Reservation with ID {request.Id} doesn't exists");

            await context.RespondAsync(_reservationRepository.FindByIdAsync(request.Id, default));
        }
    }
}
}
