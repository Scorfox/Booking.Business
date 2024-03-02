using AutoMapper;
using Otus.Booking.Common.Booking.Contracts.Reservation.Models;
using Otus.Booking.Common.Booking.Contracts.Reservation.Requests;
using Otus.Booking.Common.Booking.Contracts.Reservation.Responses;

namespace Booking.Business.Application.Mappings;

public sealed class ReservationMapper : Profile
{
    public ReservationMapper()
    {
        CreateMap<CreateReservation, Domain.Entities.Reservation>();
        CreateMap<Domain.Entities.Reservation, CreateReservationResult>();

        CreateMap<Domain.Entities.Reservation, FullReservationDto>();

        CreateMap<UpdateReservation, Domain.Entities.Reservation>();
        CreateMap<Domain.Entities.Reservation, UpdateReservationResult>();

        CreateMap<GetReservationId, Domain.Entities.Reservation>();
        CreateMap<Domain.Entities.Reservation, FullReservationDto>();
    }
}