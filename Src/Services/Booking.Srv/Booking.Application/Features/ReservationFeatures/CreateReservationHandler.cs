using AutoMapper;
using Booking.Application.Common.Exceptions;
using Booking.Application.Features.TableFeatures;
using Booking.Application.Repositories;
using Booking.Domain.Entities;
using MediatR;

namespace Booking.Application.Features.ReservationFeatures;

public sealed class CreateReservationHandler : IRequestHandler<CreateReservationRequest, CreateReservationResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IReservationRepository _reservationRepository;
    private readonly IMapper _mapper;

    public CreateReservationHandler(IUnitOfWork unitOfWork, IReservationRepository reservationRepository, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _reservationRepository = reservationRepository;
        _mapper = mapper;
    }
    
    public async Task<CreateReservationResponse> Handle(CreateReservationRequest request, CancellationToken cancellationToken)
    {
        var reservation = _mapper.Map<Reservation>(request);
        
        _reservationRepository.Create(reservation);
        await _unitOfWork.Save(cancellationToken);

        return _mapper.Map<CreateReservationResponse>(reservation);
    }
}