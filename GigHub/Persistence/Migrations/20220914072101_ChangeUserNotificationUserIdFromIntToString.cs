using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GigHub.Data.Migrations
{
    public partial class ChangeUserNotificationUserIdFromIntToString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserNotifications_AspNetUsers_UserId1",
                table: "UserNotifications");

            migrationBuilder.DropIndex(
                name: "IX_UserNotifications_UserId1",
                table: "UserNotifications");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "UserNotifications");

            migrationBuilder.DropPrimaryKey("PK_UserNotifications", "UserNotifications");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserNotifications",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey("PK_UserNotifications", "UserNotifications", "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserNotifications_UserId",
                table: "UserNotifications",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserNotifications_AspNetUsers_UserId",
                table: "UserNotifications",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserNotifications_AspNetUsers_UserId",
                table: "UserNotifications");

            migrationBuilder.DropIndex(
                name: "IX_UserNotifications_UserId",
                table: "UserNotifications");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "UserNotifications",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "UserNotifications",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_UserNotifications_UserId1",
                table: "UserNotifications",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_UserNotifications_AspNetUsers_UserId1",
                table: "UserNotifications",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
