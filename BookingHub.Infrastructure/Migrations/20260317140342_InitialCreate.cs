using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookingHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    IdValue = table.Column<Guid>(type: "TEXT", nullable: false),
                    BookingDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SeatCount = table.Column<int>(type: "INTEGER", nullable: false),
                    Destination_Name = table.Column<string>(type: "TEXT", nullable: true),
                    Destination_Address = table.Column<string>(type: "TEXT", nullable: true),
                    FlightIdValue = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.IdValue);
                });

            migrationBuilder.CreateTable(
                name: "Flights",
                columns: table => new
                {
                    IdValue = table.Column<Guid>(type: "TEXT", nullable: false),
                    FromLocation = table.Column<string>(type: "TEXT", nullable: false),
                    ToLocation = table.Column<string>(type: "TEXT", nullable: false),
                    DepartureDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AvailableSeats = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flights", x => x.IdValue);
                });

            migrationBuilder.InsertData(
                table: "Flights",
                columns: new[] { "IdValue", "AvailableSeats", "DepartureDate", "FromLocation", "ToLocation" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), 100, new DateTime(2027, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "Delhi", "Mumbai" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), 50, new DateTime(2027, 2, 1, 15, 30, 0, 0, DateTimeKind.Utc), "Bangalore", "Hyderabad" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Flights");
        }
    }
}
