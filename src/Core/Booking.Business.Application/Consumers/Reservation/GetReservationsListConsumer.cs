using AutoMapper;
using Booking.Business.Application.Repositories;
using MassTransit;
using Otus.Booking.Common.Booking.Contracts.Reservation.Models;
using Otus.Booking.Common.Booking.Contracts.Reservation.Requests;
using Otus.Booking.Common.Booking.Contracts.Reservation.Responses;

namespace Booking.Business.Application.Consumers.Reservation;

public class GetReservationsListConsumer:IConsumer <GetReservationsList>
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IMapper _mapper;

    public GetReservationsListConsumer(IReservationRepository reservationRepository, IMapper mapper)
    {
        _reservationRepository = reservationRepository;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<GetReservationsList> context)
    {
        var request = context.Message;

        var reservations = await _reservationRepository.GetPaginatedListAsync(request.Offset, request.Count);
        var totalCount = await _reservationRepository.GetTotalCount();
        await context.RespondAsync(new GetReservationsListResult
        {
            Elements = _mapper.Map<List<FullReservationDto>>(reservations), 
            TotalCount = totalCount 
        });
    }
}