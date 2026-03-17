using System;
using System.Threading;
using System.Threading.Tasks;
using BookingHub.Application.DTOs;
using BookingHub.Domain.Interfaces;
using MediatR;
using BookingHub.Application.Requests;

namespace BookingHub.Application.Handlers
{
    /// <summary>
    /// Use-case handler to fetch a single booking by id.
    /// </summary>
    public class GetBookingByIdHandler : IRequestHandler<GetBookingByIdQuery, BookingDto>
    {
        private readonly IBookingRepository _repository;

        public GetBookingByIdHandler(IBookingRepository repository)
        {
            _repository = repository;
        }

        public async Task<BookingDto?> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken)
        {
            var booking = await _repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
            if (booking == null) return null;

            return new BookingDto
            {
                Id = booking.Id.Value,
                BookingDate = booking.BookingDate.Value,
                NumberOfSeats = booking.SeatCount.Value,
                Destination = booking.Destination == null ? null : new BookingDestinationDto { Name = booking.Destination.Name, Address = booking.Destination.Address }
            };
        }
    }
}
