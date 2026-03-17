using System;
using System.Collections.Generic;
using MediatR;
using BookingHub.Application.DTOs;

namespace BookingHub.Application.Requests
{
    public record GetBookingsByDateRangeQuery(DateTime StartDate, DateTime EndDate) : IRequest<IEnumerable<BookingDto>>;
}
