using AutoMapper;
using Booking.Business.Application.Exceptions;
using Booking.Business.Application.Repositories;
using MassTransit;
using Otus.Booking.Common.Booking.Contracts.Reservation.Requests;
using Otus.Booking.Common.Booking.Contracts.Reservation.Responses;

namespace Booking.Business.Application.Consumers.Reservation;

public class UpdateReservationConsumer : IConsumer<UpdateReservation>
{
    private readonly IReservationRepository _reservationRepository;
    private readonly ITableRepository _tableRepository;
    private readonly IMapper _mapper;

    public UpdateReservationConsumer(IReservationRepository reservationRepository, ITableRepository tableRepository, IMapper mapper)
    {
        _reservationRepository = reservationRepository;
        _tableRepository = tableRepository;
        _mapper = mapper;
    }
    
    public async Task Consume(ConsumeContext<UpdateReservation> context)
    {
        var request = context.Message;
        var reservation = await _reservationRepository.FindByIdAsync(request.Id);
        
        if (!await _reservationRepository.HasAnyByIdAsync(request.Id))
            throw new NotFoundException($"Reservation with ID {request.TableId} doesn't exists");
        
        if (!await _tableRepository.HasAnyByIdAsync(request.TableId))
            throw new NotFoundException($"Table with ID {request.TableId} doesn't exists");
        
        _mapper.Map(request, reservation);
        
        await _reservationRepository.UpdateAsync(reservation!);

        await context.RespondAsync(_mapper.Map<UpdateReservationResult>(reservation));
    }
}