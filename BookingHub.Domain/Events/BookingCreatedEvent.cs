using System;
using BookingHub.Domain.ValueObjects;

namespace BookingHub.Domain.Events
{
    /// <summary>
    /// Domain event raised when a new booking is created. Consumers can handle this event to perform
    /// side-effects (notifications, projections, etc.) without coupling to the domain model.
    /// </summary>
    internal sealed class BookingCreatedEvent : DomainEvent
    {
        public BookingId BookingId { get; }
        public BookingDate BookingDate { get; }
        public SeatCount SeatCount { get; }

        public BookingCreatedEvent(BookingId bookingId, BookingDate bookingDate, SeatCount seatCount)
        {
            BookingId = bookingId;
            BookingDate = bookingDate;
            SeatCount = seatCount;
        }
    }
}
