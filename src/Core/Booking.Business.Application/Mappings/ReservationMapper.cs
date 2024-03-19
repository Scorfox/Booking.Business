using AutoMapper;
using Otus.Booking.Common.Booking.Contracts.Reservation.Models;
using Otus.Booking.Common.Booking.Contracts.Reservation.Requests;
using Otus.Booking.Common.Booking.Contracts.Reservation.Responses;

namespace Booking.Business.Application.Mappings;

public sealed class ReservationMapper : Profile
{
    public ReservationMapper()
    {
        // Create
        CreateMap<CreateReservation, Domain.Entities.Reservation>();
        CreateMap<Domain.Entities.Reservation, CreateReservationResult>();

        // Read
        CreateMap<Domain.Entities.Reservation, GetReservationResult>();
        CreateMap<Domain.Entities.Reservation, ReservationGettingDto>();

        // Update
        CreateMap<UpdateReservation, Domain.Entities.Reservation>();
        CreateMap<Domain.Entities.Reservation, UpdateReservationResult>();
    }
}