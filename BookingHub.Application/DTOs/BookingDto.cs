using System;

namespace BookingHub.Application.DTOs
{
    public class BookingDto
    {
        public Guid Id { get; set; }

        public DateTime BookingDate { get; set; }

        public int NumberOfSeats { get; set; }

        public BookingDestinationDto? Destination { get; set; }
    }
}
