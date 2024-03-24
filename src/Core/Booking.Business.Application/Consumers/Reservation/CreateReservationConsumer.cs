using AutoMapper;
using Booking.Business.Application.Repositories;
using Booking.Business.Application.Services;
using MassTransit;
using Otus.Booking.Common.Booking.Contracts.Reservation.Requests;
using Otus.Booking.Common.Booking.Contracts.Reservation.Responses;
using Otus.Booking.Common.Booking.Exceptions;

namespace Booking.Business.Application.Consumers.Reservation;

public class CreateReservationConsumer : IConsumer<CreateReservation>
{
    private readonly BookingLog _bookingLog;
    private readonly IMapper _mapper;
    private readonly ITableRepository _tableRepository;
    private readonly IReservationRepository _reservationRepository;

    public CreateReservationConsumer(
        BookingLog bookingLog,
        IMapper mapper,
        ITableRepository tableRepository,
        IReservationRepository reservationRepository
        )
    {
        _bookingLog = bookingLog;
        _mapper = mapper;
        _tableRepository = tableRepository;
        _reservationRepository = reservationRepository;
    }
    
    public async Task Consume(ConsumeContext<CreateReservation> context)
    {
        var request = context.Message;
        
        if (!await _tableRepository.HasAnyByIdAsync(request.TableId))
            throw new BadRequestException($"Table with ID {request.TableId} doesn't exists");
            
        var reservation = _mapper.Map<Domain.Entities.Reservation>(request);
        
        await _reservationRepository.CreateAsync(reservation);
        
        _bookingLog.AddLog($"{request.TableId} is pre-booked at {request.From} to {request.To} by userId {request.WhoBookedId}");

        await context.RespondAsync(_mapper.Map<CreateReservationResult>(reservation));
    }
}