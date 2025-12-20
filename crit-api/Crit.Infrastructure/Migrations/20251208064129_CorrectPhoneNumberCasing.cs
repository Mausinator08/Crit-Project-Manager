using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crit.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CorrectPhoneNumberCasing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "type",
                table: "PhoneNumber",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "number",
                table: "PhoneNumber",
                newName: "Number");

            migrationBuilder.RenameColumn(
                name: "extension",
                table: "PhoneNumber",
                newName: "Extension");

            migrationBuilder.RenameColumn(
                name: "countryCode",
                table: "PhoneNumber",
                newName: "CountryCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "PhoneNumber",
                newName: "type");

            migrationBuilder.RenameColumn(
                name: "Number",
                table: "PhoneNumber",
                newName: "number");

            migrationBuilder.RenameColumn(
                name: "Extension",
                table: "PhoneNumber",
                newName: "extension");

            migrationBuilder.RenameColumn(
                name: "CountryCode",
                table: "PhoneNumber",
                newName: "countryCode");
        }
    }
}
