using AutoMapper;
using Booking.Domain.Entities;

namespace Booking.Application.Features.ReservationFeatures;

public sealed class CreateReservationMapper : Profile
{
    public CreateReservationMapper()
    {
        CreateMap<CreateReservationRequest, Reservation>();
        CreateMap<Reservation, CreateReservationResponse>();
    }
}