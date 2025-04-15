using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangedNamingConventionAndPosition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "City",
                table: "MemberAddressEntity",
                newName: "StreetName");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "MemberAddressEntity",
                newName: "CityName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StreetName",
                table: "MemberAddressEntity",
                newName: "City");

            migrationBuilder.RenameColumn(
                name: "CityName",
                table: "MemberAddressEntity",
                newName: "Address");
        }
    }
}
