using System;
using System.Linq;
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
        public async Task Should_Return_Bookings_Within_DateRange()
        {
            using var db = CreateInMemoryDb();
            var repo = new BookingRepository(db);

            // seed bookings across dates
            var b1 = Domain.Entities.Booking.Create(DateTime.UtcNow.Date.AddDays(1).AddHours(10), 2);
            var b2 = Domain.Entities.Booking.Create(DateTime.UtcNow.Date.AddDays(3).AddHours(12), 1);
            // place b3 outside the requested range
            var b3 = Domain.Entities.Booking.Create(DateTime.UtcNow.Date.AddDays(6).AddHours(9), 4);
            db.Bookings.AddRange(b1, b2, b3);
            await db.SaveChangesAsync();

            var handler = new GetBookingsByDateHandler(repo);

            var start = DateTime.UtcNow.Date.AddDays(2);
            var end = DateTime.UtcNow.Date.AddDays(5);

            var results = await handler.Handle(new GetBookingsByDateRangeQuery(start, end), default);

            Assert.NotNull(results);
            var list = results.ToList();
            Assert.Single(list);
            Assert.Equal(b2.IdValue, list[0].Id);
        }

        [Fact]
        public async Task Should_Return_Empty_When_No_Bookings_In_Range()
        {
            using var db = CreateInMemoryDb();
            var repo = new BookingRepository(db);

            var b1 = Domain.Entities.Booking.Create(DateTime.UtcNow.Date.AddDays(1).AddHours(10), 2);
            db.Bookings.Add(b1);
            await db.SaveChangesAsync();

            var handler = new GetBookingsByDateHandler(repo);

            var start = DateTime.UtcNow.Date.AddDays(5);
            var end = DateTime.UtcNow.Date.AddDays(6);

            var results = await handler.Handle(new GetBookingsByDateRangeQuery(start, end), default);

            Assert.Empty(results);
        }

        [Fact]
        public async Task Boundary_Includes_Start_And_End_Dates()
        {
            using var db = CreateInMemoryDb();
            var repo = new BookingRepository(db);

            var start = DateTime.UtcNow.Date.AddDays(2);
            var end = DateTime.UtcNow.Date.AddDays(4);

            var bStart = Domain.Entities.Booking.Create(start.AddHours(0), 1);
            var bMiddle = Domain.Entities.Booking.Create(start.AddDays(1).AddHours(3), 1);
            var bEnd = Domain.Entities.Booking.Create(end.AddHours(23), 1);

            db.Bookings.AddRange(bStart, bMiddle, bEnd);
            await db.SaveChangesAsync();

            var handler = new GetBookingsByDateHandler(repo);
            var results = await handler.Handle(new GetBookingsByDateRangeQuery(start, end), default);

            var list = results.ToList();
            Assert.Equal(3, list.Count);
            Assert.Contains(list, x => x.Id == bStart.IdValue);
            Assert.Contains(list, x => x.Id == bMiddle.IdValue);
            Assert.Contains(list, x => x.Id == bEnd.IdValue);
        }
    }
}
