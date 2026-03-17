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
    }
}
