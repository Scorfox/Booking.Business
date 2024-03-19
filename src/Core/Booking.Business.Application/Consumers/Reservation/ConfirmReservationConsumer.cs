using AutoMapper;
using Booking.Business.Application.Repositories;
using MassTransit;
using Otus.Booking.Common.Booking.Contracts.Reservation.Requests;
using Otus.Booking.Common.Booking.Contracts.Reservation.Responses;
using Otus.Booking.Common.Booking.Exceptions;

namespace Booking.Business.Application.Consumers.Reservation;

public class ConfirmReservationConsumer : IConsumer<ConfirmReservation>
{
    private readonly IMapper _mapper;
    private readonly IReservationRepository _reservationRepository;
    private readonly ITableRepository _tableRepository;

    public ConfirmReservationConsumer(
        IMapper mapper,
        IReservationRepository reservationRepository,
        ITableRepository tableRepository
        )
    {
        _mapper = mapper;
        _reservationRepository = reservationRepository;
        _tableRepository = tableRepository;
    }
    
    public async Task Consume(ConsumeContext<ConfirmReservation> context)
    {
        var request = context.Message;
        
        if (await _tableRepository.FindByIdAsync(request.Id) == null)
            throw new NotFoundException($"Table with ID {request.TableId} doesn't exists");
        
        var reservation = await _reservationRepository.FindByIdAsync(request.Id);
        
        if (reservation == null)
            throw new NotFoundException($"Reservation with ID {request.Id} doesn't exists");
        
        if (request.CompanyId != reservation.Table.CompanyId)
            throw new ForbiddenException($"RequestCompanyId {request.CompanyId} is not equal TableCompanyId {reservation.Table.CompanyId}");

        reservation.WhoConfirmedId = request.WhoConfirmedId;
        
        await _reservationRepository.UpdateAsync(reservation);

        await context.RespondAsync(_mapper.Map<ConfirmReservationResult>(reservation));
    }
}