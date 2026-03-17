using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BookingHub.Domain.Entities;

namespace BookingHub.Domain.Interfaces
{
    /// <summary>
    /// Repository interface for Booking. This lives in the domain layer as a contract.
    /// Infrastructure will implement this interface. Application layer will use it.
    /// </summary>
    public interface IBookingRepository
    {
        Task AddAsync(Booking booking, CancellationToken cancellationToken = default);

        Task<Booking?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<IEnumerable<Booking>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    }
}
