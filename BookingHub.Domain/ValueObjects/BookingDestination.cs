using System;

namespace BookingHub.Domain.ValueObjects
{
    /// <summary>
    /// Value object representing the destination of a booking.
    /// Immutable and enforces simple validation.
    /// </summary>
    public sealed class BookingDestination : IEquatable<BookingDestination>
    {
        public string Name { get; }
        public string Address { get; }

        // For EF Core
        private BookingDestination() { Name = string.Empty; Address = string.Empty; }

        public BookingDestination(string name, string address)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required.", nameof(name));
            if (string.IsNullOrWhiteSpace(address)) throw new ArgumentException("Address is required.", nameof(address));

            Name = name;
            Address = address;
        }

        public override bool Equals(object? obj) => Equals(obj as BookingDestination);
        public bool Equals(BookingDestination? other) => other != null && Name == other.Name && Address == other.Address;
        public override int GetHashCode() => HashCode.Combine(Name, Address);

        public static bool operator ==(BookingDestination? left, BookingDestination? right) => Equals(left, right);
        public static bool operator !=(BookingDestination? left, BookingDestination? right) => !Equals(left, right);
    }
}
