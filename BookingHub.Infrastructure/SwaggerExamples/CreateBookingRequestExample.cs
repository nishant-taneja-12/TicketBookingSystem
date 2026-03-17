using System;
using Swashbuckle.AspNetCore.Filters;
using BookingHub.Application.DTOs;

namespace BookingHub.Infrastructure.SwaggerExamples
{
    public class CreateBookingRequestExample : IExamplesProvider<CreateBookingRequest>
    {
        public CreateBookingRequest GetExamples()
        {
            return new CreateBookingRequest
            {
                BookingDate = DateTime.UtcNow.AddDays(3).Date.AddHours(10),
                NumberOfSeats = 3,
                Destination = new BookingDestinationDto { Name = "Conference Center", Address = "123 Main St, City" }
            };
        }
    }
}
