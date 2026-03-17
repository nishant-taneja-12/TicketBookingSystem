using System;
using BookingHub.Domain.ValueObjects;

namespace BookingHub.Domain.Entities
{
    public class Flight
    {
        // EF core
        private Flight() { }

        private Flight(FlightId id, string from, string to, DateTime departureDate, int availableSeats)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            From = string.IsNullOrWhiteSpace(from) ? throw new ArgumentException("From is required", nameof(from)) : from;
            To = string.IsNullOrWhiteSpace(to) ? throw new ArgumentException("To is required", nameof(to)) : to;

            if (departureDate.Kind == DateTimeKind.Unspecified)
                departureDate = DateTime.SpecifyKind(departureDate, DateTimeKind.Utc);

            if (departureDate < DateTime.UtcNow)
                throw new ArgumentException("DepartureDate cannot be in the past.", nameof(departureDate));

            if (availableSeats < 0)
                throw new ArgumentException("AvailableSeats cannot be negative.", nameof(availableSeats));

            DepartureDate = departureDate;
            AvailableSeats = availableSeats;
        }

        public FlightId Id { get; private set; } = null!;

        // EF primitive
        public Guid IdValue
        {
            get => Id.Value;
            private set => Id = FlightId.FromGuid(value);
        }

        public string From { get; private set; } = null!;
        public string To { get; private set; } = null!;
        public DateTime DepartureDate { get; private set; }
        public int AvailableSeats { get; private set; }

        public static Flight Create(string from, string to, DateTime departureDate, int availableSeats)
        {
            var id = FlightId.New();
            return new Flight(id, from, to, departureDate, availableSeats);
        }

        public void ReduceAvailableSeats(int seats)
        {
            if (seats <= 0) throw new ArgumentException("Seats to reduce must be positive.", nameof(seats));
            if (seats > AvailableSeats) throw new InvalidOperationException("Insufficient available seats.");
            AvailableSeats -= seats;
        }
    }
}
