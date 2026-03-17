using System;

namespace BookingHub.Domain.ValueObjects
{
    /// <summary>
    /// Value object representing number of seats for a booking. Enforces domain rules (min/max).
    /// </summary>
    public sealed class SeatCount : IEquatable<SeatCount>
    {
        public int Value { get; }
        private const int MaxSeats = 10;

        private SeatCount(int value)
        {
            Value = value;
        }

        public static SeatCount FromInt(int value)
        {
            if (value <= 0) throw new ArgumentException("Seat count must be greater than zero.", nameof(value));
            if (value > MaxSeats) throw new ArgumentException($"Seat count must not exceed {MaxSeats}.", nameof(value));
            return new SeatCount(value);
        }

        public override bool Equals(object? obj) => Equals(obj as SeatCount);
        public bool Equals(SeatCount? other) => other != null && Value == other.Value;
        public override int GetHashCode() => Value.GetHashCode();

        public static implicit operator int(SeatCount s) => s.Value;
        public override string ToString() => Value.ToString();
    }
}
