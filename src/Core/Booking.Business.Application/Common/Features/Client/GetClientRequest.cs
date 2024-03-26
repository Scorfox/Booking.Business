using MediatR;
using Otus.Booking.Common.Booking.Contracts.Common;
using Otus.Booking.Common.Booking.Contracts.User.Requests;
using Otus.Booking.Common.Booking.Contracts.User.Responses;

namespace Booking.Business.Application.Common.Features.Client
{
    public sealed record GetClientRequest : GetUserById, IRequest<GetUserResult>;
}
