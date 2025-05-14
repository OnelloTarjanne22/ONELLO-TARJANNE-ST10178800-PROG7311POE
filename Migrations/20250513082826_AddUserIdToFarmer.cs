using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PROG7311POE_ST10178800.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToFarmer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Farmers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Farmers");
        }
    }
}
