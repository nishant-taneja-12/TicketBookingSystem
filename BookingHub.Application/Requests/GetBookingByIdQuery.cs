using System;
using MediatR;
using BookingHub.Application.DTOs;

namespace BookingHub.Application.Requests
{
    public record GetBookingByIdQuery(Guid Id) : IRequest<BookingDto?>;
}
