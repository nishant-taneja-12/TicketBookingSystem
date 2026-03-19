using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BookingHub.Domain.Entities;
using BookingHub.Domain.ValueObjects;
using BookingHub.Infrastructure.Data;
using BookingHub.Infrastructure.Repositories;
using BookingHub.Infrastructure.SwaggerExamples;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BookingHub.Tests
{
    public class InfrastructureTests
    {
        private static BookingDbContext CreateSqliteDbContext()
        {
            var dbPath = Path.Combine(Path.GetTempPath(), $"bookinghub-tests-{Guid.NewGuid():N}.db");
            var options = new DbContextOptionsBuilder<BookingDbContext>()
                .UseSqlite($"Data Source={dbPath}")
                .Options;

            var context = new BookingDbContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            return context;
        }

        [Fact]
        public async Task BookingRepository_GetById_WhenMissing_ReturnsNull()
        {
            using var db = CreateSqliteDbContext();
            var repo = new BookingRepository(db);
            var result = await repo.GetByIdAsync(Guid.NewGuid());
            Assert.Null(result);
        }

        [Fact]
        public async Task FlightRepository_GetById_WhenMissing_ReturnsNull()
        {
            using var db = CreateSqliteDbContext();
            var repo = new FlightRepository(db);
            var result = await repo.GetByIdAsync(FlightId.New());
            Assert.Null(result);
        }

        [Fact]
        public async Task FlightRepository_Update_PersistsAvailableSeats()
        {
            using var db = CreateSqliteDbContext();
            var flight = Flight.Create("Auckland", "Sydney", DateTime.UtcNow.AddDays(5), 20);
            db.Flights.Add(flight);
            await db.SaveChangesAsync();

            var repo = new FlightRepository(db);
            var loaded = await repo.GetByIdAsync(FlightId.FromGuid(flight.IdValue));
            Assert.NotNull(loaded);

            loaded!.ReduceAvailableSeats(3);
            await repo.UpdateAsync(loaded);

            var updated = await db.Flights.AsNoTracking().FirstAsync(f => f.IdValue == flight.IdValue);
            Assert.Equal(17, updated.AvailableSeats);
        }

        [Fact]
        public async Task BookingRepository_AddAsync_PersistsDestinationAndFlightId()
        {
            using var db = CreateSqliteDbContext();
            var repo = new BookingRepository(db);
            var flightId = FlightId.New();
            var booking = Booking.Create(
                DateTime.UtcNow.AddDays(2),
                2,
                new BookingDestination("Convention Center", "45 Queen St"),
                flightId);

            await repo.AddAsync(booking);

            var persisted = await db.Bookings.AsNoTracking().FirstAsync(b => b.IdValue == booking.IdValue);
            Assert.Equal("Convention Center", persisted.Destination!.Name);
            Assert.Equal("45 Queen St", persisted.Destination!.Address);
            Assert.Equal(flightId.Value, persisted.FlightIdValue);
        }

        [Fact]
        public async Task BookingRepository_GetByDateRange_ReturnsSortedAndInclusive()
        {
            using var db = CreateSqliteDbContext();
            var repo = new BookingRepository(db);
            var start = DateTime.UtcNow.Date.AddDays(1);
            var end = DateTime.UtcNow.Date.AddDays(3);

            var b1 = Booking.Create(start.AddHours(20), 1);
            var b2 = Booking.Create(start.AddDays(1).AddHours(8), 1);
            var b3 = Booking.Create(end.AddHours(22), 1);
            var outOfRange = Booking.Create(end.AddDays(1), 1);

            db.Bookings.AddRange(b1, b2, b3, outOfRange);
            await db.SaveChangesAsync();

            var results = (await repo.GetByDateRangeAsync(start, end)).ToList();

            Assert.Equal(3, results.Count);
            Assert.Equal(b1.IdValue, results[0].IdValue);
            Assert.Equal(b2.IdValue, results[1].IdValue);
            Assert.Equal(b3.IdValue, results[2].IdValue);
        }

        [Fact]
        public void BookingDbContext_SeedsDefaultFlights()
        {
            using var db = CreateSqliteDbContext();
            var flights = db.Flights.AsNoTracking().ToList();

            Assert.Equal(2, flights.Count);
            Assert.Contains(flights, f => f.IdValue == Guid.Parse("11111111-1111-1111-1111-111111111111"));
            Assert.Contains(flights, f => f.IdValue == Guid.Parse("22222222-2222-2222-2222-222222222222"));
        }

        [Fact]
        public void SwaggerExamples_ReturnExpectedExamplePayloads()
        {
            var bookingExample = new BookingDtoExample().GetExamples();
            var createExample = new CreateBookingRequestExample().GetExamples();

            Assert.NotEqual(Guid.Empty, bookingExample.Id);
            Assert.NotNull(bookingExample.Destination);
            Assert.Equal(3, createExample.SeatCount);
            Assert.Equal(Guid.Parse("11111111-1111-1111-1111-111111111111"), createExample.FlightId);
        }
    }
}
