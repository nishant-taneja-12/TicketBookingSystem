using System;
using BookingHub.Domain.Entities;
using BookingHub.Domain.ValueObjects;
using Xunit;

namespace BookingHub.Tests
{
    public class DomainEdgeCasesTests
    {
        [Fact]
        public void Flight_Create_WithEmptyFrom_Throws()
        {
            Assert.Throws<ArgumentException>(() => Flight.Create("", "Sydney", DateTime.UtcNow.AddDays(2), 10));
        }

        [Fact]
        public void Flight_Create_WithEmptyTo_Throws()
        {
            Assert.Throws<ArgumentException>(() => Flight.Create("Melbourne", " ", DateTime.UtcNow.AddDays(2), 10));
        }

        [Fact]
        public void Flight_Create_WithPastDeparture_Throws()
        {
            Assert.Throws<ArgumentException>(() => Flight.Create("Melbourne", "Sydney", DateTime.UtcNow.AddMinutes(-1), 10));
        }

        [Fact]
        public void Flight_Create_WithNegativeSeats_Throws()
        {
            Assert.Throws<ArgumentException>(() => Flight.Create("Melbourne", "Sydney", DateTime.UtcNow.AddDays(2), -1));
        }

        [Fact]
        public void Flight_Create_WithUnspecifiedDate_AssignsUtcKind()
        {
            var unspecified = DateTime.SpecifyKind(DateTime.UtcNow.AddDays(3), DateTimeKind.Unspecified);
            var flight = Flight.Create("Melbourne", "Sydney", unspecified, 10);

            Assert.Equal(DateTimeKind.Utc, flight.DepartureDate.Kind);
        }

        [Fact]
        public void BookingDestination_WithEmptyAddress_Throws()
        {
            Assert.Throws<ArgumentException>(() => new BookingDestination("Venue", ""));
        }

        [Fact]
        public void Booking_ChangeSeatCount_WithInvalidValue_Throws()
        {
            var booking = Booking.Create(DateTime.UtcNow.AddDays(1), 2);

            Assert.Throws<ArgumentException>(() => booking.ChangeSeatCount(0));
        }

        [Fact]
        public void SeatCount_ToString_ReturnsNumericValue()
        {
            var seatCount = SeatCount.FromInt(3);

            Assert.Equal("3", seatCount.ToString());
        }

        [Fact]
        public void BookingDate_Today_IsValid()
        {
            var today = DateTime.UtcNow.Date;
            var bookingDate = BookingDate.FromDateTime(today);

            Assert.Equal(today, bookingDate.Value.Date);
        }

        [Fact]
        public void BookingDestination_Equality_UsesNameAndAddress()
        {
            var a = new BookingDestination("Hall A", "100 George St");
            var b = new BookingDestination("Hall A", "100 George St");
            var c = new BookingDestination("Hall B", "100 George St");

            Assert.True(a == b);
            Assert.True(a.Equals(b));
            Assert.True(a != c);
        }

        [Fact]
        public void BookingId_And_FlightId_Equality_And_ToString_Work()
        {
            var guid = Guid.NewGuid();
            var bookingId1 = BookingId.FromGuid(guid);
            var bookingId2 = BookingId.FromGuid(guid);
            var flightId1 = FlightId.FromGuid(guid);
            var flightId2 = FlightId.FromGuid(guid);

            Assert.Equal(bookingId1, bookingId2);
            Assert.Equal(flightId1, flightId2);
            Assert.Equal(guid.ToString(), bookingId1.ToString());
            Assert.Equal(guid.ToString(), flightId1.ToString());
        }

        [Fact]
        public void Booking_SetDestination_WithNull_Throws()
        {
            var booking = Booking.Create(DateTime.UtcNow.AddDays(1), 1);

            Assert.Throws<ArgumentNullException>(() => booking.SetDestination(null!));
        }
    }
}
