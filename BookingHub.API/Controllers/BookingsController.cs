using System;
using System.Threading.Tasks;
using BookingHub.Application.DTOs;
using BookingHub.Application.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace BookingHub.API.Controllers
{
    /// <summary>
    /// Thin API controller. Controllers should only handle HTTP concerns and delegate to application handlers.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BookingsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Create a booking.
        /// </summary>
        [HttpPost]
        [SwaggerRequestExample(typeof(CreateBookingRequest), typeof(BookingHub.Infrastructure.SwaggerExamples.CreateBookingRequestExample))]
        [SwaggerResponseExample(201, typeof(BookingHub.Infrastructure.SwaggerExamples.BookingDtoExample))]
        public async Task<IActionResult> Create([FromBody] CreateBookingRequest request)
        {
            try
            {
                var result = await _mediator.Send(request).ConfigureAwait(false);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (ArgumentException ex)
            {
                // Validation errors thrown by domain are returned as BadRequest.
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Get a booking by id.
        /// </summary>
        [HttpGet("{id:guid}")]
        [SwaggerResponseExample(200, typeof(BookingHub.Infrastructure.SwaggerExamples.BookingDtoExample))]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new GetBookingByIdQuery(id)).ConfigureAwait(false);
            if (result == null) return NotFound();
            return Ok(result);
        }

        /// <summary>
        /// Get bookings by date range. Provide startDate and endDate query params in ISO format (yyyy-MM-dd or full date).
        /// </summary>
        [HttpGet("bydaterange")]
        [SwaggerResponseExample(200, typeof(BookingHub.Infrastructure.SwaggerExamples.BookingDtoExample))]
        public async Task<IActionResult> GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            // Validate range
            if (startDate > endDate)
            {
                return BadRequest(new { error = "startDate must be less than or equal to endDate" });
            }

            var results = await _mediator.Send(new GetBookingsByDateRangeQuery(startDate, endDate)).ConfigureAwait(false);
            return Ok(results);
        }
    }
}
