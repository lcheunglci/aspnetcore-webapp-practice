using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GigHub.Data.Migrations
{
    public partial class UpdateNotificationToNotCascadeDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserNotifications_AspNetUsers_UserId1",
                table: "UserNotifications");

            migrationBuilder.AddForeignKey(
                name: "FK_UserNotifications_AspNetUsers_UserId1",
                table: "UserNotifications",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserNotifications_AspNetUsers_UserId1",
                table: "UserNotifications");

            migrationBuilder.AddForeignKey(
                name: "FK_UserNotifications_AspNetUsers_UserId1",
                table: "UserNotifications",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
