using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookingHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFlightEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Bookings_Destination_BookingId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Destination_BookingId",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "To",
                table: "Flights",
                newName: "ToLocation");

            migrationBuilder.RenameColumn(
                name: "From",
                table: "Flights",
                newName: "FromLocation");

            migrationBuilder.RenameColumn(
                name: "Destination_Destination_Name",
                table: "Bookings",
                newName: "Destination_Name");

            migrationBuilder.RenameColumn(
                name: "Destination_Destination_Address",
                table: "Bookings",
                newName: "Destination_Address");

            migrationBuilder.AlterColumn<string>(
                name: "ToLocation",
                table: "Flights",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FromLocation",
                table: "Flights",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

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
            migrationBuilder.DeleteData(
                table: "Flights",
                keyColumn: "IdValue",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Flights",
                keyColumn: "IdValue",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.RenameColumn(
                name: "ToLocation",
                table: "Flights",
                newName: "To");

            migrationBuilder.RenameColumn(
                name: "FromLocation",
                table: "Flights",
                newName: "From");

            migrationBuilder.RenameColumn(
                name: "Destination_Name",
                table: "Bookings",
                newName: "Destination_Destination_Name");

            migrationBuilder.RenameColumn(
                name: "Destination_Address",
                table: "Bookings",
                newName: "Destination_Destination_Address");

            migrationBuilder.AlterColumn<string>(
                name: "To",
                table: "Flights",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "From",
                table: "Flights",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<Guid>(
                name: "Destination_BookingId",
                table: "Bookings",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Bookings_Destination_BookingId",
                table: "Bookings",
                column: "Destination_BookingId",
                principalTable: "Bookings",
                principalColumn: "IdValue",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
