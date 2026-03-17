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
    }
}
