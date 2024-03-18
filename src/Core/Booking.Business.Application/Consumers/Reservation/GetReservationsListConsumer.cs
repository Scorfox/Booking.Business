using AutoMapper;
using Booking.Business.Application.Repositories;
using MassTransit;
using Otus.Booking.Common.Booking.Contracts.Reservation.Models;
using Otus.Booking.Common.Booking.Contracts.Reservation.Requests;
using Otus.Booking.Common.Booking.Contracts.Reservation.Responses;

namespace Booking.Business.Application.Consumers.Reservation;

public class GetReservationsListConsumer:IConsumer <GetReservationsList>
{
    private readonly IMapper _mapper;
    private readonly IReservationRepository _reservationRepository;

    public GetReservationsListConsumer(IMapper mapper, IReservationRepository reservationRepository)
    {
        _mapper = mapper;
        _reservationRepository = reservationRepository;
    }

    public async Task Consume(ConsumeContext<GetReservationsList> context)
    {
        var request = context.Message;

        var reservations = await _reservationRepository.GetPaginatedListAsync(request.Offset, request.Count);
        
        await context.RespondAsync(new GetReservationsListResult
        {
            Elements = _mapper.Map<List<ReservationGettingDto>>(reservations), 
            TotalCount = await _reservationRepository.GetTotalCount() 
        });
    }
}