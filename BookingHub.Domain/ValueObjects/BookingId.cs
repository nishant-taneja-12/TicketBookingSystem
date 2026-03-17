using System;

namespace BookingHub.Domain.ValueObjects
{
    /// <summary>
    /// Value object representing a strongly-typed Booking identifier.
    /// Wrapping Guid makes intent explicit and prevents misuse of plain GUIDs across the model.
    /// </summary>
    public sealed class BookingId : IEquatable<BookingId>
    {
        public Guid Value { get; }

        private BookingId(Guid value)
        {
            if (value == Guid.Empty) throw new ArgumentException("BookingId cannot be empty.", nameof(value));
            Value = value;
        }

        public static BookingId New() => new BookingId(Guid.NewGuid());

        public static BookingId FromGuid(Guid id) => new BookingId(id);

        public override bool Equals(object? obj) => Equals(obj as BookingId);
        public bool Equals(BookingId? other) => other != null && Value == other.Value;
        public override int GetHashCode() => Value.GetHashCode();

        public static implicit operator Guid(BookingId id) => id.Value;
        public override string ToString() => Value.ToString();
    }
}
