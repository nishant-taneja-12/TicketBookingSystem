using System;

namespace BookingHub.Domain.Events
{
    /// <summary>
    /// Base class for domain events. Domain events are used to capture things that happened inside the domain
    /// which other parts of the system may react to. They are intentionally lightweight and have no framework
    /// dependencies so the domain remains pure.
    /// </summary>
    internal abstract class DomainEvent
    {
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
    }
}
