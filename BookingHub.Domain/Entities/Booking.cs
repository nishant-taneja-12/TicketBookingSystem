using System;
using System.Collections.Generic;
using System.Linq;
using BookingHub.Domain.ValueObjects;
using BookingHub.Domain.Events;

namespace BookingHub.Domain.Entities
{
    /// <summary>
    /// Aggregate root for Booking.
    /// Uses Value Objects to express domain intent and keep invariants inside the aggregate.
    /// Raises domain events when important lifecycle changes occur.
    /// </summary>
    public class Booking
    {
        // Store events as objects to avoid EF Core attempting to map DomainEvent as an entity type.
        private readonly List<object> _events = new();

        public BookingId Id { get; private set; }

        // EF-only primitive representation of the Id to ease persistence and querying. This
        // keeps the domain model expressive (BookingId) while allowing EF Core to use a
        // primitive key without polluting the domain with persistence concerns.
        public Guid IdValue
        {
            get => Id.Value;
            private set => Id = BookingId.FromGuid(value);
        }

        public BookingDate BookingDate { get; private set; }

        public SeatCount SeatCount { get; private set; }

        // Optional value object for destination
        public BookingHub.Domain.ValueObjects.BookingDestination? Destination { get; private set; }

        // New: Flight association
        public FlightId? FlightId { get; private set; }

        // EF-only primitive representation for FlightId
        public Guid? FlightIdValue
        {
            get => FlightId == null ? null : FlightId.Value;
            private set => FlightId = value == null ? null : FlightId.FromGuid(value.Value);
        }

        // For EF Core
        private Booking() { }

        private Booking(BookingId id, BookingDate bookingDate, SeatCount seatCount, BookingHub.Domain.ValueObjects.BookingDestination? destination = null, FlightId? flightId = null)
        {
            Id = id;
            BookingDate = bookingDate;
            SeatCount = seatCount;
            Destination = destination;
            FlightId = flightId;

            // raise event as part of creation
            _events.Add(new BookingCreatedEvent(Id, BookingDate, SeatCount));
        }

        /// <summary>
        /// Factory that accepts primitive inputs for interoperability but creates Value Objects internally.
        /// Business invariants are enforced by the Value Objects and aggregate methods.
        /// </summary>
        public static Booking Create(DateTime bookingDate, int numberOfSeats, BookingHub.Domain.ValueObjects.BookingDestination? destination = null, FlightId? flightId = null)
        {
            var bd = BookingDate.FromDateTime(bookingDate);
            var sc = SeatCount.FromInt(numberOfSeats);
            var id = BookingId.New();
            return new Booking(id, bd, sc, destination, flightId);
        }

        /// <summary>
        /// Exposes domain events produced by this aggregate. Consumers (repositories/services) may dispatch them.
        /// Internal to avoid EF Core mapping and to keep domain events encapsulated.
        /// </summary>
        internal IReadOnlyCollection<DomainEvent> DomainEvents => _events.Cast<DomainEvent>().ToList().AsReadOnly();

        internal void ClearDomainEvents() => _events.Clear();

        /// <summary>
        /// Change seat count via aggregate behavior to ensure invariants are maintained.
        /// </summary>
        public void ChangeSeatCount(int newCount)
        {
            SeatCount = SeatCount.FromInt(newCount);
        }

        public void SetDestination(BookingHub.Domain.ValueObjects.BookingDestination destination)
        {
            Destination = destination ?? throw new ArgumentNullException(nameof(destination));
        }
    }
}
