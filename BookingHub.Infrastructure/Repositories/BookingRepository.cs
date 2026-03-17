using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BookingHub.Domain.Entities;
using BookingHub.Domain.Interfaces;
using BookingHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookingHub.Infrastructure.Repositories
{
    /// <summary>
    /// EF Core implementation of IBookingRepository. Infrastructure layer depends on Domain and EF Core.
    /// Repositories only handle persistence; business logic remains in the domain entities.
    /// </summary>
    public class BookingRepository : IBookingRepository
    {
        private readonly BookingDbContext _dbContext;

        public BookingRepository(BookingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Booking booking, CancellationToken cancellationToken = default)
        {
            _dbContext.Bookings.Add(booking);
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<Booking?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            // Compare primitive IdValue persisted by EF
            return await _dbContext.Bookings
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.IdValue == id, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<Booking>> GetByDateAsync(DateTime date, CancellationToken cancellationToken = default)
        {
            // Compare Date part to support retrieving all bookings for a given calendar date.
            var start = date.Date;
            var end = start.AddDays(1);

            return await _dbContext.Bookings
                .AsNoTracking()
                .Where(b => b.BookingDate.Value >= start && b.BookingDate.Value < end)
                .OrderBy(b => b.BookingDate.Value)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
