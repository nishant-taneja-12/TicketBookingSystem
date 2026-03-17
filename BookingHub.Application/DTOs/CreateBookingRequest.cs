using System;
using MediatR;

namespace BookingHub.Application.DTOs
{
    /// <summary>
    /// Request DTO for creating a booking. DTOs are part of application layer and used to transfer data in/out.
    /// Application layer orchestrates use-cases; domain rules live inside entities.
    /// </summary>
    public class CreateBookingRequest : IRequest<BookingDto>
    {
        public DateTime BookingDate { get; set; }

        public int SeatCount { get; set; }

        public BookingDestinationDto? Destination { get; set; }

        public Guid FlightId { get; set; }
    }
}
