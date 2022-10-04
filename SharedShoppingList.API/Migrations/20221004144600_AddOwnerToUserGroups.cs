using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SharedShoppingList.API.Migrations
{
    public partial class AddOwnerToUserGroups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "UserGroups",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_OwnerId",
                table: "UserGroups",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserGroups_AspNetUsers_OwnerId",
                table: "UserGroups",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserGroups_AspNetUsers_OwnerId",
                table: "UserGroups");

            migrationBuilder.DropIndex(
                name: "IX_UserGroups_OwnerId",
                table: "UserGroups");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "UserGroups");
        }
    }
}
