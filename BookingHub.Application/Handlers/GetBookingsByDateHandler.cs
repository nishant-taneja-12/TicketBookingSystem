using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BookingHub.Application.DTOs;
using BookingHub.Domain.Interfaces;
using MediatR;
using BookingHub.Application.Requests;

namespace BookingHub.Application.Handlers
{
    /// <summary>
    /// Use-case handler to retrieve bookings for a specific date.
    /// </summary>
    public class GetBookingsByDateHandler : IRequestHandler<GetBookingsByDateQuery, IEnumerable<BookingDto>>
    {
        private readonly IBookingRepository _repository;

        public GetBookingsByDateHandler(IBookingRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<BookingDto>> Handle(GetBookingsByDateQuery request, CancellationToken cancellationToken)
        {
            var bookings = await _repository.GetByDateAsync(request.Date.Date, cancellationToken).ConfigureAwait(false);

            return bookings.Select(b => new BookingDto
            {
                Id = b.Id.Value,
                BookingDate = b.BookingDate.Value,
                NumberOfSeats = b.SeatCount.Value,
                Destination = b.Destination == null ? null : new BookingDestinationDto { Name = b.Destination.Name, Address = b.Destination.Address }
            });
        }
    }
}
