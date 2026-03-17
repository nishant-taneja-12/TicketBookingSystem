using System;
using BookingHub.Domain.Entities;
using BookingHub.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BookingHub.Infrastructure.Data
{
    /// <summary>
    /// EF Core DbContext living in infrastructure layer. Responsible for persistence concerns.
    /// The domain model lives in the Domain layer and is mapped here.
    /// </summary>
    public class BookingDbContext : DbContext
    {
        public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options)
        {
        }

        public DbSet<Booking> Bookings { get; set; } = null!;
        public DbSet<Flight> Flights { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var booking = modelBuilder.Entity<Booking>();
            booking.ToTable("Bookings");

            // Prevent EF from trying to map domain Id value object directly.
            booking.Ignore(b => b.Id);
            // Prevent EF from mapping the FlightId value object; use FlightIdValue primitive instead
            booking.Ignore(b => b.FlightId);

            // Use the primitive IdValue property for the PK so EF can work with a Guid directly.
            booking.HasKey(b => b.IdValue);

            // Map IdValue to column 'IdValue' (new schema)
            booking.Property(b => b.IdValue)
                .HasColumnName("IdValue")
                .HasColumnType("TEXT");

            // BookingDate -> stored as TEXT/DateTime using converter
            //var dateConverter = new ValueConverter<BookingDate, DateTime>(
            //    v => v.Value,
            //    v => BookingDate.FromDateTime(v));

            //booking.Property(b => b.BookingDate)
            //    .HasConversion(dateConverter)
            //    .IsRequired()
            //    .HasColumnType("TEXT")
            //    .HasColumnName("BookingDate");

            booking.Ignore(b => b.BookingDate);

            booking.Property(b => b.BookingDateValue)
                .HasColumnName("BookingDate")
                .HasColumnType("TEXT")
                .IsRequired();

            // SeatCount -> map to column 'SeatCount' (new schema)
            var seatsConverter = new ValueConverter<SeatCount, int>(
                v => v.Value,
                v => SeatCount.FromInt(v));

            booking.Property(b => b.SeatCount)
                .HasConversion(seatsConverter)
                .IsRequired()
                .HasColumnType("INTEGER")
                .HasColumnName("SeatCount");

            // Map BookingDestination value object as an owned type (stored in the same table)
            booking.OwnsOne(b => b.Destination, dest =>
            {
                dest.Property(d => d.Name)
                    .HasColumnName("Destination_Name")
                    .HasColumnType("TEXT");

                dest.Property(d => d.Address)
                    .HasColumnName("Destination_Address")
                    .HasColumnType("TEXT");
            });

            // Map FlightIdValue primitive column
            booking.Property(b => b.FlightIdValue)
                .HasColumnName("FlightIdValue")
                .HasColumnType("TEXT");

            // ----- Flight mapping -----
            var flight = modelBuilder.Entity<Flight>();
            flight.ToTable("Flights");

            // Prevent EF from mapping value object directly
            flight.Ignore(f => f.Id);
            flight.HasKey(f => f.IdValue);
            flight.Property(f => f.IdValue).HasColumnType("TEXT").HasColumnName("IdValue");

            // Use FromLocation/ToLocation to avoid reserved words
            flight.Property(f => f.From)
                .IsRequired()
                .HasColumnType("TEXT")
                .HasColumnName("FromLocation");

            flight.Property(f => f.To)
                .IsRequired()
                .HasColumnType("TEXT")
                .HasColumnName("ToLocation");

            flight.Property(f => f.DepartureDate)
                .IsRequired()
                .HasColumnType("TEXT")
                .HasColumnName("DepartureDate");

            flight.Property(f => f.AvailableSeats)
                .IsRequired()
                .HasColumnType("INTEGER")
                .HasColumnName("AvailableSeats");

            // Seed two flights (use fixed future dates)
            flight.HasData(
                new
                {
                    IdValue = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    From = "Delhi",
                    To = "Mumbai",
                    DepartureDate = new DateTime(2027, 1, 1, 10, 0, 0, DateTimeKind.Utc),
                    AvailableSeats = 100
                },
                new
                {
                    IdValue = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    From = "Bangalore",
                    To = "Hyderabad",
                    DepartureDate = new DateTime(2027, 2, 1, 15, 30, 0, DateTimeKind.Utc),
                    AvailableSeats = 50
                }
            );
        }
    }
}
