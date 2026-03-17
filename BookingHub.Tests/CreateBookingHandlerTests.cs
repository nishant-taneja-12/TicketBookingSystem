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
            var handler = new CreateBookingHandler(repo);

            var request = new CreateBookingRequest
            {
                BookingDate = DateTime.UtcNow.AddDays(1),
                NumberOfSeats = 3
            };

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(3, result.NumberOfSeats);

            // Verify persisted
            var persisted = await db.Bookings.FirstOrDefaultAsync(b => b.Id == result.Id);
            Assert.NotNull(persisted);
        }
    }
}
