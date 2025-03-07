using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booking_API.Migrations
{
    /// <inheritdoc />
    public partial class bookingDates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "BookedDates",
                newName: "StartDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "BookedDates",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "BookedDates");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "BookedDates",
                newName: "Date");
        }
    }
}
