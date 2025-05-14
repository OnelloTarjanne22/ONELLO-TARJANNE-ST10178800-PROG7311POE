using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PROG7311POE_ST10178800.Migrations
{
    /// <inheritdoc />
    public partial class RemoveOthers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Contact",
                table: "Farmers");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Farmers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Contact",
                table: "Farmers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Farmers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
