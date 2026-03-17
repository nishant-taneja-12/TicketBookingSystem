using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBookingDestination : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Destination_Address",
                table: "Bookings",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Destination_Name",
                table: "Bookings",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Destination_Address",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Destination_Name",
                table: "Bookings");
        }
    }
}
