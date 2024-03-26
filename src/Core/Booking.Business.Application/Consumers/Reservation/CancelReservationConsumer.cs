using AutoMapper;
using Booking.Business.Application.Repositories;
using Booking.Business.Domain.Entities;
using MassTransit;
using Otus.Booking.Common.Booking.Contracts.Filial.Requests;
using Otus.Booking.Common.Booking.Contracts.Filial.Responses;
using Otus.Booking.Common.Booking.Contracts.Reservation.Requests;
using Otus.Booking.Common.Booking.Contracts.Reservation.Responses;
using Otus.Booking.Common.Booking.Contracts.User.Requests;
using Otus.Booking.Common.Booking.Contracts.User.Responses;
using Otus.Booking.Common.Booking.Exceptions;
using Otus.Booking.Common.Booking.Notifications.Models;

namespace Booking.Business.Application.Consumers.Reservation;

public class CancelReservationConsumer : IConsumer<CancelReservation>
{
    private readonly IMapper _mapper;
    private readonly IReservationRepository _reservationRepository;
    private readonly ITableRepository _tableRepository;
    private readonly IRequestClient<GetUserById> _userRequestClient;
    private readonly IRequestClient<GetFilialById> _filialByIdRequestClient;

    public CancelReservationConsumer(
        IMapper mapper, 
        IReservationRepository reservationRepository,
        IRequestClient<GetUserById> req,
        IRequestClient<GetFilialById> filial,
        ITableRepository tableRepository
        )
    {
        _mapper = mapper;
        _userRequestClient = req;
        _filialByIdRequestClient = filial;
        _reservationRepository = reservationRepository;
        _tableRepository = tableRepository;
    }
    
    public async Task Consume(ConsumeContext<CancelReservation> context)
    {
        var request = context.Message;
        var table = await _tableRepository.FindByIdAsync(request.Id);
        if (table == null)
            throw new NotFoundException($"Table with ID {request.TableId} doesn't exists");
        
        var reservation = await _reservationRepository.FindByIdAsync(request.Id);
        
        if (reservation == null)
            throw new NotFoundException($"Reservation with ID {request.Id} doesn't exists");
        
        if (request.CompanyId != reservation.Table.CompanyId)
            throw new ForbiddenException($"RequestCompanyId {request.CompanyId} is not equal TableCompanyId {reservation.Table.CompanyId}");

        reservation.WhoCancelledId = request.WhoCancelledId;

        var user = await _userRequestClient.GetResponse<GetUserResult>(new GetUserById { Id = reservation.WhoBookedId });
        var filial =
            await _filialByIdRequestClient.GetResponse<GetFilialResult>(
                new GetFilialById {CompanyId = reservation.Table.FilialId});

        await _reservationRepository.UpdateAsync(reservation);


        var reservationStatusNotification = new ReservationStatusNotification()
        {
            Email = user.Message.Email,
            Address = filial.Message.Address,
            FilialName = filial.Message.Name,
            FirstName = user.Message.FirstName,
            LastName = user.Message.LastName,
            Persons = table.SeatsNumber,
            TableName = table.Name,
            Status = ReservationStatus.Rejected
        };

        await context.Publish(reservationStatusNotification);

        await context.RespondAsync(_mapper.Map<CancelReservationResult>(reservation));
    }
}