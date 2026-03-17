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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var booking = modelBuilder.Entity<Booking>();
            booking.ToTable("Bookings");

            // Prevent EF from trying to map domain Id value object directly.
            booking.Ignore(b => b.Id);

            // Use the primitive IdValue property for the PK so EF can work with a Guid directly.
            booking.HasKey(b => b.IdValue);

            booking.Property(b => b.IdValue)
                .HasColumnType("TEXT");

            // BookingDate -> stored as TEXT/DateTime using converter
            var dateConverter = new ValueConverter<BookingDate, DateTime>(
                v => v.Value,
                v => BookingDate.FromDateTime(v));

            booking.Property(b => b.BookingDate)
                .HasConversion(dateConverter)
                .IsRequired()
                .HasColumnType("TEXT");

            // SeatCount -> INTEGER
            var seatsConverter = new ValueConverter<SeatCount, int>(
                v => v.Value,
                v => SeatCount.FromInt(v));

            booking.Property(b => b.SeatCount)
                .HasConversion(seatsConverter)
                .IsRequired()
                .HasColumnType("INTEGER");

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
        }
    }
}
