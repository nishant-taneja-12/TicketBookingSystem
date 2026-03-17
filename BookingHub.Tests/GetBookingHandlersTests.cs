using System;
using System.Threading.Tasks;
using BookingHub.Application.Handlers;
using BookingHub.Application.Requests;
using BookingHub.Infrastructure.Data;
using BookingHub.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BookingHub.Tests
{
    public class GetBookingHandlersTests
    {
        private BookingDbContext CreateInMemoryDb()
        {
            var options = new DbContextOptionsBuilder<BookingDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new BookingDbContext(options);
        }

        [Fact]
        public async Task GetById_ReturnsBooking()
        {
            using var db = CreateInMemoryDb();
            var repo = new BookingRepository(db);

            // seed
            var booking = Domain.Entities.Booking.Create(DateTime.UtcNow.AddDays(1), 2);
            db.Bookings.Add(booking);
            await db.SaveChangesAsync();

            var handler = new GetBookingByIdHandler(repo);
            var result = await handler.Handle(new GetBookingByIdQuery(booking.IdValue), default);

            Assert.NotNull(result);
            Assert.Equal(booking.IdValue, result.Id);
        }

        [Fact]
        public async Task GetByDate_ReturnsBookings()
        {
            using var db = CreateInMemoryDb();
            var repo = new BookingRepository(db);

            var booking = Domain.Entities.Booking.Create(DateTime.UtcNow.AddDays(2).Date.AddHours(10), 4);
            db.Bookings.Add(booking);
            await db.SaveChangesAsync();

            var handler = new GetBookingsByDateHandler(repo);
            var results = await handler.Handle(new GetBookingsByDateQuery(booking.BookingDate.Value.Date), default);

            Assert.NotEmpty(results);
        }
    }
}
