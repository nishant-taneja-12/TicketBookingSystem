using System;

namespace BookingHub.Domain.ValueObjects
{
    /// <summary>
    /// Value object for a Booking date. Ensures bookings cannot be created in the past.
    /// Stored as DateTime (UTC) and enforces domain rule at creation time.
    /// </summary>
    public sealed class BookingDate : IEquatable<BookingDate>
    {
        public DateTime Value { get; }

        private BookingDate(DateTime value)
        {
            Value = value;
        }

        public static BookingDate FromDateTime(DateTime dt)
        {
            var utc = dt.Kind == DateTimeKind.Utc ? dt : dt.ToUniversalTime();
            var todayUtc = DateTime.UtcNow.Date;
            if (utc.Date < todayUtc) throw new ArgumentException("Booking date cannot be in the past.", nameof(dt));
            return new BookingDate(utc);
        }

        internal static BookingDate FromPersistence(DateTime dt)
        {
            var utc = dt.Kind == DateTimeKind.Utc ? dt : dt.ToUniversalTime();
            return new BookingDate(utc); // NO validation
        }
        public override bool Equals(object? obj) => Equals(obj as BookingDate);
        public bool Equals(BookingDate? other) => other != null && Value == other.Value;
        public override int GetHashCode() => Value.GetHashCode();

        public static implicit operator DateTime(BookingDate d) => d.Value;
        public override string ToString() => Value.ToString("o");
    }
}
