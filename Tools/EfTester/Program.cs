using System;
using BookingHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

class Program
{
    static int Main()
    {
        try
        {
            var options = new DbContextOptionsBuilder<BookingDbContext>()
                .UseInMemoryDatabase("EfTesterDb")
                .Options;

            using var db = new BookingDbContext(options);

            var booking = BookingHub.Domain.Entities.Booking.Create(DateTime.UtcNow.AddDays(1), 2);
            db.Bookings.Add(booking);
            db.SaveChanges();

            Console.WriteLine("Added booking successfully.");
            return 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception thrown:");
            Console.WriteLine(ex.ToString());
            return 3;
        }
    }
}
