using System.Threading;
using System.Threading.Tasks;
using BookingHub.Domain.Entities;
using BookingHub.Domain.ValueObjects;

namespace BookingHub.Domain.Interfaces
{
    public interface IFlightRepository
    {
        Task<Flight?> GetByIdAsync(FlightId id, CancellationToken cancellationToken = default);
        Task UpdateAsync(Flight flight, CancellationToken cancellationToken = default);
    }
}
