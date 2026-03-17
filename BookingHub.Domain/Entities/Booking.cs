using System;

namespace BookingHub.Domain.Entities
{
    /// <summary>
    /// Aggregate root for Booking.
    /// This entity contains the business rules and validation related to a booking.
    /// Domain layer must not depend on other layers.
    /// </summary>
    public class Booking
    {
        // Private setter ensures invariants are kept inside the entity.
        public Guid Id { get; private set; }

        public DateTime BookingDate { get; private set; }

        public int NumberOfSeats { get; private set; }

        // New: optional value object representing destination
        public BookingHub.Domain.ValueObjects.BookingDestination? Destination { get; private set; }

        // For EF Core
        private Booking() { }

        private Booking(Guid id, DateTime bookingDate, int numberOfSeats, BookingHub.Domain.ValueObjects.BookingDestination? destination = null)
        {
            Id = id;
            BookingDate = bookingDate;
            NumberOfSeats = numberOfSeats;
            Destination = destination;

            Validate();
        }

        /// <summary>
        /// Factory method to create a booking while enforcing business rules.
        /// Business rules:
        /// - NumberOfSeats must be > 0
        /// - BookingDate cannot be in the past (compared to UTC now, date precision considered)
        /// </summary>
        /// <exception cref="ArgumentException">When validation fails</exception>
        public static Booking Create(DateTime bookingDate, int numberOfSeats)
        {
            return new Booking(Guid.NewGuid(), bookingDate, numberOfSeats);
        }

        private void Validate()
        {
            if (NumberOfSeats <= 0)
                throw new ArgumentException("NumberOfSeats must be greater than zero.", nameof(NumberOfSeats));

            // We interpret "cannot be in the past" as booking date must be >= today (UTC)
            var todayUtc = DateTime.UtcNow.Date;
            if (BookingDate.Date < todayUtc)
                throw new ArgumentException("BookingDate cannot be in the past.", nameof(BookingDate));
        }

        // Example domain behavior: change seats (keeps validations inside entity)
        public void ChangeNumberOfSeats(int newNumberOfSeats)
        {
            NumberOfSeats = newNumberOfSeats;
            Validate();
        }

        // Allow setting the destination as part of aggregate behavior
        public void SetDestination(BookingHub.Domain.ValueObjects.BookingDestination destination)
        {
            Destination = destination ?? throw new ArgumentNullException(nameof(destination));
        }
    }
}
