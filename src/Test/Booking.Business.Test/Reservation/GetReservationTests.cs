using AutoFixture;
using AutoMapper;
using Booking.Business.Application.Consumers.Reservation;
using Booking.Business.Application.Mappings;
using Booking.Business.Persistence.Context;
using Booking.Business.Persistence.Repositories;
using MassTransit.Testing;


namespace Booking.Business.Test.Reservation
{
    public class GetReservationTests
    {
        private GetReservationConsumer Consumer { get; }

        public GetReservationTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<ReservationMapper>());

            //var reservationRepository = new ReservationRepository(DataContext);
            //Consumer = new GetReservationConsumer(reservationRepository, new Mapper(config));
        }

        // Todo
    }
}
