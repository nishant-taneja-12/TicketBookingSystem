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
            // Create domain entity. Domain enforces validation.
            var booking = Booking.Create(request.BookingDate, request.NumberOfSeats);

            if (request.Destination != null)
            {
                var dest = new BookingDestination(request.Destination.Name, request.Destination.Address);
                booking.SetDestination(dest);
            }

            await _repository.AddAsync(booking, cancellationToken).ConfigureAwait(false);

            return new BookingDto
            {
                Id = booking.Id,
                BookingDate = booking.BookingDate,
                NumberOfSeats = booking.NumberOfSeats,
                Destination = booking.Destination == null ? null : new BookingDestinationDto { Name = booking.Destination.Name, Address = booking.Destination.Address }
            };
        }
    }
}
