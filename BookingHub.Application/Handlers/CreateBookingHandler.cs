using System.Threading;
using System.Threading.Tasks;
using BookingHub.Application.DTOs;
using BookingHub.Domain.Interfaces;
using BookingHub.Domain.Entities;
using BookingHub.Domain.ValueObjects;
using MediatR;

namespace BookingHub.Application.Handlers
{
    /// <summary>
    /// Use case handler for creating a booking. Orchestrates domain creation and persistence.
    /// Application layer: coordinates domain operations and repositories but does not contain business rules.
    /// </summary>
    public class CreateBookingHandler : IRequestHandler<CreateBookingRequest, BookingDto>
    {
        private readonly IBookingRepository _repository;

        public CreateBookingHandler(IBookingRepository repository)
        {
            _repository = repository;
        }

        public async Task<BookingDto> Handle(CreateBookingRequest request, CancellationToken cancellationToken)
        {
            BookingHub.Domain.ValueObjects.BookingDestination? dest = null;
            if (request.Destination != null)
            {
                dest = new BookingHub.Domain.ValueObjects.BookingDestination(request.Destination.Name, request.Destination.Address);
            }

            var booking = Booking.Create(request.BookingDate, request.NumberOfSeats, dest);

            await _repository.AddAsync(booking, cancellationToken).ConfigureAwait(false);

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
