using System;
using Swashbuckle.AspNetCore.Filters;
using BookingHub.Application.DTOs;

namespace BookingHub.Infrastructure.SwaggerExamples
{
    public class BookingDtoExample : IExamplesProvider<BookingDto>
    {
        public BookingDto GetExamples()
        {
            return new BookingDto
            {
                Id = Guid.NewGuid(),
                BookingDate = DateTime.UtcNow.AddDays(3).Date.AddHours(10),
                NumberOfSeats = 3,
                Destination = new BookingDestinationDto { Name = "Conference Center", Address = "123 Main St, City" }
            };
        }
    }
}
