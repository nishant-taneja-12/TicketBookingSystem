using System.Threading;
using System.Threading.Tasks;
using BookingHub.Domain.Entities;
using BookingHub.Domain.Interfaces;
using BookingHub.Domain.ValueObjects;
using BookingHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookingHub.Infrastructure.Repositories
{
    public class FlightRepository : IFlightRepository
    {
        private readonly BookingDbContext _dbContext;

        public FlightRepository(BookingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Flight?> GetByIdAsync(FlightId id, CancellationToken cancellationToken = default)
        {
            // Return tracked entity so updates within same DbContext do not cause identity conflicts.
            return await _dbContext.Flights
                .FirstOrDefaultAsync(f => f.IdValue == id.Value, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task UpdateAsync(Flight flight, CancellationToken cancellationToken = default)
        {
            _dbContext.Flights.Update(flight);
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
