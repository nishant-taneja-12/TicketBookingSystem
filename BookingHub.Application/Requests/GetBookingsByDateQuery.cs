using System;
using System.Collections.Generic;
using MediatR;
using BookingHub.Application.DTOs;

namespace BookingHub.Application.Requests
{
    public record GetBookingsByDateQuery(DateTime Date) : IRequest<IEnumerable<BookingDto>>;
}
