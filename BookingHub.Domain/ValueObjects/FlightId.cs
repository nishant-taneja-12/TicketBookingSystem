using System;

namespace BookingHub.Domain.ValueObjects
{
    /// <summary>
    /// Value object representing a strongly-typed Flight identifier.
    /// </summary>
    public sealed class FlightId : IEquatable<FlightId>
    {
        public Guid Value { get; }

        private FlightId(Guid value)
        {
            if (value == Guid.Empty) throw new ArgumentException("FlightId cannot be empty.", nameof(value));
            Value = value;
        }

        public static FlightId New() => new FlightId(Guid.NewGuid());

        public static FlightId FromGuid(Guid id) => new FlightId(id);

        public override bool Equals(object? obj) => Equals(obj as FlightId);
        public bool Equals(FlightId? other) => other != null && Value == other.Value;
        public override int GetHashCode() => Value.GetHashCode();

        public static implicit operator Guid(FlightId id) => id.Value;
        public override string ToString() => Value.ToString();
    }
}
