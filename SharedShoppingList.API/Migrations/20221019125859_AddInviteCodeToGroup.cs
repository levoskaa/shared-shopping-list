using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SharedShoppingList.API.Migrations
{
    public partial class AddInviteCodeToGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InviteCode",
                table: "UserGroups",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InviteCode",
                table: "UserGroups");
        }
    }
}
