using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyShop_Logging.Migrations
{
    /// <inheritdoc />
    public partial class DateStampsFromBaseEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateUpdated",
                table: "Products",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "Products",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "DateUpdated",
                table: "Categories",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "Categories",
                newName: "CreatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Products",
                newName: "DateUpdated");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Products",
                newName: "DateCreated");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Categories",
                newName: "DateUpdated");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Categories",
                newName: "DateCreated");
        }
    }
}
