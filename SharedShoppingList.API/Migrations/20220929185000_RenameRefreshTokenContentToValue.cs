using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SharedShoppingList.API.Migrations
{
    public partial class RenameRefreshTokenContentToValue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Token",
                table: "RefreshTokens",
                newName: "Value");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Value",
                table: "RefreshTokens",
                newName: "Token");
        }
    }
}
