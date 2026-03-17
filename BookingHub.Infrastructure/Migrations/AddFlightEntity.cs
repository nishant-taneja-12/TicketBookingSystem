using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingHub.Migrations
{
    public partial class AddFlightEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Flights",
                columns: table => new
                {
                    IdValue = table.Column<string>(type: "TEXT", nullable: false),
                    FromLocation = table.Column<string>(type: "TEXT", nullable: false),
                    ToLocation = table.Column<string>(type: "TEXT", nullable: false),
                    DepartureDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AvailableSeats = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flights", x => x.IdValue);
                });

            // Seed initial flights
            migrationBuilder.InsertData(
                table: "Flights",
                columns: new[] { "IdValue", "FromLocation", "ToLocation", "DepartureDate", "AvailableSeats" },
                values: new object[,]
                {
                    { "11111111-1111-1111-1111-111111111111", "Delhi", "Mumbai", new DateTime(2027, 1, 1, 10, 0, 0, DateTimeKind.Utc), 100 },
                    { "22222222-2222-2222-2222-222222222222", "Bangalore", "Hyderabad", new DateTime(2027, 2, 1, 15, 30, 0, DateTimeKind.Utc), 50 }
                }
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Flights",
                keyColumn: "IdValue",
                keyValue: "11111111-1111-1111-1111-111111111111");

            migrationBuilder.DeleteData(
                table: "Flights",
                keyColumn: "IdValue",
                keyValue: "22222222-2222-2222-2222-222222222222");

            migrationBuilder.DropTable(
                name: "Flights");
        }
    }
}
