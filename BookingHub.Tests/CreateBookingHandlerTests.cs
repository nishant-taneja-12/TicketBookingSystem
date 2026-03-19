using System;
using System.Threading;
using System.Threading.Tasks;
using BookingHub.Application.DTOs;
using BookingHub.Application.Handlers;
using BookingHub.Infrastructure.Data;
using BookingHub.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BookingHub.Tests
{
    public class CreateBookingHandlerTests
    {
        private BookingDbContext CreateInMemoryDb()
        {
            var options = new DbContextOptionsBuilder<BookingDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new BookingDbContext(options);
        }

        [Fact]
        public async Task Handle_CreatesBooking()
        {
            using var db = CreateInMemoryDb();
            var repo = new BookingRepository(db);
            var flightRepo = new FlightRepository(db);

            // Seed a flight
            var flight = BookingHub.Domain.Entities.Flight.Create("Delhi", "Mumbai", DateTime.UtcNow.AddDays(10), 10);
            db.Flights.Add(flight);
            await db.SaveChangesAsync();

            var handler = new CreateBookingHandler(repo, flightRepo);

            var request = new CreateBookingRequest
            {
                BookingDate = DateTime.UtcNow.AddDays(1),
                SeatCount = 3,
                FlightId = flight.IdValue
            };

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(3, result.NumberOfSeats);

            // Verify persisted
            var persisted = await db.Bookings.FirstOrDefaultAsync(b => b.IdValue == result.Id);
            Assert.NotNull(persisted);
        }

        [Fact]
        public async Task Handle_WhenFlightNotFound_ThrowsArgumentException()
        {
            using var db = CreateInMemoryDb();
            var repo = new BookingRepository(db);
            var flightRepo = new FlightRepository(db);
            var handler = new CreateBookingHandler(repo, flightRepo);

            var request = new CreateBookingRequest
            {
                BookingDate = DateTime.UtcNow.AddDays(1),
                SeatCount = 2,
                FlightId = Guid.NewGuid()
            };

            var ex = await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(request, CancellationToken.None));
            Assert.Equal("Flight not found.", ex.Message);
        }

        [Fact]
        public async Task Handle_WhenRequestedSeatsExceedAvailability_ThrowsArgumentException()
        {
            using var db = CreateInMemoryDb();
            var repo = new BookingRepository(db);
            var flightRepo = new FlightRepository(db);

            var flight = BookingHub.Domain.Entities.Flight.Create("Delhi", "Mumbai", DateTime.UtcNow.AddDays(10), 2);
            db.Flights.Add(flight);
            await db.SaveChangesAsync();

            var handler = new CreateBookingHandler(repo, flightRepo);
            var request = new CreateBookingRequest
            {
                BookingDate = DateTime.UtcNow.AddDays(1),
                SeatCount = 3,
                FlightId = flight.IdValue
            };

            var ex = await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(request, CancellationToken.None));
            Assert.Equal("Requested seats exceed available seats.", ex.Message);
        }

        [Fact]
        public async Task Handle_ReducesFlightAvailableSeats_WhenBookingCreated()
        {
            using var db = CreateInMemoryDb();
            var repo = new BookingRepository(db);
            var flightRepo = new FlightRepository(db);

            var flight = BookingHub.Domain.Entities.Flight.Create("Delhi", "Mumbai", DateTime.UtcNow.AddDays(10), 10);
            db.Flights.Add(flight);
            await db.SaveChangesAsync();

            var handler = new CreateBookingHandler(repo, flightRepo);
            var request = new CreateBookingRequest
            {
                BookingDate = DateTime.UtcNow.AddDays(1),
                SeatCount = 4,
                FlightId = flight.IdValue
            };

            await handler.Handle(request, CancellationToken.None);

            var updated = await db.Flights.FirstAsync(f => f.IdValue == flight.IdValue);
            Assert.Equal(6, updated.AvailableSeats);
        }

        [Fact]
        public async Task Handle_WithInvalidDestination_ThrowsArgumentException()
        {
            using var db = CreateInMemoryDb();
            var repo = new BookingRepository(db);
            var flightRepo = new FlightRepository(db);

            var flight = BookingHub.Domain.Entities.Flight.Create("Delhi", "Mumbai", DateTime.UtcNow.AddDays(10), 10);
            db.Flights.Add(flight);
            await db.SaveChangesAsync();

            var handler = new CreateBookingHandler(repo, flightRepo);
            var request = new CreateBookingRequest
            {
                BookingDate = DateTime.UtcNow.AddDays(1),
                SeatCount = 1,
                FlightId = flight.IdValue,
                Destination = new BookingDestinationDto { Name = "", Address = "Some address" }
            };

            await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(request, CancellationToken.None));
        }
    }
}
