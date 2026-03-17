using System;
using BookingHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

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

            booking.HasKey(b => b.Id);

            // SQLite stores Guid as TEXT -- using conversion ensures portability and explicit schema.
            booking.Property(b => b.Id)
                .HasConversion(id => id.ToString(), id => Guid.Parse(id))
                .HasColumnType("TEXT");

            // Store DateTime as TEXT (ISO 8601). EF Core maps DateTime to TEXT in SQLite by default,
            // but explicit column type improves clarity.
            booking.Property(b => b.BookingDate)
                .IsRequired()
                .HasColumnType("TEXT");

            booking.Property(b => b.NumberOfSeats)
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
