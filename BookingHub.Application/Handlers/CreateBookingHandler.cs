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
        private readonly IFlightRepository _flightRepository;

        public CreateBookingHandler(IBookingRepository repository, IFlightRepository flightRepository)
        {
            _repository = repository;
            _flightRepository = flightRepository;
        }

        public async Task<BookingDto> Handle(CreateBookingRequest request, CancellationToken cancellationToken)
        {
            BookingHub.Domain.ValueObjects.BookingDestination? dest = null;
            if (request.Destination != null)
            {
                dest = new BookingHub.Domain.ValueObjects.BookingDestination(request.Destination.Name, request.Destination.Address);
            }

            // Fetch flight from repository
            var flightId = FlightId.FromGuid(request.FlightId);
            var flight = await _flightRepository.GetByIdAsync(flightId, cancellationToken).ConfigureAwait(false);
            if (flight == null) throw new System.ArgumentException("Flight not found.");

            // Validate requested seats against flight availability
            if (request.SeatCount > flight.AvailableSeats) throw new System.ArgumentException("Requested seats exceed available seats.");

            // Reduce available seats on the flight
            flight.ReduceAvailableSeats(request.SeatCount);

            // Create booking with FlightId
            var booking = Booking.Create(request.BookingDate, request.SeatCount, dest, flightId);

            // Persist booking and update flight
            await _repository.AddAsync(booking, cancellationToken).ConfigureAwait(false);
            await _flightRepository.UpdateAsync(flight, cancellationToken).ConfigureAwait(false);

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
