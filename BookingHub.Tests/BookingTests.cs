using System;
using BookingHub.Domain.Entities;
using Xunit;

namespace BookingHub.Tests
{
    public class BookingTests
    {
        [Fact]
        public void Create_WithValidData_Succeeds()
        {
            var bookingDate = DateTime.UtcNow.AddDays(1);
            var booking = Booking.Create(bookingDate, 2);

            Assert.Equal(2, booking.SeatCount.Value);
            Assert.Equal(bookingDate.Date, booking.BookingDate.Value.Date);
            Assert.NotEqual(Guid.Empty, booking.Id.Value);
        }

        [Fact]
        public void Create_WithZeroSeats_Throws()
        {
            var bookingDate = DateTime.UtcNow.AddDays(1);
            Assert.Throws<ArgumentException>(() => Booking.Create(bookingDate, 0));
        }

        [Fact]
        public void Create_WithPastDate_Throws()
        {
            var bookingDate = DateTime.UtcNow.AddDays(-1);
            Assert.Throws<ArgumentException>(() => Booking.Create(bookingDate, 1));
        }

        [Fact]
        public void Create_WithSeatCountAboveMax_Throws()
        {
            var bookingDate = DateTime.UtcNow.AddDays(1);
            Assert.Throws<ArgumentException>(() => Booking.Create(bookingDate, 11));
        }

        [Fact]
        public void Flight_ReduceAvailableSeats_WithZero_Throws()
        {
            var flight = Flight.Create("A", "B", DateTime.UtcNow.AddDays(2), 5);
            Assert.Throws<ArgumentException>(() => flight.ReduceAvailableSeats(0));
        }

        [Fact]
        public void Flight_ReduceAvailableSeats_AboveAvailable_Throws()
        {
            var flight = Flight.Create("A", "B", DateTime.UtcNow.AddDays(2), 5);
            Assert.Throws<InvalidOperationException>(() => flight.ReduceAvailableSeats(6));
        }

        [Fact]
        public void BookingDestination_WithEmptyName_Throws()
        {
            Assert.Throws<ArgumentException>(() => new Domain.ValueObjects.BookingDestination("", "Addr"));
        }

        [Fact]
        public void BookingId_FromEmptyGuid_Throws()
        {
            Assert.Throws<ArgumentException>(() => Domain.ValueObjects.BookingId.FromGuid(Guid.Empty));
        }

        [Fact]
        public void FlightId_FromEmptyGuid_Throws()
        {
            Assert.Throws<ArgumentException>(() => Domain.ValueObjects.FlightId.FromGuid(Guid.Empty));
        }
    }
}
